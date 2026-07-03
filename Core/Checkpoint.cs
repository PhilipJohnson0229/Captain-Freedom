using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Animator anim;
    public string id;
    public float animationDuration, loadWaitTime;
    public bool activationStatus, initialAnimationPlayed;
    public ActivateGameObjectAction[] activateGameObjectActions;

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<BNGPlayerController>() != null && !activationStatus)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if (activationStatus == false)
            //AudioManager.instance.PlaySFX(4, transform);


        activationStatus = true;

        Extensions.RunActions(activateGameObjectActions);

        if (!initialAnimationPlayed && anim != null)
        {
            StartCoroutine(playActivationAnimation(animationDuration));
        }
        //TODO we need to play an animation where we sit down and reset the scene
    }

    public void ActivateCheckpointOnLoad(bool status)
    {
        if (activationStatus == false)
            //AudioManager.instance.PlaySFX(4, transform);


            activationStatus = true;

        Extensions.RunActions(activateGameObjectActions);

        SetAnimationStatus(status);

        if (!initialAnimationPlayed && anim != null)
        {
            StartCoroutine(playActivationAnimation(animationDuration));
        }
        //TODO we need to play an animation where we sit down and reset the scene
    }

    public void SetAnimationStatus(bool status)
    {
        initialAnimationPlayed = status;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }

    IEnumerator playActivationAnimation(float duration)
    {
        float animationTime = duration;
        anim.SetBool("Play", true);
        while (animationTime > 0) 
        {
            yield return new WaitForSeconds(.2f);
            animationTime -= .1f; 
        }
        anim.SetBool("Play", false);
        initialAnimationPlayed = true;
    }

    IEnumerator waitForLoad()
    {
        yield return new WaitForSeconds(loadWaitTime);
    }
}
