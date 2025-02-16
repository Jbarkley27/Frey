using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string chipName;
    public string chipDescription;
    public float cursorRange;
    public float chipAimSize;
    public Sprite chipAimSprite;
    public CanvasGroup selectedBorderCG;
    public enum SelectionState { None, Selected, Hovered };

    private void Start() {
        SetSelectionState(SelectionState.None);
    }

    public virtual void ActivateChip(Vector3 position) {
        Debug.Log("Activating Chip");
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(WorldCursorManager.instance.IsActiveChip(this)) {
            return;
        }

        SetSelectionState(SelectionState.Hovered);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(WorldCursorManager.instance.IsActiveChip(this)) {
            return;
        }

        SetSelectionState(SelectionState.None);
    }

    public void OnPointerClick(PointerEventData eventData) {
        SetSelectionState(SelectionState.Selected);
        WorldCursorManager.instance.LoadChip(this);
    }

    public void SetSelectionState(SelectionState state) {
        switch (state) {
            case SelectionState.None:
                selectedBorderCG.alpha = 0;
                break;
            case SelectionState.Selected:
                selectedBorderCG.alpha = 1;
                break;
            case SelectionState.Hovered:
                selectedBorderCG.alpha = 0.5f;
                break;
        }
    }
}
