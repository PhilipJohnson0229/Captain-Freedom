using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LightSwitchAction : Actions
{
    public Texture2D[] darkLightMapDir, darkLightMapColor;
    public Texture2D[] brightLightMapDir, brightLightMapColor;

    public LightmapData[] darkLightMap, brightLightMap;

    public LightMatSwitchContainer[] lightMatSwitchContainers;

    public bool switched = false;

    public GameObject lightSwitchToggle;
    public Vector3 onPosition, offPosition;
    public Quaternion onRotation, offRotation;

    public float fogDensityOn = 0.06f;   // Fog density when lights are ON
    public float fogDensityOff = 0.117f;   // Fog density when lights are OFF

    private void SetSwitch()
    {
        List<LightmapData> dLightMap = new List<LightmapData>();

        for (int i = 0; i < darkLightMapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();

            lmdata.lightmapDir = darkLightMapDir[i];
            lmdata.lightmapColor = darkLightMapColor[i];

            dLightMap.Add(lmdata);
        }

        darkLightMap = dLightMap.ToArray();

        List<LightmapData> blightmap = new List<LightmapData>();

        for (int i = 0; i < brightLightMapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();

            lmdata.lightmapDir = brightLightMapDir[i];
            lmdata.lightmapColor = brightLightMapColor[i];

            blightmap.Add(lmdata);
        }

        brightLightMap = blightmap.ToArray();

    }

    public override void Act()
    {
        //Turn on and off lights
        SetSwitch();
        switched = !switched;
        PerformSwitch(switched);
    }

    private void PerformSwitch(bool switched)
    {
        if (switched)
        {
            LightmapSettings.lightmaps = brightLightMap;
            foreach(LightMatSwitchContainer container in lightMatSwitchContainers)
            {
                container.mesh.material = container.lightMat;
            }

            lightSwitchToggle.transform.localRotation = onRotation;
            lightSwitchToggle.transform.localPosition = onPosition;
        }
        else
        {
            LightmapSettings.lightmaps = darkLightMap;
            foreach (LightMatSwitchContainer container in lightMatSwitchContainers)
            {
                container.mesh.material = container.darkMat;
            }

            lightSwitchToggle.transform.localRotation = offRotation;
            lightSwitchToggle.transform.localPosition = offPosition;
        }

        RenderSettings.fog = true;
        RenderSettings.fogDensity = switched ? fogDensityOn : fogDensityOff;
    }

    public void LoadSwitch(bool savedSwitchStatus)
    {
        Debug.Log("Tryitng to load switch with lights off");
        SetSwitch();

        switched = savedSwitchStatus;

        PerformSwitch(switched);
    }
}

[System.Serializable]
public class LightMatSwitchContainer
{
    public Renderer mesh;
    public Material lightMat, darkMat;
}
