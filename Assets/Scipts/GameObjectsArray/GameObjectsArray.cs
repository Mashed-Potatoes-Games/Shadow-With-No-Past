using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameObjectsArray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeGridArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGridArray() 
    {
        List<GameObject> GridChildren = GetGridChildren();
        foreach (GameObject obj in GridChildren)
            Debug.Log(obj);
        GetGridColliders(GridChildren);
    }

    List<GameObject> GetGridChildren()
    {
        List<GameObject> GridChildren = new List<GameObject>();

        foreach (Transform child in transform)
        {
            GridChildren.Add(child.gameObject);
        }

        return GridChildren;
    }

    void GetGridColliders(List<GameObject> GridChildren)
    {
        foreach(GameObject child in GridChildren)
        {
            Tilemap TilemapComponent = child.GetComponent<Tilemap>();
            if(TilemapComponent != null)
            {
                Debug.Log("This is a tilemap with tag: " + child.tag);
            }
        }
    }
}
