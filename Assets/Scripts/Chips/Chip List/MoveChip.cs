using UnityEngine;

public class MoveChip : Chip
{
    public override void ActivateChip(Vector3 position)
    {
        Debug.Log("Moving Chip Activated");
        GlobalDataStore.instance.playerMovementSystem.Move(position);
    }
}