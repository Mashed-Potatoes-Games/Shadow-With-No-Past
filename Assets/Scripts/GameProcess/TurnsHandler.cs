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

        private const float SecondsBetweenEnemiesMove = 0.5f;
        private Queue<ITurnController> TurnsQueue = new Queue<ITurnController>();

        private WorldManagement ActiveWorld;
        private WorldManagement InactiveWorld;

        void Start()
        {
            IsWorking = true;
            StartCoroutine(TurnsCoroutine());
        }

        public IEnumerator TurnsCoroutine()
        {
            while (IsWorking)
            {

                ReloadWorldReferences();
                yield return InitiateQueueAndTelegraph();
                Debug.Log("Enemies telegraph is over");

                yield return MakeTurns();
                Debug.Log("Enemies turn is over");
            }

        }

        private void ReloadWorldReferences()
        {
            foreach (WorldManagement World in GetComponentsInChildren<WorldManagement>())
            {
                if (World.active)
                {
                    ActiveWorld = World;
                } 
                else
                {
                    InactiveWorld = World;
                }
            }
        }

        private IEnumerator InitiateQueueAndTelegraph()
        {
            yield return AddToQueueAndTelegraphFromWorld(ActiveWorld);
            yield return AddToQueueAndTelegraphFromWorld(InactiveWorld);
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
                foreach (var turnController in world.GetComponentsInChildren<ITurnController>())
                {
                    if (turnController.Priority == priority)
                    {
                        yield return turnController.PrepareAndTelegraphMove();
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
