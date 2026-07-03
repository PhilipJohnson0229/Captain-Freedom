using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private bool pausedGame;
    [field:SerializeField] public Inventory inventory {  get; private set; }
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private EquipmentItem[] sceneItems;
    [SerializeField] private string closestCheckpointId;
    [SerializeField] private Transform player;
    //public UI ui;

    #region Debug
    public TMP_Text log;
    #endregion
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

    }

    private void Start()
    {
        Log("System initializing...");
        checkpoints = FindObjectsOfType<Checkpoint>();
        sceneItems = FindObjectsOfType<EquipmentItem>();
        inventory = GetComponent<Inventory>();
        player = PlayerManager.instance.player.transform;

        Log("System initialized");
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            pausedGame = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            pausedGame = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 1;
        }
    }

    //basically this is short hand for return this single line piece of code
    //in this case we simply return the boolean
    public bool IsPaused() => pausedGame;

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    //This is where we build the JSON object for our save file
    public void SaveData(GameData _data)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        if (_data.scenes.TryGetValue(activeScene, out GameScene value))
        {

            Debug.Log("Saving...");

            value.currentSceneName = activeScene;


            if (FindClosestCheckpoint() != null)
                value.closestCheckpointId = FindClosestCheckpoint().id;

            value.checkpoints.Clear();

            foreach (Checkpoint checkpoint in checkpoints)
            {
                value.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
            }

            value.checkpointsInitialAnimations.Clear();

            foreach (Checkpoint checkpoint in checkpoints)
            {
                value.checkpointsInitialAnimations.Add(checkpoint.id, checkpoint.initialAnimationPlayed);
            }

            value.equipmentItemsInScene.Clear();

            foreach (EquipmentItem g in sceneItems)
            {
                value.equipmentItemsInScene.Add(g.eId, g.activated);
            }
        }
        else
        {
            Debug.Log($"Saving New Scene {activeScene}...");

            GameScene gameScene = new GameScene();
            gameScene.currentSceneName = activeScene;


            if (FindClosestCheckpoint() != null)
                gameScene.closestCheckpointId = FindClosestCheckpoint().id;

            foreach (Checkpoint checkpoint in checkpoints)
            {
                gameScene.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
            }

            foreach (Checkpoint checkpoint in checkpoints)
            {
                gameScene.checkpointsInitialAnimations.Add(checkpoint.id, checkpoint.initialAnimationPlayed);
            }

            foreach (EquipmentItem g in sceneItems)
            {
                gameScene.equipmentItemsInScene.Add(g.eId, g.activated);
            }

            _data.scenes.Add(activeScene, gameScene);
        }
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        Debug.Log("Trying to load closest checkpoint...");
        if (_data.scenes.TryGetValue(SceneManager.GetActiveScene().name, out GameScene value))
        {
            if (value.closestCheckpointId == null)
                return;

            closestCheckpointId = value.closestCheckpointId;

            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (closestCheckpointId == checkpoint.id)
                {
                    if (closestCheckpointId == checkpoint.id)
                    {
                        Debug.Log($"Moving Player to checkpoint: {checkpoint.transform.position}");

                        PlayerTeleport teleport = PlayerManager.instance.player.GetComponent<PlayerTeleport>();

                        if (teleport != null)
                        {
                            Debug.Log($"Were using the teleport to get to {checkpoint.transform.position}");
                            teleport.TeleportPlayerToTransform(checkpoint.transform);
                        }
                        else
                        {
                            Debug.LogWarning("No PlayerTeleport found on player. Falling back to manual move.");
                            PlayerManager.instance.player.transform.SetPositionAndRotation(
                                checkpoint.transform.position,
                                checkpoint.transform.rotation
                            );
                        }

                        break;
                    }
                }
            }
        }        
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector3.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    private void LoadCheckpoints(GameData _data)
    {
        if(_data.scenes.TryGetValue(SceneManager.GetActiveScene().name, out GameScene value))
        {
            foreach (KeyValuePair<string, bool> pair in value.checkpoints)
            {
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    foreach (KeyValuePair<string, bool> nestedPair in value.checkpointsInitialAnimations)
                    {
                        if ((checkpoint.id == pair.Key) && (pair.Value == true) && (checkpoint.id == nestedPair.Key))
                            checkpoint.ActivateCheckpointOnLoad(nestedPair.Value);
                    }

                }
            }

            foreach (KeyValuePair<string, bool> pair in value.checkpointsInitialAnimations)
            {
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    if (checkpoint.id == pair.Key)
                        checkpoint.SetAnimationStatus(pair.Value);
                }
            }
        }
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);
        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);            
    }

    public void Log(string logText)
    {
        log.text = logText;
    }
}
