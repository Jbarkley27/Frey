using UnityEngine;
using System.Collections;


public class MovementSystem : MonoBehaviour
{

    private Rigidbody rb;
    public float rotateSpeed = 1.0f;
    private float rotateDirection;
    private float rotateDifference;
    public float maxVelocity;
    public float forceMagnitude = 5.55f; // sweet spot
    




    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
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
            rb.rotation,             // Current rotation
            targetRotation,          // Target rotation
            rotateSpeed * Time.deltaTime              // Interpolation factor
        );

        // Apply the smooth rotation to the Rigidbody
        rb.MoveRotation(smoothedRotation);
    }


    // MOVEMENT HANDLING ----------------------------------------------------
    public void Move(Vector3 position)
    {
        Vector3 targetPosition = position;
        Vector3 currentPosition = transform.position;

        // Calculate direction and distance
        Vector3 direction = (targetPosition - currentPosition).normalized;
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // Calculate required force
        Vector3 force = direction * forceMagnitude * distance;

        // Apply force to the Rigidbody
        rb.AddForce(force, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Environment"))
        {
            Debug.Log("Collision with Environment");
        }
    }
}
