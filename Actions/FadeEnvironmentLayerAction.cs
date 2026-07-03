using UnityEngine;

public class FadeEnvironmentLayerAction : Actions
{
    public LayerFader layerFader;
    public Actions[] fadeCompleteActions;
    public bool fadeIn;
    public override void Act()
    {
        if (fadeIn)
        {
            layerFader.StartFadeIn();
        }
        else
        {
            layerFader.StartFadeOut();
        }
        
    }

    private void OnEnable()
    {
        layerFader.onFadeCompleted += RunFadeCompleteActions;
    }

    private void OnDisable()
    {
        layerFader.onFadeCompleted -= RunFadeCompleteActions;
    }

    public void RunFadeCompleteActions()
    {
        Extensions.RunActions(fadeCompleteActions);
    }
}
