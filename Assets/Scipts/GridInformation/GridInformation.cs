using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Entity;

public class GridInformation : MonoBehaviour
{
    public enum CellStatus
    {
        NoGround,
        Obstacle,
        Free,
        Occupied
    }

    public Grid ThisGrid;

    public List<Tilemap> GroundMaps;
    public List<Tilemap> ObstacleMaps;
    public EntitiesGrid EntitiesGrid;

    // Initialize information
    void Start()
    {
        ThisGrid = gameObject.GetComponent<Grid>();

        GetMaps();
    }

    // Update is called once per frame
    // We don't need those
    void Update()
    {

    }

    void GetMaps()
    {
        //Tilemaps or other objects need to be the childer in order to find and work with those.
        //This is because every world has it's own tilemap and it allows to differentiate between them.
        List<GameObject> GridChildren = GetGridChildren();

        foreach (GameObject obj in GridChildren)
        {
            Tilemap tilemap = obj.GetComponent<Tilemap>();
            EntitiesGrid entGrid = obj.GetComponent<EntitiesGrid>();
            if (tilemap != null)
            {
                if (obj.CompareTag("GroundTilemap"))
                {
                    GroundMaps.Add(tilemap);
                }
                else if (obj.CompareTag("ObstacleTilemap"))
                {
                    ObstacleMaps.Add(tilemap);
                }
            }
            else if (entGrid != null)
            {
                EntitiesGrid = entGrid;
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

    public bool IsObstacle(Vector2Int pos)
    {
        bool isObstacle = false;
        //Cycles through obstacle tilemaps to find at least one in given position
        Vector3Int vector3Pos = new Vector3Int(pos.x, pos.y, 0);
        foreach (Tilemap tilemap in ObstacleMaps)
        {
            if (tilemap.GetTile(vector3Pos) != null)
            {
                isObstacle = true;
            }
        }
        return isObstacle;
    }

    public bool IsGround(Vector2Int pos)
    {
        //Cycles through ground tilemaps to find at least one in given position
        Vector3Int vector3Pos = new Vector3Int(pos.x, pos.y, 0);
        bool isGround = false;
        foreach (Tilemap tilemap in GroundMaps)
        {
            if (tilemap.GetTile(vector3Pos) != null)
            {
                isGround = true;
            }
        }
        return isGround;
    }

    //This function is needed mostly so the Entities could call this one instead of GridInformation.EntitiesGrid.IsOccupied();
    //This way I'm hiding Entities grid from it.
    public bool IsOccupied(Vector2Int pos)
    {
        return EntitiesGrid.IsOccupied(pos);
    }

    //Returns, what is at the given coordinate.
    public CellStatus GetCellStatus(Vector2Int pos)
    {
        if(!IsGround(pos))
        {
            return CellStatus.NoGround;
        } else if (IsObstacle(pos))
        {
            return CellStatus.Obstacle;
        } else if(IsOccupied(pos))
        {
            return CellStatus.Occupied;
        }
        return CellStatus.Free;
    }
}
