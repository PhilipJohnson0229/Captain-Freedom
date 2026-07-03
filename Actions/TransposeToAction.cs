using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TransposeToAction : Actions
{
    public Transform objectToMove, newLocation;
    public bool  hasNavAgent;
    public Actions[] actions;

    public override void Act()
    {
        MoveToPosition();
    }

    public void MoveToPosition()
    {
        if (hasNavAgent)
        {
            NavMeshAgent agent = objectToMove.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.nextPosition = newLocation.position;
                agent.Warp(newLocation.position);
                agent.updatePosition = true;
            }
        }
        else
        {
            objectToMove.position = newLocation.position;
            objectToMove.rotation = newLocation.rotation;
        }

        Extensions.RunActions(actions);
    }
}
