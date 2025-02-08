using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



/**
 * This class is responsible for determining the next move that the enemy will make and updating the UI accordingly.
 */
public class EnemyIntentionModule : MonoBehaviour
{
    public Image nextMoveIcon;
    public CanvasGroup nextMoveCanvasGroup;
    public Sprite moveIcon;
    public Sprite shootIcon;
    public enum Intention { MOVE, SHOOT };
    public Intention nextIntention;

    public GameObject projectilePrefab;
    public int projectileCount;
    public float spreadAngle;
    public int range;
    public float force;
    public int damage;
    public bool isPlayerProjectile;
    public Transform source;
    public Transform target;



    private void Start()
    {
        
    }



    private void Update() 
    {
        
    }


    public void ActOnIntention()
    {
        Debug.Log("Acting on intention");

        GetComponent<EnemyMovementModule>().StopCalculatingPath();

        if (nextIntention == Intention.MOVE)
        {
            GetComponent<EnemyMovementModule>().ResumeMovement();
        } else if (nextIntention == Intention.SHOOT)
        {
            Debug.Log("Shooting");
            ArcProjectileSystem.instance.SpawnProjectiles(
                new ArcProjectileSystem.ProjectileData(
                    projectilePrefab,
                    projectileCount,
                    spreadAngle,
                    force,
                    range,
                    damage,
                    isPlayerProjectile,
                    source,
                    target
                )
            );
        }
        

    }



    public void CalculateNextIntention()
    {
        Debug.Log("Calculating next intention");
        

        if (GetComponent<EnemyMovementModule>().IsWithinRange())
        {
            Debug.Log("Player is within range, I Should Shoot");
            nextMoveIcon.sprite = shootIcon;
            nextMoveIcon.transform.DOPunchScale(new Vector3(.8f, 0.2f, 0.1f), 0.5f);
            nextIntention = Intention.SHOOT;
        } else {
            Debug.Log("Player is not within range, I Should Move");
            nextMoveIcon.sprite = moveIcon;
            nextMoveIcon.transform.DOPunchScale(new Vector3(.8f, 0.2f, 0.1f), 0.5f);
            nextIntention = Intention.MOVE;
            GetComponent<EnemyMovementModule>().CalculatePathToPlayer();
        }
    }

}