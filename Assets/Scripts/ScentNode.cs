using UnityEngine;

public class ScentNode : MonoBehaviour 
{
    [SerializeField] private bool _enemyHasScent = false;

    void Start()
    {
        _enemyHasScent = false;
    }

    public bool EnemyHasScent
    {
        get { return _enemyHasScent; }
        set { _enemyHasScent = value; }
    }
}