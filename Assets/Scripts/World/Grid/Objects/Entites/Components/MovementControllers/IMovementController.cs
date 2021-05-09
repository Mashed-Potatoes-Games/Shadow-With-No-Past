using ShadowWithNoPast.Entities.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public interface IMovementController
    {
        bool CanCellBePassable(WorldPos pos);
        Queue<WorldPos> FindClearPath(WorldPos start, WorldPos end);
        Queue<WorldPos> FindPathThroughEntities(WorldPos start, WorldPos end);
        Queue<WorldPos> GetPath(WorldPos targetPos, bool isSearchStrict = true);
        List<WorldPos> GetAvailableMoves();
        bool IsCellFree(WorldPos pos);
        IEnumerator MoveWithDelay(Queue<WorldPos> path);
        bool TryInstantMoveTo(Direction direction);
        bool TryInstantMoveTo(WorldPos targetPos);
    }
}