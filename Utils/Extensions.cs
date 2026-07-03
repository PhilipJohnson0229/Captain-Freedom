using UnityEngine;
using UnityEngine.EventSystems;
public static class Extensions
{

    public static void RunActions(Actions[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act();
        }
    }
}
