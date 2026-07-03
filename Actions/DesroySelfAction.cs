using UnityEngine;

public class DesroySelfAction : Actions
{
    public override void Act()
    {
        Destroy(gameObject);
    }
}
