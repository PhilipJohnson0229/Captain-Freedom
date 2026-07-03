using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using BNG;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public BNGPlayerController player;
    public string currentScene;
    public bool canMove = true;
    public bool freeLook = true;
    public bool isInteracting = false;

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
        //for testing
       
    }
   

    public void LoadData(GameData _data)
    {
       
    }

    public void SaveData(GameData _data)
    {
        
    }

    public void HaltPlayerMovement()
    {
        canMove = false;
        //player.StateMachine.ChangeState(player.PlayerIdleState);
    }

    public void ResumePlayerMovement() => canMove = true;
}
