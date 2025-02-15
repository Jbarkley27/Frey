using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class EnemyMovementModule : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] public Transform _target;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _finalLineRenderer;
    [SerializeField] private Transform _enemyVisual;
    public NavMeshObstacle _navMeshObstacle;
    Coroutine _pathCoroutine;
    public bool _pathing = false;
    public Rigidbody _rb;
    public float _rotateSpeed = 5.0f;


    [Header("NavMesh Agent Settings")]
    [SerializeField] private float _speedMin = 1.0f;
    [SerializeField] private float _speedMax = 2.0f;
    [SerializeField] private float _accelerationMin = 1.0f;
    [SerializeField] private float _accelerationMax = 2.0f;
    // [SerializeField] private float _angularSpeedMin = 120.0f;
    // [SerializeField] private float _angularSpeedMax = 240.0f;
    [SerializeField] private float _stoppingDistanceMin = 1.0f;
    [SerializeField] private float _stoppingDistanceMax = 2.0f;
    [SerializeField] private float _currentStoppingDistance = 0.5f;
    [SerializeField] private float _finalStoppingDistance = 0.5f;
    [SerializeField] private float _radiusMin = 0.5f;
    [SerializeField] private float _radiusMax = 1.0f;


    [Header("Path Limiting Settings")]
    [SerializeField] private List<Vector3> _newPoints = new List<Vector3>();
    [SerializeField] private float _spacing = 1f;
    [Range(20f, 100f)] public float _percentageOfPath = 50f;
    [Range(20f, 100f)] public float _maxPercentageOfPath = 50f;
    List<Vector3> _tempPoints = new List<Vector3>();
    [SerializeField] private int _minPoints = 2;

    private void Start()
    {
        _navMeshAgent.autoRepath = false;
        RandomizeAgentValues();
        _target = PlayerScentNodes.instance.GetRandomScentNode().transform;
        _rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        // ensure enemy y position is always 0
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        _navMeshAgent.stoppingDistance = _currentStoppingDistance;

        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;

        RotateTowards(GlobalDataStore.instance.player.transform.position - transform.position);
    }





    // MOVEMENT FUNCTIONS -----------------------------------------

    public void StopCalculatingPath()
    {
        if (_pathCoroutine != null) StopCoroutine(_pathCoroutine);
    }


    



    public IEnumerator CalculatePathHelper(Vector3 targetPos, bool isFinalPath = false)
    {
        StopMovement();
        _navMeshAgent.SetDestination(targetPos);
        StopMovement();

        // Keep trying until the agent successfully calculates a path
        while (!_navMeshAgent.hasPath)
        {
            Debug.Log("Path not found, retrying...");
            yield return null; // Wait one frame
            _navMeshAgent.SetDestination(targetPos); // Retry setting destination
            StopMovement();
        }


        // Draw the path that the enemy will take
        Debug.Log("Path found!");

        if (isFinalPath) 
        {
            // This is when we have already limited the path
            Debug.Log("Final Path Calculated -- Not limiting path");

            _finalLineRenderer.positionCount = _navMeshAgent.path.corners.Length;
            _finalLineRenderer.SetPosition(0, _enemyVisual.position);
            _finalLineRenderer.SetPositions(_navMeshAgent.path.corners);
            // EXIT
            _pathing = false;
            yield break;
        }
        else
        {
            // This is when we are calculating/limiting the path for the first time
            _lineRenderer.positionCount = _navMeshAgent.path.corners.Length;
            _lineRenderer.SetPosition(0, _enemyVisual.position);
            _lineRenderer.SetPositions(_navMeshAgent.path.corners);
        }

        Debug.Log("Starting to limit path");
        LimitPath();
    }






    public void ResumeMovement()
    {
        transform.DOPath(_navMeshAgent.path.corners, Random.Range(.9f, 1.2f), PathType.CatmullRom, PathMode.Ignore)
            .SetEase(Ease.InOutQuad);
    }

    public void StopMovement()
    {
        // Debug.Log("Stopping Movement");
        // _navMeshAgent.isStopped = true;
        // _navMeshAgent.velocity = Vector3.zero;
        // _currentStoppingDistance = _finalStoppingDistance;
    }




    public void HideLineRenderer()
    {
        _lineRenderer.positionCount = 0;
        _finalLineRenderer.positionCount = 0;
    }

    public void ShowLineRenderer()
    {
        _lineRenderer.gameObject.SetActive(true);
    }








    public void CalculatePathToPlayer(EnemyIntentionModule.Intention intention = EnemyIntentionModule.Intention.MOVE)
    {
        _pathing = true;
        Debug.Log("Beginning path calculation");
        _navMeshAgent.ResetPath();
        StartCoroutine(CalculatePathHelper(GetMovePosition(intention)));
    }


    public Vector3 GetMovePosition(EnemyIntentionModule.Intention intention)
    {
        // if (intention == EnemyIntentionModule.Intention.MOVE)
        // {
        //     return _target.position;
        // }
        // else if (intention == EnemyIntentionModule.Intention.SEEK)
        // {
        //     Vector2 rndPoint = Random.insideUnitSphere * 5;
        //     return new Vector3(rndPoint.x, 0, rndPoint.y) + transform.position;
        // }

        return _target.position;
    }



    // This is called after calculating the path to the player
    public void LimitPath()
    {
        if (_lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned.");
            return;
        }

        Debug.Log("Limiting Path");


        _newPoints.Clear();
        _tempPoints.Clear();

        // // Get positions from LineRenderer
        int pointCount = _lineRenderer.positionCount;

        Vector3[] originalPoints = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(originalPoints);


        // // Calculate total length of the line
        float totalLength = 0f;
        for (int i = 0; i < pointCount - 1; i++)
        {
            totalLength += Vector3.Distance(originalPoints[i], originalPoints[i + 1]);
        }


        // // Determine number of spheres to spawn
        int sphereCount = Mathf.FloorToInt(totalLength / _spacing);

        // // Spawn spheres along the line
        float distanceCovered = 0f;
        int segmentIndex = 0;




        for (int i = 0; i < sphereCount; i++)
        {
            float targetDistance = i * _spacing;

            // Move along the line segments to find the correct position
            while (segmentIndex < pointCount - 1 && distanceCovered + Vector3.Distance(originalPoints[segmentIndex], originalPoints[segmentIndex + 1]) < targetDistance)
            {
                distanceCovered += Vector3.Distance(originalPoints[segmentIndex], originalPoints[segmentIndex + 1]);
                segmentIndex++;
            }

            // Interpolate position within the current segment
            float remainingDistance = targetDistance - distanceCovered;
            float segmentLength = Vector3.Distance(originalPoints[segmentIndex], originalPoints[segmentIndex + 1]);
            float t = remainingDistance / segmentLength;

            Vector3 spawnPosition = Vector3.Lerp(originalPoints[segmentIndex], originalPoints[segmentIndex + 1], t);

            // Add the position to the list
            _newPoints.Add(spawnPosition);
        }



        // If we only have a few points, just use the original points, no need to shorten the path
        if (_newPoints.Count < _minPoints)
        {
            Debug.Log("Not enough points to shorten path, continue to use the original target/path");
            _navMeshAgent.ResetPath();
            StartCoroutine(CalculatePathHelper(_target.position, true));
            return;
        }




        // Limit the path by removing a percentage of teh newPoints array
        // calculate the percentage of the path
        int finalPathPercentage = (int)(_newPoints.Count * (_percentageOfPath / 100.0f));

        finalPathPercentage = Mathf.Clamp(finalPathPercentage, 0, _newPoints.Count);


        for (int i = 0; i < finalPathPercentage; i++) _tempPoints.Add(_newPoints[i]);
        


        _navMeshAgent.ResetPath();
        StartCoroutine(CalculatePathHelper(_tempPoints.Last(), true));
    }




    public void RotateTowards(Vector3 targetDirection)
    {
        // if (TurnBasedBattleManager.instance.IsTimeStopped()) return;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        targetRotation = targetRotation.normalized;

        // only rotate around the y axis
        targetRotation.x = 0;
        targetRotation.z = 0;

        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            _rb.rotation,             // Current rotation
            targetRotation,          // Target rotation
            _rotateSpeed * Time.deltaTime // Interpolation factor
        );

        smoothedRotation = smoothedRotation.normalized;

        // Apply the smooth rotation to the Rigidbody
        _rb.MoveRotation(smoothedRotation);
    }





    // UTIL FUNCTIONS --------------------------------------------

    public bool IsWithinRange()
    {
        return Vector3.Distance(transform.position, _target.position) < _finalStoppingDistance;
    }

    public void RandomizeAgentValues()
    {
        // This function will serve as a way to randomize navmesh agent settings so that each enemy can have a unique speed, acceleration, etc.
        _navMeshAgent.speed = Random.Range(_speedMin, _speedMax);
        _navMeshAgent.acceleration = Random.Range(_accelerationMin, _accelerationMax);
        // _navMeshAgent.angularSpeed = Random.Range(_angularSpeedMin, _angularSpeedMax);
        _navMeshAgent.stoppingDistance = Random.Range(_stoppingDistanceMin, _stoppingDistanceMax);
        _navMeshAgent.radius = Random.Range(_radiusMin, _radiusMax);
        _rotateSpeed = Random.Range(_rotateSpeed - 1f, _rotateSpeed + 1f);
        _percentageOfPath = Random.Range(_percentageOfPath, _maxPercentageOfPath);

        _finalStoppingDistance = _navMeshAgent.stoppingDistance;
    }
}
