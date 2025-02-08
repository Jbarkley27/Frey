using UnityEngine;



public class MovementSystem : MonoBehaviour
{

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _rotateSpeed = 1.0f;
    private float rotateDirection;
    private float rotateDifference;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _forceMagnitude = 5.55f; // sweet spot
    




    private void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
        // ensure player y position is always 0
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        
        RotateTowards(WorldLayerManager.instance.GetDirectionFromWorldCursor(transform.position));
    }



    // ROTATION HANDLING -----------------------------------------------------
    private void RotateTowards(Vector3 targetDirection)
    {
        if (targetDirection == Vector3.zero)
            return;


        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        targetRotation = targetRotation.normalized;

        // only rotate around the y axis
        targetRotation.x = 0;
        targetRotation.z = 0;


        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            _rb.rotation,                              // Current rotation
            targetRotation,                            // Target rotation
            _rotateSpeed * Time.deltaTime              // Interpolation factor
        );

        smoothedRotation = smoothedRotation.normalized;

        // Apply the smooth rotation to the Rigidbody
        _rb.MoveRotation(smoothedRotation);
    }


    // MOVEMENT HANDLING ----------------------------------------------------
    public void Move(Vector3 position)
    {
        if (TurnBasedBattleManager.instance.IsTimeStopped()) return;
        

        Vector3 targetPosition = position;
        Vector3 currentPosition = transform.position;

        // Calculate direction and distance
        Vector3 direction = (targetPosition - currentPosition).normalized;
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // Calculate required force
        Vector3 force = direction * _forceMagnitude * distance;

        // Apply force to the Rigidbody
        _rb.AddForce(force, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Environment"))
        {
            Debug.Log("Collision with Environment");
        }
    }
}
