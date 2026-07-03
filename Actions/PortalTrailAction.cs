using UnityEngine;

public class PortalTrailAction : Actions
{
    public AfterimageFX effect;

    public bool setActive;
    public override void Act()
    {
        if (setActive)
        {
            effect.isActive = true;
        }
        else
        {
            effect.isActive = false;
        }
       
    }
}
