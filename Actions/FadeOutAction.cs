using UnityEngine;

public class FadeOutAction : Actions
{
    //public UI_FadeScreen fadeScreen;
    public Actions[] actions;
    public override void Act()
    {
        //if (fadeScreen != null)
        //{
        //    fadeScreen.FadeOut();
        //}
    }

    private void OnEnable()
    {
        //fadeScreen.onFadeOutComplete += PostFadeActions;
    }

    private void OnDisable()
    {
        //fadeScreen.onFadeOutComplete -= PostFadeActions;
    }

    public void PostFadeActions()
    {
        Extensions.RunActions(actions);
    }
}
