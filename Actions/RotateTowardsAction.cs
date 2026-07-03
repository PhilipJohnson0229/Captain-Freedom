using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateTowardsAction : Actions
{
    public Transform target;  // Transform to rotate towards
    public Transform rotated;
    public Actions[] completionActions;
    public override void Act()
    {
        Vector3 rotation = Quaternion.LookRotation(target.transform.position - rotated.transform.position).eulerAngles;
        rotation.x = 0f;

        rotated.transform.rotation = Quaternion.Euler(rotation);

        if (completionActions != null && completionActions.Length > 0)
            Extensions.RunActions(completionActions);
    }
}
