using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointOfInterest :  Grabbable
{
    public string poiId;
    //public Transform entryPoint;
    public PositionNodes[] positionNodes;
    public GameObject poiVisual;
    public Animator buttonAnim;
    public ItemData itemData;
    public bool isButton;
    public bool objectiveComplete = false; // these bools dont need to be saved its just for the puzzle at runtime
    public int catMouthObjectIndex, activationAudio;
   

    [ContextMenu("Generate POI ID")]
    private void GenerateId()
    {
        poiId = System.Guid.NewGuid().ToString();
    }

    //public override void Interact()
    //{
    //    if (activated) return;
    //    SetActivated(true);
    //    AudioManager.instance.PlaySFX(activationAudio, null);
    //    Companion cat = PlayerManager.instance.companion;
    //    cat.poi = this;
    //    //this is where the cat begins his task to investigate
    //    cat.StateMachine.ChangeState(cat.InvestigateState);
    //}

    public void GrabItem()
    {
        //ActivatePointOfInterest();
        //HandleCursor(false);
        //if (buttonAnim != null)
        //{
        //    buttonAnim.SetBool("Activate", true);
        //}
    }

    public void ActivatePointOfInterest()
    {
        //SetActivated(true);
        //objectiveComplete = true;

        //if(poiVisual != null)
        //    poiVisual.SetActive(false);

        //if (buttonAnim != null)
        //{
        //    buttonAnim.SetBool("Activate", true);
        //}

        //Extensions.RunActions(actions);
    }
}

[System.Serializable]
public class PositionNodes
{
    public Transform node;
    public float minDetectionDistance;
    public float jumpLerpingSpeed;
    public bool requiresJumpCheck;
    public bool goal;
}
