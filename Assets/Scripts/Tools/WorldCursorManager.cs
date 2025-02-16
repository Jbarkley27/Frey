using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Image = UnityEngine.UI.Image;


public class WorldCursorManager : MonoBehaviour
{
    [SerializeField] private GameObject _lineEndObject;
    [SerializeField] private LayerMask _touchLayer;
    [SerializeField] public LineRenderer _lineRenderer;
    public static WorldCursorManager instance;
    public Chip activeChip;
    public enum CursorState { No_Chip_Loaded, Chip_Loaded };
    public CursorState cursorState = CursorState.No_Chip_Loaded;
    public GameObject cursorAimRoot;
    public Image cursorAimImage;

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
        DrawLine();

        if (activeChip == null)
        {
            _lineRenderer.enabled = false;
            cursorAimRoot.SetActive(false);
        }
        else
        {
            _lineRenderer.enabled = true;
            cursorAimRoot.SetActive(true);
        }
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


    public Transform GetWorldCursor()
    {
        return _lineEndObject.transform;
    }

    public float distanceExtra;
    public Vector3 finalWorldCursorPosition;
    public void DrawLine()
    {
        if (_lineRenderer == null)
        {
            Debug.LogError("LineRenderer is null");
            return;
        }


        if (activeChip == null)
        {
            _lineRenderer.enabled = false;
            return;
        }

        // get the position of the line end
        Vector3 lineStart = GlobalDataStore.instance.player.transform.position;
        finalWorldCursorPosition = GetWorldCursor().position;

        // clamp the distance
        if (Vector3.Distance(lineStart, finalWorldCursorPosition) > activeChip.cursorRange)
        {
            Vector3 direction = finalWorldCursorPosition - lineStart;
            direction.Normalize();
            finalWorldCursorPosition = lineStart + direction * (activeChip.cursorRange);
        }

        // make the cursor aim image follow the cursor up to the active chip's range
        // we need the aim image to to be a certain distance further than the line end
        Vector3 aimImagePosition = finalWorldCursorPosition + (finalWorldCursorPosition - lineStart).normalized * distanceExtra;
        cursorAimRoot.transform.position = aimImagePosition;
        cursorAimImage.transform.localScale = new Vector3(activeChip.chipAimSize, activeChip.chipAimSize, 1);

        

        SetLineRendererSettings(lineStart, finalWorldCursorPosition, 2);
    }


    public void SetLineRendererSettings(Vector3 start, Vector3 end, int count)
    {
        if (_lineRenderer == null) return;

         // set the line renderer
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = count;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }


    public void ActivateCursor()
    {
        if (!TurnBasedBattleManager.instance.IsTimeStopped()) return;

        if (UITest.instance.isPointerOverUIElement) return;
        
        // listen for mouse click
        if (Input.GetMouseButtonUp(0))
        {
            if (CursorState.No_Chip_Loaded == cursorState)
            {
                // We need to load the chip
                return;
            }

            if (CursorState.Chip_Loaded == cursorState)
            {
                // We need to activate the chip
                if (activeChip == null)
                {
                    Debug.LogError("Active Chip is null");
                    return;
                }

                Debug.Log("Mouse Clicked");
                TurnBasedBattleManager.instance.ResumeTime();
                activeChip.ActivateChip(finalWorldCursorPosition);
                activeChip = null; // Could become an issue
                PlayerChipManager.instance.ClearAllChipStates();
                cursorState = CursorState.No_Chip_Loaded;
            }
        }
    }








    // CHIP LOADING


    public void LoadChip(Chip chip)
    {
        if (activeChip != null)
        {
            activeChip.SetSelectionState(Chip.SelectionState.None);
        }

        activeChip = chip;
        cursorState = CursorState.Chip_Loaded;
        cursorAimImage.sprite = chip.chipAimSprite;

    }


    public bool IsActiveChip(Chip chip)
    {
        return activeChip == chip;
    }
}
