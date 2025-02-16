using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyID: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public int _turnsAlive = 0;

    [Header("Modules")]
    public EnemyIntentionModule intentionModule;
    public EnemyMovementModule movementModule;
    public EnemyHealthModule healthModule;

    private void Start()
    {
        EnemyManager.instance.AddEnemy(this);
    }

    public void IncrementTurnsAlive()
    {
        _turnsAlive++;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        healthModule.ShowUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        healthModule.HideUI();
    }
}