using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridInformation : MonoBehaviour
{

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

    public bool IsObstacle(Vector2Int position)
    {
        bool isObstacle = false;
        Vector3Int vector3Pos = new Vector3Int(position.x, position.y, 0);
        foreach(Tilemap tilemap in ObstacleMaps)
        {
            if (tilemap.GetTile(vector3Pos) != null)
            {
                isObstacle = true;
            }
        }
        return isObstacle;
    }

    public bool IsGround(Vector2Int position)
    {
        Vector3Int vector3Pos = new Vector3Int(position.x, position.y, 0);
        bool isGround = false;
        foreach(Tilemap tilemap in GroundMaps)
        {
            if(tilemap.GetTile(vector3Pos) != null)
            {
                isGround = true;
            }
        }
        return isGround;
    }
}
