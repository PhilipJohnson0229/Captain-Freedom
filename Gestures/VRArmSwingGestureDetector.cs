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

public enum PowerGestureSlot
{
    None,

    LeftUp,
    LeftDown,
    LeftSide,

    RightUp,
    RightDown,
    RightSide
}

public class VRArmSwingGestureDetector : MonoBehaviour
{
    [Header("Hand Setup")]
    [SerializeField] private HandSide handSide;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Transform playerHead;
    [SerializeField] private float minRadialChange = 0.12f;
    [SerializeField] private float minHorizontalSwingDistance = 0.15f;

    [Header("Rotation Correction")]
    [SerializeField] private bool flipSideGesturesOnNegativeYaw = true;
    private Vector3 startOutwardDirection;

    [Header("Input")]
    [SerializeField] private bool useDebugGrabHeld;
    [SerializeField] private bool debugGrabHeld;

    [Header("Gesture Tuning")]
    [SerializeField] private float minSwingDistance = 0.22f;
    [SerializeField] private float minSwingSpeed = 0.75f;
    [SerializeField] private float requiredDominance = 1.25f;
    [SerializeField] private float maxGestureTime = 0.9f;
    [SerializeField] private float gestureCooldown = 0.35f;

    [Header("Power Loadout")]
    [SerializeField] private PowerLoadoutManager powerLoadoutManager;


    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private Vector3 gestureStartPosition;
    private Vector3 previousHandPosition;
    private float gestureStartTime;
    private float cooldownTimer;
    private bool wasGrabHeld;
    private bool gestureAlreadyDetected;
    private float startBodyDistance;

    private void Update()
    {
        if (handTransform == null)
            return;

        cooldownTimer -= Time.deltaTime;

        bool grabHeld = IsGrabHeld();

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

        startOutwardDirection = GetStartOutwardDirection();

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

    private Vector3 GetStartOutwardDirection()
    {
        Vector3 bodyPosition = bodyTransform != null
            ? bodyTransform.position
            : transform.position;

        Vector3 direction = gestureStartPosition - bodyPosition;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            // Fallback if hand/body are weirdly aligned.
            if (bodyTransform != null)
            {
                return handSide == HandSide.Right
                    ? bodyTransform.right
                    : -bodyTransform.right;
            }

            return handSide == HandSide.Right
                ? Vector3.right
                : Vector3.left;
        }

        return direction.normalized;
    }

    private ArmSwingGesture ClassifyGesture(Vector3 worldDelta)
    {
        float verticalAmount = worldDelta.y;
        float absVertical = Mathf.Abs(verticalAmount);

        Vector3 horizontalDelta = worldDelta;
        horizontalDelta.y = 0f;

        float horizontalDistance = horizontalDelta.magnitude;

        // Up / down still use world Y because those are already working.
        if (absVertical >= minSwingDistance &&
            absVertical > horizontalDistance * requiredDominance)
        {
            return verticalAmount > 0f
                ? ArmSwingGesture.Upward
                : ArmSwingGesture.Downward;
        }

        if (horizontalDistance < minHorizontalSwingDistance)
            return ArmSwingGesture.None;

        Vector3 outwardAxis = GetOutwardAxisForHand();

        float outwardAmount = Vector3.Dot(horizontalDelta.normalized, outwardAxis);

        // Require the motion to be clearly along inward/outward axis.
        if (Mathf.Abs(outwardAmount) < 0.55f)
            return ArmSwingGesture.None;

        return outwardAmount > 0f
            ? ArmSwingGesture.Outward
            : ArmSwingGesture.Inward;
    }

    private Vector3 GetOutwardAxisForHand()
    {
        Transform reference = bodyTransform != null ? bodyTransform : playerHead;

        if (reference == null)
        {
            return handSide == HandSide.Right ? Vector3.right : Vector3.left;
        }

        // Use yaw only. Ignore pitch/roll.
        Vector3 right = reference.right;
        right.y = 0f;

        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.right;

        right.Normalize();

        // Right hand outward is body right.
        // Left hand outward is body left.
        return handSide == HandSide.Right ? right : -right;
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
        PowerGestureSlot gestureSlot = GetGestureSlot(gesture);

        if (GameManager.instance != null)
        {
            GameManager.instance.Log($"{handSide} hand gesture detected: {gesture}. Gesture Slot Activated: {gestureSlot}");
        }

        if (gestureSlot == PowerGestureSlot.None)
            return;

        switch (gestureSlot)
        {
            case PowerGestureSlot.RightUp:
                if (powerLoadoutManager != null)
                {
                    powerLoadoutManager.TrySummonGun(handSide);
                }
                else
                {
                    GameManager.instance.Log("No PowerLoadoutManager assigned.");
                }
                break;

            case PowerGestureSlot.LeftUp:
                if (powerLoadoutManager != null)
                {
                    powerLoadoutManager.TrySummonJet(handSide);
                }
                else
                {
                    GameManager.instance.Log("No PowerLoadoutManager assigned.");
                }
                break;

            case PowerGestureSlot.LeftDown:
                GameManager.instance.Log("LeftDown slot activated. No power assigned yet.");
                break;

            case PowerGestureSlot.LeftSide:
                GameManager.instance.Log("LeftSide slot activated. No power assigned yet.");
                break;

            case PowerGestureSlot.RightDown:
                GameManager.instance.Log("RightDown slot activated. No power assigned yet.");
                break;

            case PowerGestureSlot.RightSide:
                GameManager.instance.Log("RightSide slot activated. No power assigned yet.");
                break;
        }
    }

    private PowerGestureSlot GetGestureSlot(ArmSwingGesture gesture)
    {
        switch (gesture)
        {
            case ArmSwingGesture.Upward:
                return handSide == HandSide.Left
                    ? PowerGestureSlot.LeftUp
                    : PowerGestureSlot.RightUp;

            case ArmSwingGesture.Downward:
                return handSide == HandSide.Left
                    ? PowerGestureSlot.LeftDown
                    : PowerGestureSlot.RightDown;

            case ArmSwingGesture.Inward:
            case ArmSwingGesture.Outward:
                return handSide == HandSide.Left
                    ? PowerGestureSlot.LeftSide
                    : PowerGestureSlot.RightSide;

            default:
                return PowerGestureSlot.None;
        }
    }
}

