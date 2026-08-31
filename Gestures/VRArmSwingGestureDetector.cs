using UnityEngine;
using BNG;

public enum ArmSwingGesture
{
    None,
    Outward,
    Inward,
    Upward,
    Downward,
    Forward,
    Backward
}

public enum HandSide
{
    Left,
    Right
}

public class VRArmSwingGestureDetector : MonoBehaviour
{
    [Header("Hand Setup")]
    [SerializeField] private HandSide handSide;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform playerHead;

    [Header("Input")]
    [SerializeField] private bool useDebugGrabHeld;
    [SerializeField] private bool debugGrabHeld;

    [Header("Gesture Tuning")]
    [SerializeField] private float minSwingDistance = 0.22f;
    [SerializeField] private float minSwingSpeed = 0.75f;
    [SerializeField] private float requiredDominance = 1.25f;
    [SerializeField] private float maxGestureTime = 0.9f;
    [SerializeField] private float gestureCooldown = 0.35f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private Vector3 gestureStartPosition;
    private Vector3 previousHandPosition;
    private float gestureStartTime;
    private float cooldownTimer;
    private bool wasGrabHeld;
    private bool gestureAlreadyDetected;

    private void Update()
    {
        if (handTransform == null)
            return;

        cooldownTimer -= Time.deltaTime;

        bool grabHeld = IsGrabHeld();

        //if (grabHeld)
        //{
        //    ControllerHand controllerHand = handSide == HandSide.Right
        //    ? ControllerHand.Right
        //    : ControllerHand.Left;
        //    GameManager.instance.Log($"Controller Hand: {controllerHand} Grab Activated");
        //}
        if (grabHeld && !wasGrabHeld)
        {
            BeginGesture();
        }

        if (grabHeld && wasGrabHeld)
        {
            TrackGesture();
        }

        if (!grabHeld && wasGrabHeld)
        {
            EndGesture();
        }

        wasGrabHeld = grabHeld;
        previousHandPosition = handTransform.position;
    }

    private bool IsGrabHeld()
    {
        if (InputBridge.Instance == null)
            return false;

        ControllerHand controllerHand = handSide == HandSide.Right
            ? ControllerHand.Right
            : ControllerHand.Left;
        

        return InputBridge.Instance.GetGrabbedControllerBinding(
            GrabbedControllerBinding.Grip,
            controllerHand
        );
    }

    private void BeginGesture()
    {
        gestureStartPosition = handTransform.position;
        previousHandPosition = handTransform.position;
        gestureStartTime = Time.time;
        gestureAlreadyDetected = false;

        if (showDebugLogs)
            Debug.Log($"{handSide} gesture watch started.");
    }

    private void TrackGesture()
    {
        if (gestureAlreadyDetected)
            return;

        if (cooldownTimer > 0f)
            return;

        float elapsed = Time.time - gestureStartTime;

        if (elapsed > maxGestureTime)
        {
            // Restart the gesture window while grip is still held.
            BeginGesture();
            return;
        }

        Vector3 worldDelta = handTransform.position - gestureStartPosition;
        Vector3 localDelta = GetPlayerRelativeDelta(worldDelta);

        float distance = localDelta.magnitude;

        Vector3 frameDelta = handTransform.position - previousHandPosition;
        float speed = frameDelta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);

        if (distance < minSwingDistance)
            return;

        if (speed < minSwingSpeed)
            return;

        ArmSwingGesture gesture = ClassifyGesture(localDelta);

        if (gesture == ArmSwingGesture.None)
            return;

        gestureAlreadyDetected = true;
        cooldownTimer = gestureCooldown;

        OnGestureDetected(gesture);
    }

    private void EndGesture()
    {
        if (showDebugLogs)
            Debug.Log($"{handSide} gesture watch ended.");

        gestureAlreadyDetected = false;
    }

    private ArmSwingGesture ClassifyGesture(Vector3 localDelta)
    {
        float absX = Mathf.Abs(localDelta.x);
        float absY = Mathf.Abs(localDelta.y);
        float absZ = Mathf.Abs(localDelta.z);

        bool xDominant = absX > absY * requiredDominance && absX > absZ * requiredDominance;
        bool yDominant = absY > absX * requiredDominance && absY > absZ * requiredDominance;
        bool zDominant = absZ > absX * requiredDominance && absZ > absY * requiredDominance;

        if (xDominant)
        {
            bool movedRight = localDelta.x > 0f;

            if (handSide == HandSide.Right)
                return movedRight ? ArmSwingGesture.Outward : ArmSwingGesture.Inward;

            return movedRight ? ArmSwingGesture.Inward : ArmSwingGesture.Outward;
        }

        if (yDominant)
        {
            return localDelta.y > 0f
                ? ArmSwingGesture.Upward
                : ArmSwingGesture.Downward;
        }

        if (zDominant)
        {
            return localDelta.z > 0f
                ? ArmSwingGesture.Forward
                : ArmSwingGesture.Backward;
        }

        return ArmSwingGesture.None;
    }

    private Vector3 GetPlayerRelativeDelta(Vector3 worldDelta)
    {
        if (playerHead == null)
            return worldDelta;

        Vector3 playerForward = playerHead.forward;
        playerForward.y = 0f;
        playerForward.Normalize();

        Vector3 playerRight = playerHead.right;
        playerRight.y = 0f;
        playerRight.Normalize();

        float rightAmount = Vector3.Dot(worldDelta, playerRight);
        float upAmount = Vector3.Dot(worldDelta, Vector3.up);
        float forwardAmount = Vector3.Dot(worldDelta, playerForward);

        return new Vector3(rightAmount, upAmount, forwardAmount);
    }

    private void OnGestureDetected(ArmSwingGesture gesture)
    {
        GameManager.instance.Log($"{handSide} hand gesture detected: {gesture}");

        // Next step will be:
        // powerLoadoutManager.TrySummon(gesture, handSide);
    }
}