using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entities;

namespace ShadowWithNoPast.GameProcess
{
    public class TurnsHandler : MonoBehaviour
    {
        public bool IsWorking;

        private const float SecondsBetweenEnemiesMove = 0.1f;
        private Queue<ITurnController> TurnsQueue = new Queue<ITurnController>();

        private WorldsChanger changer;

        void Start()
        {
            changer = GetComponent<WorldsChanger>();

            IsWorking = true;
            StartCoroutine(TurnsCoroutine());
        }

        public IEnumerator TurnsCoroutine()
        {
            while (IsWorking)
            {
                yield return InitiateQueueAndTelegraph();
                Debug.Log("Enemies telegraph is over");

                yield return MakeTurns();
                Debug.Log("Enemies turn is over");
            }

        }

        private IEnumerator InitiateQueueAndTelegraph()
        {
            yield return AddToQueueAndTelegraphFromWorld(changer.CurrentlyActive);
            yield return AddToQueueAndTelegraphFromWorld(changer.CurrentlyInactive);
        }

        public IEnumerator AddToQueueAndTelegraphFromWorld(WorldManagement world)
        {
            if(world is null)
            {
                yield break;
            }

            var priorities = Enum.GetValues(typeof(TurnPriority));
            foreach (TurnPriority priority in priorities)
            {
                var turnControllers = world.objects.GetComponentsInChildren<ITurnController>();
                foreach (var turnController in turnControllers)
                {
                    if (turnController.Priority == priority)
                    {
                        yield return turnController.MoveAndTelegraphAction();
                        TurnsQueue.Enqueue(turnController);
                        yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
                    }
                }
            }
        }

        public IEnumerator MakeTurns()
        {
            while (TurnsQueue.Count > 0)
            {
                var turnController = TurnsQueue.Dequeue();
                yield return turnController.ExecuteMove();
                yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
            }
        }
    }
}
