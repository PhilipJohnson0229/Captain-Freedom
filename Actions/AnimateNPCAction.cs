using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
public class AnimateNPCAction : Actions
{
    [SerializeField]
    List<AnimParameter> anims = new List<AnimParameter>();

    [SerializeField]
    List<Actions> actions = new List<Actions>();


    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        for (int i = 0; i < anims.Count; i++)
        {
            anims[i].IinitHashId();
        }
    }

    public override void Act()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        int i = 0;
        while (i < anims.Count)
        {
            yield return new WaitForSeconds(anims[i].InvookeDelay);

            animator.SetTrigger(anims[i].HashId);

            i++;

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(animator.GetNextAnimatorStateInfo(0).length);
        }

        for (int j = 0; j < actions.Count; j++)
        {
            actions[j].Act();
        }
    }
}

[System.Serializable]
public class AnimParameter
{
    //this will be used to go from one animation state to another
    [SerializeField]
    string triggerName;
    [SerializeField]
    float invokeDelay;


    public float InvookeDelay { get { return invokeDelay; } }

    //this is more performant
    public int HashId { get; private set; }

    public void IinitHashId()
    {
        HashId = Animator.StringToHash(triggerName);
    }
}