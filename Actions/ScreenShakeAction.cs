using UnityEngine;

public class ScreenShakeAction : Actions
{
    public ScreenShake screenShake;
    public bool shake;
    public float shakeDuration, shakeAmount;
    public override void Act()
    {
        Debug.Log("Trying to set off the screen shake");
        if (screenShake == null) return;

        screenShake.shakeAmount = shakeAmount;

        screenShake.shakeCam = shake;

        screenShake.shake = shakeDuration;
    }
}
