using UnityEngine;

public class EnemyID: MonoBehaviour
{
    public int id;
    public int _turnsAlive = 0;

    [Header("Modules")]
    public EnemyIntentionModule intentionModule;
    public EnemyMovementModule movementModule;

    private void Start()
    {
        EnemyManager.instance.AddEnemy(this);
    }

    public void IncrementTurnsAlive()
    {
        _turnsAlive++;
    }
}