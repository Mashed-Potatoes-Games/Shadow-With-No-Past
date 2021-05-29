using ShadowWithNoPast.Entities;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
public class ObstacleTilemap : MonoBehaviour
{
    public Tilemap Obstacles;

    private TilemapCollider2D tilemapCollider;
    private World world;

    void Start()
    {
        Obstacles = GetComponent<Tilemap>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        world = GetComponentInParent<World>();
    }
    public bool IsObstacle(Vector2Int pos)
    {
        Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
        var tileAtPos = Obstacles.GetTile(tilePos);
        return tileAtPos != null;
    }

    // Fuction, that was created to test events and opacity in the tilemap.
    // TODO: Either delete it completely or write it better.
    // world.EventManager.EntityMoved.AddListener(ShowEntititesBehindObjects);
    private void ShowEntititesBehindObjects(GridEntity entity)
    {
        Vector2 cellCenter = entity.Vector + new Vector2(World.TileOffset, World.TileOffset);
        if(tilemapCollider.OverlapPoint(cellCenter))
        {
            var tilePos = Obstacles.WorldToCell(cellCenter + new Vector2Int(0, -1));
            var tile = Obstacles.GetTile(tilePos);
            Obstacles.SetTileFlags(tilePos, TileFlags.None);
            Obstacles.SetColor(tilePos, new Color(0.5f, 0.5f, 0.5f, 0.5f));
            Obstacles.RefreshTile(tilePos);
            var flags = Obstacles.GetTileFlags(tilePos);
            var color = Obstacles.GetColor(tilePos);
        }
    }

}
