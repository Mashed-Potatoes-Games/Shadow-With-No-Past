using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entities;
using System.Linq;

namespace ShadowWithNoPast.GameProcess
{
    public class TurnsHandler : MonoBehaviour
    {
        public bool IsWorking;

        private const float SecondsBetweenEnemiesMove = 0.1f;
        private Queue<GridEntity> EntitiesQueue = new Queue<GridEntity>();

        private WorldsChanger changer;

        void Start()
        {
            changer = GetComponent<WorldsChanger>();

            IsWorking = true;
            StartCoroutine(TurnsCoroutine());
        }

        private IEnumerator TurnsCoroutine()
        {
            while (IsWorking)
            {
                yield return InitiateQueueAndTelegraph();

                yield return MakeTurns();
            }

        }

        private IEnumerator InitiateQueueAndTelegraph()
        {
            yield return AddToQueueAndTelegraphFromWorld(changer.CurrentlyActive);
            yield return AddToQueueAndTelegraphFromWorld(changer.CurrentlyInactive);
        }

        private IEnumerator AddToQueueAndTelegraphFromWorld(WorldManagement world)
        {
            if(world is null)
            {
                yield break;
            }

            world.AttacksAccounter.Clear();

            var priorities = Enum.GetValues(typeof(TurnPriority));
            foreach (TurnPriority priority in priorities)
            {
                var entities = world.GetEntities();
                foreach (var entity in entities)
                {
                    if (entity == null) continue;

                    if (entity.TurnController.Priority == priority)
                    {
                        yield return entity.TurnController.MoveAndTelegraphAction();
                        EntitiesQueue.Enqueue(entity);
                        yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
                    }
                }
            }
        }

        private IEnumerator MakeTurns()
        {
            while (EntitiesQueue.Count > 0)
            {
                var entity = EntitiesQueue.Dequeue();
                if (entity == null) continue;
                yield return entity.TurnController.ExecuteMove();
                yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
            }
        }
    }
}
