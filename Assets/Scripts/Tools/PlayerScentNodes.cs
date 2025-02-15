using System.Collections.Generic;
using UnityEngine;


public class PlayerScentNodes : MonoBehaviour
{
    public List<ScentNode> _scentNodes = new List<ScentNode>();
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
        // foreach (Transform child in transform)
        // {
        //     _scentNodes.Add(child.gameObject.GetComponent<ScentNode>());
        //     child.gameObject.GetComponent<ScentNode>().EnemyHasScent = false;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position;
    }


    public ScentNode GetRandomScentNode()
    {
        ScentNode newNode = _scentNodes[Random.Range(0, _scentNodes.Count)];

        foreach (ScentNode node in _scentNodes)
        {
            if (!node.EnemyHasScent)
            {
                node.EnemyHasScent = true;
                return node;
            }
        }

        newNode.EnemyHasScent = true;
        return newNode;
    }
}
