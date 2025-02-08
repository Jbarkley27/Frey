using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerProjectile : MonoBehaviour 
{
    public int damage;
    public float range;
    public Vector3 direction;
    public float force;
    public bool isSetup = false;
    public bool isPlayerProjectile = true;

    public void SetupProjectile(Vector3 direction, float force, int damage, float range)
    {
        this.damage = damage;
        this.range = range;
        this.direction = direction;
        this.force = force;

        // gameObject.transform.rotation = GlobalDataStore.instance.player.transform.rotation;

        gameObject.GetComponent<Rigidbody>().linearVelocity = gameObject.transform.forward * force;

        isSetup = true;
    }

    private void Update() {
        if (gameObject == null || !isSetup) return;

        ShouldDestroyItself();
        
        if (TurnBasedBattleManager.instance.IsTimeStopped())
        {
            gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        } else
        {
            gameObject.GetComponent<Rigidbody>().linearVelocity = gameObject.transform.forward * force;
        }
    }

    public void ShouldDestroyItself()
    {

        if (Mathf.Abs(transform.position.x) > ArcProjectileSystem.instance.projectileMaxRange 
            || 
            Mathf.Abs(transform.position.z) > ArcProjectileSystem.instance.projectileMaxRange)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (isPlayerProjectile)
        {
            if (collider.CompareTag("enemy-hitbox"))
            {
                Debug.Log("Hit enemy");
            } 
        }
        else
        {
            if (collider.CompareTag("player-hitbox"))
            {
                Debug.Log("Hit player");
            }
        }
    }


}