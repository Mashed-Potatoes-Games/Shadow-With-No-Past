using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
public class ObstacleTilemap : MonoBehaviour
{
    public Tilemap obstacles;

    private void Start()
    {
        obstacles = GetComponent<Tilemap>();
    }
    public bool IsObstacle(Vector2Int pos)
    {
        Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
        var tileAtPos = obstacles.GetTile(tilePos);
        return tileAtPos != null;
    }

}
