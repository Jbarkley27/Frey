using UnityEngine;

public class GlobalDataStore : MonoBehaviour 
{
    public static GlobalDataStore instance { get; private set; }

    [Header("Player")]
    public Transform player;
    public Transform projectileSource;
    public MovementSystem playerMovementSystem;


    public Material hitFlashMaterial_Base;

    

    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Global Data Store object, destroying new one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}