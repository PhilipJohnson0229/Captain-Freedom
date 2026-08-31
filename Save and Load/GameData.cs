using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, GameScene> scenes;
    public SerializableDictionary<string, float> volumeSettings;
    public string closestCheckpointId;

    #region Powers
    public bool shieldUnlocked;
    public bool gunUnlocked;
    public bool swordUnlocked;
    public bool handJetUnlocked;

    public int shieldLevel;
    public int gunLevel;
    public int swordLevel;
    public int handJetLevel;
    #endregion
    #region Inventory
    //public SerializableDictionary<string, InventoryItemDetails> inventory;
    //public SerializableDictionary<string, int> equipment;
    //public List<string> eId;
    //public SerializableDictionary<string, string> snapZoneIds;
    #endregion
    public GameData()
    {
        scenes = new SerializableDictionary<string, GameScene>();
        volumeSettings = new SerializableDictionary<string, float>();

        shieldUnlocked = false;
        gunUnlocked = false;
        swordUnlocked = false;
        handJetUnlocked = false;

        gunLevel = 0;
        swordLevel = 0;
        handJetLevel = 0;
        shieldLevel = 0;
        #region Inventory
        //inventory = new SerializableDictionary<string, InventoryItemDetails>();
        //equipment = new SerializableDictionary<string, int>();
        //eId = new List<string>();
        //snapZoneIds = new SerializableDictionary<string, string>();
        #endregion
    }
}

[System.Serializable]
public class GameScene
{
    public string currentSceneName;
    public SerializableDictionary<string, bool> checkpoints;
    public SerializableDictionary<string, bool> checkpointsInitialAnimations;
    public string closestCheckpointId;
    public SerializableDictionary<string, bool> pickupsInScene;
    public GameScene()
    {
        currentSceneName = string.Empty;
        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
        checkpointsInitialAnimations = new SerializableDictionary<string, bool>();
        pickupsInScene = new SerializableDictionary<string, bool>();
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