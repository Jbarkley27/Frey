using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class WorldLayerManager : MonoBehaviour
{
    public GameObject lineEndObject;
    public LayerMask touchLayer;
    public GameObject playerMoveStopNode;
    public MovementSystem movementSystem;
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

    void Start()
    {
        
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayer))
        {
            lineEndObject.transform.position = hit.point;
        }
    }


    public Vector3 GetDirectionFromWorldCursor(Vector3 source)
    {
        if (lineEndObject == null)
        {
            Debug.LogError("LineEndObject is null");
            return Vector3.zero;
        }

        return lineEndObject.transform.position - source;
    }


    public void ActivateCursor()
    {
        // listen for mouse click
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Clicked");
            Instantiate(playerMoveStopNode, lineEndObject.transform.position, Quaternion.identity);
            movementSystem.Move(lineEndObject.transform.position);
        }
    }
}
