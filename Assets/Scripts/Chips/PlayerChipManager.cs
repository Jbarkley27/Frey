using System.Collections.Generic;
using UnityEngine;

public class PlayerChipManager : MonoBehaviour 
{
    public static PlayerChipManager instance { get; private set; }

    public List<Chip> Hand = new List<Chip>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Chip Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ClearAllChipStates()
    {
        foreach (Chip chip in Hand)
        {
            if (chip != null)
                chip.SetSelectionState(Chip.SelectionState.None);
        }
    }
}