using UnityEngine;

public class SaveLoadAction : Actions
{
    public bool isSaving;
    public override void Act()
    {
        if (isSaving)
            SaveManager.instance.SaveGame();
        else
            SaveManager.instance.LoadGame();

    }
}
