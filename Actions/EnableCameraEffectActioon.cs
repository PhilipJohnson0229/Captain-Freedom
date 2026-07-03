using System.Collections;
//using Picturesque.Darkbringer;
using UnityEngine;

public class EnableCameraEffectActioon : Actions
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public DarkbringerEffect effect;

    public float effectset = 1.5f;
    public float effectStart = 7f;
    [SerializeField]
    float currentPixelSize;
    public override void Act()
    {
        //effect.enabled = true;
        StartCoroutine(easeInRoutine(1f));
    }

    IEnumerator easeInRoutine(float duration)
    {
        currentPixelSize = effectStart;

        while (duration > 0) {
            duration -= 0.1f;
            currentPixelSize -= 0.2f;
            //effect.screenStretching.x = currentPixelSize;
            yield return new WaitForSeconds(0.15f);
        }

        currentPixelSize = effectset;
        //effect.screenStretching.x = currentPixelSize;
    }
}
