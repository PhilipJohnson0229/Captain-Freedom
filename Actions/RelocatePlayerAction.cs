using BNG;
using UnityEngine;

public class RelocatePlayerAction : Actions
{
    public Transform playerPos;

    public override void Act()
    {
        BNGPlayerController player = PlayerManager.instance.player;

        if (player != null)
        {
            player.transform.position = playerPos.position;
            player.transform.rotation = playerPos.rotation;
            //player.RB.linearVelocity = Vector3.zero;
        }
    }
}
