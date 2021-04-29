using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Algorithms
{
    /// <summary>
    /// Simple BFS that can use delegate, which tells, is cell free.
    /// </summary>
    public static class BreadthFirstSearch
    {
        public delegate bool IsPassable<T>(T pos);

        public static Queue<WorldPos> FindPath(WorldPos start, WorldPos end, IsPassable<WorldPos> isCellFree)
        {
            if(start.World != end.World)
            {
                Debug.LogError("BFSearch works only with 1 world. Add between worlds search implementation!");
                return null;
            }

            if (start.Equals(end))
            {
                var result = new Queue<WorldPos>();
                result.Enqueue(start);
                return result;
            }

            Queue<PathNode> SearchQueue = new Queue<PathNode>();
            SearchQueue.Enqueue(new PathNode(start));

            

            List<WorldPos> Visited = new List<WorldPos>() { start };
            while (SearchQueue.Count > 0)
            {
                PathNode Current = SearchQueue.Dequeue();

                foreach(WorldPos target in Current.GetNeighbours())
                {
                    PathNode NeighbourToCurrent = new PathNode(target, Current);

                    if (target == end)
                    {
                        return PathNodeToQueue(NeighbourToCurrent);
                    }
                    else if (!Visited.Contains(target) && isCellFree(target))
                    {
                        
                        SearchQueue.Enqueue(NeighbourToCurrent);
                        Visited.Add(target);
                    }
                }
            }
            return null;
        }

        public static Queue<WorldPos> PathNodeToQueue(PathNode node)
        {
            Queue<WorldPos> queue = new Queue<WorldPos>();

            var list = PathNodeToList(node);

            foreach(WorldPos pos in list)
            {
                queue.Enqueue(pos);
            }

            return queue;
        }

        private static List<WorldPos> PathNodeToList(PathNode node)
        {
            var list = new List<WorldPos> { node.TargetPos };

            PathNode previous = node.Previous;

            while (previous != null)
            {
                list.Add(previous.TargetPos);
                previous = previous.Previous;
            }

            list.Reverse();
            return list;
        }

        public static List<WorldPos> GetAvailableMoves(WorldPos start, int moveDistance, IsPassable<WorldPos> isPassable)
        {
            var availableMoves = new List<WorldPos>() { start };

            Queue<PathNode> SearchQueue = new Queue<PathNode>();
            SearchQueue.Enqueue(new PathNode(start));

            List<WorldPos> Visited = new List<WorldPos>() { start };
            while (SearchQueue.Count > 0)
            {
                PathNode Current = SearchQueue.Dequeue();

                foreach (WorldPos pos in Current.GetNeighbours())
                {
                    if (!Visited.Contains(pos) && isPassable(pos))
                    {
                        PathNode NeighbourToCurrent = new PathNode(pos, Current);

                        Visited.Add(pos);
                        availableMoves.Add(pos);

                        if (NeighbourToCurrent.PathLength < moveDistance)
                        {
                            SearchQueue.Enqueue(NeighbourToCurrent);
                        }
                    }
                }
            }
            return availableMoves;
        }
    }

    /// <summary>
    /// Simple node implementation for backtracking path.
    /// </summary>
    public class PathNode
    {
        public WorldManagement World;
        public Vector2Int Pos;
        public PathNode Previous;

        public WorldPos TargetPos => new WorldPos(World, Pos);

        public int PathLength { get
            {
                return Previous is null ? 0 : Previous.PathLength + 1;
            } }

        public PathNode(WorldManagement world, Vector2Int pos)
        {
            World = world;
            Pos = pos;
        }

        public PathNode(WorldPos target) : this(target.World, target.Vector) { }

        public PathNode(WorldManagement world, Vector2Int pos, PathNode previous) : this(world, pos)
        {
            Previous = previous;
        }

        public PathNode(WorldPos target, PathNode previous) : this(target.World, target.Vector, previous) { }

        #region All existing Neighbours
        public WorldPos TopPos()
        {
            return new WorldPos(World, Pos + Vector2Int.up);
        }
        public WorldPos RightPos()
        {
            return new WorldPos(World, Pos + Vector2Int.right);
        }
        public WorldPos BottomPos()
        {
            return new WorldPos(World, Pos + Vector2Int.down);
        }
        public WorldPos LeftPos()
        {
            return new WorldPos(World, Pos + Vector2Int.left);
        }
        #endregion

        public IEnumerable<WorldPos> GetNeighbours()
        {
            yield return TopPos();
            yield return RightPos();
            yield return BottomPos();
            yield return LeftPos();
        }

        public override string ToString()
        {
            return Pos.ToString();
        }

        public string ToStringManual()
        {
            if(Previous != null)
            {
                return Previous.ToString() + " " + Pos.ToString();
            }
            else
            {
                return Pos.ToString();
            }
        }

    }

}
