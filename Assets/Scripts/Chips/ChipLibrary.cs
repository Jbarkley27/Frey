using UnityEngine;

public class ChipLibrary: MonoBehaviour
{
    public static ChipLibrary instance { get; private set; }
    public enum ChipID { BOOST, BLAST };

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Chip Library object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}