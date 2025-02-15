using UnityEngine;

public class ArcProjectileSystem : MonoBehaviour 
{
    public GameObject projectilePrefab; // Prefab to instantiate
    public int projectileCount = 10; // Number of projectiles to spawn
    public float spreadAngle = 360f; // Total angle of the spread (360 for a full circle)
    public float range = 10; // Range of the projectile
    public float force = 10f; // Force to apply to the projectile
    public static ArcProjectileSystem instance;
    public float projectileMaxRange = 100f;


    public struct ProjectileData
    {
        public GameObject projectilePrefab;
        public int projectileCount;
        public float spreadAngle;
        public float range;
        public float force;
        public int damage;
        public bool isPlayerProjectile;
        public Transform source;
        public Transform target;

        public ProjectileData(GameObject projectilePrefab, int projectileCount, float spreadAngle, float force, float range, int damage, bool isPlayerProjectile, Transform source, Transform target)
        {
            this.projectilePrefab = projectilePrefab;
            this.projectileCount = projectileCount;
            this.spreadAngle = spreadAngle;
            this.range = range;
            this.force = force;
            this.damage = damage;
            this.isPlayerProjectile = isPlayerProjectile;
            this.source = source;
            this.target = target;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Skill Manager object, destroying new one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void SpawnProjectiles(ProjectileData data)
    {
        if (data.projectilePrefab == null)
        {
            Debug.LogWarning("Projectile Prefab is not assigned!");
            return;
        }
        
        float angleStep = data.spreadAngle / Mathf.Max(data.projectileCount - 1, 1);
        float startingAngle = -data.spreadAngle / 2; // Center the arc

        for (int i = 0; i < data.projectileCount; i++)
        {
            // Calculate the rotation for each projectile
            float currentAngle = startingAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0) * data.source.rotation;

            // Spawn the projectile
            GameObject projectile = Instantiate(data.projectilePrefab, data.source.position, rotation);

            // Ensure the projectile faces outward
            projectile.transform.forward = rotation * Vector3.forward;

            // Use the rigidbody to apply force to the projectile to go straight
            PlayerProjectile newProjectile = projectile.GetComponent<PlayerProjectile>();
            newProjectile.SetupProjectile(data.target.position - data.source.position, data.force, data.damage, data.range, data.isPlayerProjectile);
        }
    }
}