using UnityEngine;

public class Bed : TileObjectInstance
{
    public override void OnSecondaryInteract(IItemReceiver receiver)
    {
        TimeManager.Instance.NewDay();
    }
}
