using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
public class GroundTilemap : MonoBehaviour
{
    public Tilemap ground;

    private void Start()
    {
        ground = GetComponent<Tilemap>();
    }

    public bool IsGround(Vector2Int pos)
    {
        Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
        var tileAtPos = ground.GetTile(tilePos);
        return tileAtPos != null;
    }
}
