using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchSceneAction : Actions
{
    public string sceneName;
    public float waitTime = 2f;
    public Actions[] actions;
    public override void Act()
    {
        PlayerManager.instance.currentScene = SceneManager.GetActiveScene().name;
        Extensions.RunActions(actions);
        StartCoroutine(Fade(waitTime));
    }

    private IEnumerator Fade(float waitTime)
    {        
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }
}
