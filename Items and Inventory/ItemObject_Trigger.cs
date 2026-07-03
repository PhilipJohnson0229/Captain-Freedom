using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class ItemObject_Trigger : Grabbable
{
    public string itId;

    public ActivateGameObjectAction[] activationActions;

    [ContextMenu("Generate IT ID")]
    private void GenerateId()
    {
        itId = System.Guid.NewGuid().ToString();
    }

    //public override void Interact()
    //{
    //    Debug.Log("Trying to pick up item ");
    //    //HandleCursor(false);

    //    if (!activated)
    //    {
    //        Extensions.RunActions(actions);
    //    }

    //    ActivateItemTrigger();
    //}

    public void ActivateItemTrigger()
    {
        Debug.Log("Trying to activate this item");
        //SetActivated(true);
        Extensions.RunActions(activationActions);
    }
}
