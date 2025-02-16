using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



/**
 * This class is responsible for determining the next move that the enemy will make and updating the UI accordingly.
 */
public class EnemyIntentionModule : MonoBehaviour
{
    public Image nextMoveIcon;
    public Sprite moveIcon;
    public Sprite shootIcon;
    public Sprite canSeePlayerIcon;
    public enum Intention { MOVE, ATTACK, SEEK };
    public Intention nextIntention;
    public GameObject projectilePrefab;
    public int projectileCount;
    public float spreadAngle;
    public int range;
    public float force;
    public int damage;
    public Transform source;
    public Transform target;
    public bool isPlayerVisible = false;
    public EnemyMovementModule enemyMovementModule;



    private void Start()
    {
        enemyMovementModule = GetComponent<EnemyMovementModule>();
    }



    private void Update() 
    {
        // Draw a ray so we can see it in the game view
        Debug.DrawRay(transform.position, (GlobalDataStore.instance.player.position - transform.position), Color.white);


        RaycastHit hit;
        if (Physics.Raycast(transform.position, (GlobalDataStore.instance.player.position - transform.position), out hit, 1000f))
        {
            if (hit.collider.gameObject.CompareTag("player-hitbox"))
            {
               isPlayerVisible = true;
            } 
            else 
            {
                isPlayerVisible = false;
            }
        } 
        else 
        {
            isPlayerVisible = false;
        }
    }







    public void ActOnIntention()
    {
        Debug.Log("Acting on intention");

        enemyMovementModule.StopCalculatingPath();

        if (nextIntention == Intention.MOVE || nextIntention == Intention.SEEK)
        {
            enemyMovementModule.ResumeMovement();
        }
        else if (nextIntention == Intention.ATTACK)
        {
            Debug.Log("ATTACKING");
            ArcProjectileSystem.instance.SpawnProjectiles(
                new ArcProjectileSystem.ProjectileData(
                    projectilePrefab,
                    projectileCount,
                    spreadAngle,
                    force,
                    range,
                    damage,
                    false,
                    source,
                    target
                )
            );
        }
    }









    public void CalculateNextIntention()
    {
        Debug.Log("Calculating next intention");
        if (enemyMovementModule.IsWithinRange())
        {
            if (isPlayerVisible)
            {
                enemyMovementModule.HideLineRenderer();
                nextMoveIcon.sprite = shootIcon;
                nextMoveIcon.transform.DOPunchScale(new Vector3(.8f, 0.2f, 0.1f), 0.5f);
                nextIntention = Intention.ATTACK;
                return;
            } else {
                nextMoveIcon.sprite = canSeePlayerIcon;
                nextMoveIcon.transform.DOPunchScale(new Vector3(.8f, 0.2f, 0.1f), 0.5f);
                nextIntention = Intention.SEEK;
                enemyMovementModule.CalculatePathToPlayer(Intention.SEEK);
                return;
            }
        } else {
            nextMoveIcon.sprite = moveIcon;
            nextMoveIcon.transform.DOPunchScale(new Vector3(.8f, 0.2f, 0.1f), 0.5f);
            nextIntention = Intention.MOVE;
            enemyMovementModule.CalculatePathToPlayer();
        }
    }

}