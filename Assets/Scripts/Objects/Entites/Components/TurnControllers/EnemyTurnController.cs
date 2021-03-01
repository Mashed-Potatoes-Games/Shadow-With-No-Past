using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridEntity))]
    public class EnemyTurnController : MonoBehaviour, ITurnController
    {
        public TurnPriority Priority { get; set; } = TurnPriority.Normal;

        public int MoveDistance { get; set; } = 1;

       private float delayInSecBetweenCellsMove = 0.1f;

        private GridEntity entity;
        private WorldManagement world;
        private IMovementController movement;

        // Start is called before the first frame update
        void Start()
        {
            entity = GetComponent<GridEntity>();
            world = entity.WorldGrid;
            movement = GetComponent<IMovementController>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public IEnumerator PrepareAndTelegraphMove()
        {
            GridEntity player = FindPlayer();

            Queue<Vector2Int> path = movement.GetPath(player.CurrentPos, false);

            if (path != null)
            {
                yield return MoveWithDelay(path);
            }
        }

        public IEnumerator ExecuteMove()
        {
            yield break;
        }

        public Queue<Vector2Int> MovementQueue = new Queue<Vector2Int>();

        public IEnumerator MoveWithDelay(Queue<Vector2Int> path)
        {
            int movesLeft = MoveDistance;
            while (path.Count > 0 && movesLeft > 0)
            {
                Vector2Int pos = path.Dequeue();
                if (!movement.TryInstantMoveTo(pos))
                {
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(delayInSecBetweenCellsMove);
                    movesLeft--;
                }
            }
        }

        public GridEntity FindPlayer()
        {
            foreach (var entity in world.GetComponentsInChildren<GridEntity>())
            {
                if (entity.CompareTag("Player"))
                    return entity;
            }

            return null;
        }
    }

}