using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager 
{
    void LoadData(GameData _data);
    void SaveData(GameData _data);
}
