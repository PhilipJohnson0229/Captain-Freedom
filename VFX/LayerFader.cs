using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerFader : MonoBehaviour
{
    public string fadeLayerName = "MirrorWorld";
    public float fadeDuration = 2.0f;

    private int fadeLayer;
    [SerializeField]
    private Material[] fadeMaterials;
    private float fadeAmount = 0.0f;
    private bool isFading = false;
    [SerializeField]
    public event Action onFadeCompleted;
    public float startingValue;

    void Start()
    {
        // Get the layer index
        fadeLayer = LayerMask.NameToLayer(fadeLayerName);
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeToBlack());
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeFromBlack());
    }

    public void ResetFade()
    {
        foreach (var mat in fadeMaterials)
        {
            mat.SetFloat("_FadeAmount", 0);
        }
    }

    private IEnumerator FadeToBlack()
    {
        isFading = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeAmount = elapsedTime / fadeDuration;

            // Update fade amount on all materials
            foreach (var mat in fadeMaterials)
            {
                mat.SetFloat("_FadeAmount", fadeAmount);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure fade amount is fully applied
        fadeAmount = 1.0f;
        foreach (var mat in fadeMaterials)
        {
            mat.SetFloat("_FadeAmount", fadeAmount);
        }

        isFading = false;

        if(onFadeCompleted != null)
            onFadeCompleted?.Invoke();
    }

    private IEnumerator FadeFromBlack()
    {
        isFading = true;

        float elapsedTime = 1f;
        while (elapsedTime > 0)
        {
            fadeAmount = elapsedTime;

            // Update fade amount on all materials
            foreach (var mat in fadeMaterials)
            {
                mat.SetFloat("_FadeAmount", fadeAmount);
            }

            elapsedTime -= Time.deltaTime;

            yield return null;
        }

        // Ensure fade amount is fully applied
        fadeAmount = 0.0f;
        foreach (var mat in fadeMaterials)
        {
            mat.SetFloat("_FadeAmount", fadeAmount);
        }

        isFading = false;

        if (onFadeCompleted != null)
            onFadeCompleted?.Invoke();
    }

    private void OnDisable()
    {
        foreach (var mat in fadeMaterials)
        {
            mat.SetFloat("_FadeAmount", startingValue);
        }
    }
}
