using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class WorldCursorManager : MonoBehaviour
{
    [SerializeField] private GameObject _lineEndObject;
    [SerializeField] private LayerMask _touchLayer;
    [SerializeField] public MovementSystem _movementSystem;
    public static WorldCursorManager instance;
    public Chip activeChip;
    public enum CursorState { No_Chip_Loaded, Chip_Loaded };
    public CursorState cursorState = CursorState.No_Chip_Loaded;

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


    public Transform GetWorldCursor()
    {
        return _lineEndObject.transform;
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
                activeChip.ActivateChip();
                activeChip = null; // Could become an issue
                PlayerChipManager.instance.ClearAllChipStates();
            }
        }
    }


    public void LoadChip(Chip chip)
    {
        if (activeChip != null)
        {
            activeChip.SetSelectionState(Chip.SelectionState.None);
        }

        activeChip = chip;
        cursorState = CursorState.Chip_Loaded;
    }


    public bool IsActiveChip(Chip chip)
    {
        return activeChip == chip;
    }
}
