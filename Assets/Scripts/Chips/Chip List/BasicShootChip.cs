using UnityEngine;

public class BasicShootChip : Chip
{
    public GameObject projectilePrefab;
    public int projectileCount;
    public float spreadAngle;
    public int range;
    public float force;
    public int damage;
    public bool isPlayerProjectile;


    public override void ActivateChip()
    {
        Debug.Log("Basic Shoot Chip Activated");
        ArcProjectileSystem.instance.SpawnProjectiles(
                new ArcProjectileSystem.ProjectileData(
                    projectilePrefab,
                    projectileCount,
                    spreadAngle,
                    force,
                    range,
                    damage,
                    isPlayerProjectile,
                    GlobalDataStore.instance.player.transform,
                    WorldCursorManager.instance.GetWorldCursor()
                )
            );
    }
}