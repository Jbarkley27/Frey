using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class WorldLayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _lineEndObject;
    [SerializeField] private LayerMask _touchLayer;
    [SerializeField] public MovementSystem _movementSystem;
    public static WorldLayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Line Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        TrackWorldCursor();
        ActivateCursor();
    }







    public void TrackWorldCursor()
    {
        // raycast to get the position of the line end
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _touchLayer))
        {
            _lineEndObject.transform.position = hit.point;
        }
    }


    public Vector3 GetDirectionFromWorldCursor(Vector3 source)
    {
        if (_lineEndObject == null)
        {
            Debug.LogError("LineEndObject is null");
            return Vector3.zero;
        }

        return _lineEndObject.transform.position - source;
    }


    public void ActivateCursor()
    {
        if (!TurnBasedBattleManager.instance.IsTimeStopped()) return;
        
        // listen for mouse click
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Clicked");
            TurnBasedBattleManager.instance.ResumeTime();
            _movementSystem.Move(_lineEndObject.transform.position);
        }
    }
}
