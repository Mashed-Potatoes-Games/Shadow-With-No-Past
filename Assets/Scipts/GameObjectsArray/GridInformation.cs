using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridInformation : MonoBehaviour
{

    public LayerMask Ground;
    public LayerMask Obstacles;

    public List<Tilemap> GroundMaps;
    public List<Tilemap> ObstacleMaps;

    // Start is called before the first frame update
    void Start()
    {
        GetTilemaps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetTilemaps() 
    {
        List<GameObject> GridChildren = GetGridChildren();
        foreach (GameObject obj in GridChildren)
        {
            Tilemap tilemap = obj.GetComponent<Tilemap>();
            if (tilemap != null )
            {
                if (obj.tag == "GroundTilemap")
                {
                    GroundMaps.Add(tilemap);
                } 
                else if (obj.tag == "ObstacleTilemap")
                {
                    ObstacleMaps.Add(tilemap);
                }
            }
        }
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

    public bool IsObstacle(Vector3Int position)
    {
        bool isObstacle = false;
        foreach(Tilemap tilemap in ObstacleMaps)
        {
            if (tilemap.GetTile(position) != null)
            {
                isObstacle = true;
            }
        }
        return isObstacle;
    }

    public bool IsGround(Vector3Int position)
    {
        bool isGround = false;
        foreach(Tilemap tilemap in GroundMaps)
        {
            if(tilemap.GetTile(position) != null)
            {
                isGround = true;
            }
        }
        return isGround;
    }
}
