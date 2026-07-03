using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, GameScene> scenes;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, int> equipment;
    public List<string> eId;
    public SerializableDictionary<string, string> snapZoneIds;
    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        scenes = new SerializableDictionary<string, GameScene>();
        inventory = new SerializableDictionary<string, int>();
        equipment = new SerializableDictionary<string, int>();
        eId = new List<string>();
        snapZoneIds = new SerializableDictionary<string, string>();
        volumeSettings = new SerializableDictionary<string, float>();
    }
}

[System.Serializable]
public class GameScene
{
    public string currentSceneName;

    public SerializableDictionary<string, bool> equipmentItemsInScene;
    public SerializableDictionary<string, bool> checkpoints;
    public SerializableDictionary<string, bool> checkpointsInitialAnimations;
    public string closestCheckpointId;

    public GameScene()
    {
        currentSceneName = string.Empty;
        equipmentItemsInScene = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
        checkpointsInitialAnimations = new SerializableDictionary<string, bool>();
    }
}

[System.Serializable]
public class InventoryItemDetails
{
    public int stackSize;
    public string snapParentId;

    public InventoryItemDetails()
    {
        snapParentId = string.Empty;
        stackSize = 0;
    }
}