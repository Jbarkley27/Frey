using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerScentNodes : MonoBehaviour
{
    public List<GameObject> _scentNodes = new List<GameObject>();
    public static PlayerScentNodes instance;
    public Transform _player;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Enemy Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            _scentNodes.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position;
    }


    public Transform GetRandomScentNode()
    {
        return _scentNodes[Random.Range(0, _scentNodes.Count)].transform;
    }
}
