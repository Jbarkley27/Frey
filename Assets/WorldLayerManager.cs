using UnityEngine;

public class WorldLayerManager : MonoBehaviour
{
    public GameObject lineEndObject;
    public LayerMask touchLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrackWorldCursor();
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

}
