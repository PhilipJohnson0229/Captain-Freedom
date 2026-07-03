using UnityEngine;

public class LockPlayerAction : Actions
{
    public bool locked;
    public bool lockPlayerCamRotation;
    public override void Act()
    {
        if (locked)
        {
            PlayerManager.instance.isInteracting = true;

            if (lockPlayerCamRotation) 
            {
                PlayerManager.instance.freeLook = false;
                //PlayerManager.instance.player.SetCursorLockState(false);
            }
            else
            {
                PlayerManager.instance.freeLook = true;
                //PlayerManager.instance.player.SetCursorLockState(true);
            }
        }
        else
        {
            PlayerManager.instance.isInteracting = false;
            //PlayerManager.instance.player.SetCursorLockState(true);
            PlayerManager.instance.freeLook = true;
        }


    }
}
