using UnityEngine;

public class MoveChip : Chip
{
    public override void ActivateChip()
    {
        Debug.Log("Moving Chip Activated");
        GlobalDataStore.instance.playerMovementSystem.Move(WorldCursorManager.instance.GetWorldCursor().position);
    }
}