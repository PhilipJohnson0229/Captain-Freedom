using UnityEngine;
using BNG;

public class PowerLoadoutManager : MonoBehaviour
{
    [Header("Gun Power")]
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private bool gunUnlocked = true;

    [Header("Jet Power")]
    [SerializeField] private GameObject jetPrefab;
    [SerializeField] private bool jetUnlocked = true;

    [Header("VRIF Grabbers")]
    [SerializeField] private Grabber leftGrabber;
    [SerializeField] private Grabber rightGrabber;

    [Header("Summon Settings")]
    [SerializeField] private bool releaseExistingHeldItem = true;
    [SerializeField] private Vector3 localSpawnPositionOffset;
    [SerializeField] private Vector3 localSpawnRotationOffset;

    public void TrySummonGun(HandSide handSide)
    {
        GameManager.instance.Log($"Ttying to summon gun into {handSide} hand.");
        if (!gunUnlocked)
        {
            Debug.Log("Gun is not unlocked yet.");
            return;
        }

        if (gunPrefab == null)
        {
            Debug.LogWarning("No gun prefab assigned.");
            return;
        }

        Grabber grabber = GetGrabber(handSide);

        if (grabber == null)
        {
            Debug.LogWarning($"No grabber assigned for {handSide} hand.");
            return;
        }

        if (releaseExistingHeldItem && grabber.HeldGrabbable != null)
        {
            grabber.TryRelease();
        }

        Vector3 spawnPosition = grabber.transform.TransformPoint(localSpawnPositionOffset);
        Quaternion spawnRotation = grabber.transform.rotation * Quaternion.Euler(localSpawnRotationOffset);

        GameObject gunInstance = Instantiate(gunPrefab, spawnPosition, spawnRotation);

        Grabbable grabbable = gunInstance.GetComponent<Grabbable>();

        if (grabbable == null)
        {
            Debug.LogWarning("Summoned gun prefab does not have a Grabbable component.");
            return;
        }

        grabber.GrabGrabbable(grabbable);

        GameManager.instance.Log($"Summoned gun into {handSide} hand.");
    }

    public void TrySummonJet(HandSide handSide)
    {
        GameManager.instance.Log($"Ttying to summon jet into {handSide} hand.");
        if (!jetUnlocked)
        {
            Debug.Log("Jet is not unlocked yet.");
            return;
        }

        if (jetPrefab == null)
        {
            Debug.LogWarning("No jet prefab assigned.");
            return;
        }

        Grabber grabber = GetGrabber(handSide);

        if (grabber == null)
        {
            Debug.LogWarning($"No grabber assigned for {handSide} hand.");
            return;
        }

        if (releaseExistingHeldItem && grabber.HeldGrabbable != null)
        {
            grabber.TryRelease();
        }

        Vector3 spawnPosition = grabber.transform.TransformPoint(localSpawnPositionOffset);
        Quaternion spawnRotation = grabber.transform.rotation * Quaternion.Euler(localSpawnRotationOffset);

        GameObject jetInstance = Instantiate(jetPrefab, spawnPosition, spawnRotation);

        Grabbable grabbable = jetInstance.GetComponent<Grabbable>();

        if (grabbable == null)
        {
            Debug.LogWarning("Summoned jet prefab does not have a Grabbable component.");
            return;
        }

        grabber.GrabGrabbable(grabbable);

        GameManager.instance.Log($"Summoned jet into {handSide} hand.");
    }

    private Grabber GetGrabber(HandSide handSide)
    {
        return handSide == HandSide.Left ? leftGrabber : rightGrabber;
    }
}