using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Algorithms
{
    /// <summary>
    /// Simple BFS that can use delegat, which tells, is cell free.
    /// </summary>
    public static class BreadthFirstSearch
    {
        public delegate bool IsPassable(Vector2Int pos);


        public static Queue<Vector2Int> FindPath(Vector2Int start, Vector2Int end, IsPassable isCellFree)
        {
            Queue<PathNode> SearchQueue = new Queue<PathNode>();
            SearchQueue.Enqueue(new PathNode(start));

            List<Vector2Int> Visited = new List<Vector2Int>() { start };
            while (SearchQueue.Count > 0)
            {
                PathNode Current = SearchQueue.Dequeue();

                foreach(Vector2Int pos in Current.GetNeighbours())
                {
                    PathNode NeighbourToCurrent = new PathNode(pos, Current);

                    if (pos == end)
                    {
                        return PathNodeToQueue(NeighbourToCurrent);
                    }
                    else if (!Visited.Contains(pos) && isCellFree(pos))
                    {
                        
                        SearchQueue.Enqueue(NeighbourToCurrent);
                        Visited.Add(pos);
                    }
                }
            }
            return null;
        }

        public static Queue<Vector2Int> PathNodeToQueue(PathNode node)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            List<Vector2Int> list = new List<Vector2Int>();
            list.Add(node.Pos);

            PathNode previous = node.Previous;
            while(previous != null)
            {
                list.Add(previous.Pos);
                previous = previous.Previous;
            }

            list.Reverse();

            foreach(Vector2Int pos in list)
            {
                queue.Enqueue(pos);
            }

            return queue;
        }

        public static List<Vector2Int> GetAvailableMoves(Vector2Int startPos, int moveDistance, IsPassable isPassable)
        {
            var availableMoves = new List<Vector2Int>() { startPos }; 
            
            Queue<PathNode> SearchQueue = new Queue<PathNode>();
            SearchQueue.Enqueue(new PathNode(startPos));

            List<Vector2Int> Visited = new List<Vector2Int>() { startPos };
            while (SearchQueue.Count > 0)
            {
                PathNode Current = SearchQueue.Dequeue();

                foreach (Vector2Int pos in Current.GetNeighbours())
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
        public Vector2Int Pos;
        public PathNode Previous;

        public int PathLength { get
            {
                return Previous is null ? 0 : Previous.PathLength + 1;
            } }

        public PathNode(Vector2Int pos)
        {
            Pos = pos;
        }

        public PathNode(Vector2Int pos, PathNode previous)
        {
            Pos = pos;
            Previous = previous;
        }

        #region All existing Neighbours
        public Vector2Int TopPos()
        {
            return Pos + new Vector2Int(0, 1);
        }
        public Vector2Int RightPos()
        {
            return Pos + new Vector2Int(1, 0);
        }
        public Vector2Int BottomPos()
        {
            return Pos + new Vector2Int(0, -1);
        }
        public Vector2Int LeftPos()
        {
            return Pos + new Vector2Int(-1, 0);
        }
        #endregion

        public IEnumerable<Vector2Int> GetNeighbours()
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
