using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public interface IMovementController
    {
        Queue<Vector2Int> FindClearPath(Vector2Int start, Vector2Int end);
        Queue<Vector2Int> FindPathThroughEntities(Vector2Int start, Vector2Int end);
        Queue<Vector2Int> GetPath(Vector2Int targetPos, bool isSearchStrict = true);
        bool TryInstantMoveTo(Direction direction);
        bool TryInstantMoveTo(Vector2Int targetPos);
    }
}