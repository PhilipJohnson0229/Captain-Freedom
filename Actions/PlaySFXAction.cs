using UnityEngine;

public class PlaySFXAction : Actions
{
    public int soundToPlay;
    public Transform source;
    public override void Act()
    {
        if(source != null) 
            AudioManager.instance.PlaySFX(soundToPlay, source);
        else
            AudioManager.instance.PlaySFX(soundToPlay, null);
    }

   
}
