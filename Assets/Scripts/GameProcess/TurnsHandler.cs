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
        public event Action TurnPassed;

        public bool IsWorking;
        public TurnSystemState State = TurnSystemState.Exploration;

        private const float SecondsBetweenEnemiesMove = 0.25f;
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
                State = TurnSystemState.Exploration;
                yield return InitiateQueueAndTelegraph();

                yield return MakeTurns();
                TurnPassed?.Invoke();
            }

        }

        private IEnumerator InitiateQueueAndTelegraph()
        {
            yield return AddToQueueAndTelegraphFromWorld(changer.CurrentlyActive);
            yield return AddToQueueAndTelegraphFromWorld(changer.CurrentlyInactive);
        }

        private IEnumerator AddToQueueAndTelegraphFromWorld(World world)
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
                        if(entity.TurnController.EngageCombat())
                        {
                            State = TurnSystemState.Battle;
                            Game.MainCameraController.StartFollow(entity);
                            yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
                            yield return entity.TurnController.MoveAndTelegraphAction();
                            Game.MainCameraController.StopFollow(entity);
                            // Add to ITurnController function to check if entity going to execute next move
                            // Implement in all turn controllers
                            // Call it here and if it's going to execute move, add to queue, otherwise do nothing
                            if (entity.TurnController.ReadyToExecute())
                            {
                                EntitiesQueue.Enqueue(entity);
                            }

                            yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
                        }
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
                Game.MainCameraController.StartFollow(entity);
                yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
                yield return entity.TurnController.ExecuteMove();
                Game.MainCameraController.StartFollow(entity);
                yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
            }
        }
    }

    public enum TurnSystemState
    {
        Battle,
        Exploration
    }
}
