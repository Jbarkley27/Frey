using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager instance;
    public List<EnemyID> enemies = new List<EnemyID>();

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found an Enemy Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }






    public void ActivateAllEnemyIntentions() {
        enemies.ForEach(enemy => enemy.intentionModule.ActOnIntention());
    }

    public void StopEnemies() {
        Debug.Log("Stopping Enemy Movement");
        enemies.ForEach(enemy => enemy.movementModule.StopMovement());
    }

    public void CalculateAllEnemyIntentions() {
        StopEnemies();

        Debug.Log("Calculating Enemy Intentions");
        enemies.ForEach(enemy => enemy.intentionModule.CalculateNextIntention());
    }

    public void AddEnemy(EnemyID enemy) {
        enemies.Add(enemy);
    }
}