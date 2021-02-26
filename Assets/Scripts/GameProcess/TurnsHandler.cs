using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.GridObjects;

namespace ShadowWithNoPast.GameProcess
{
    public class TurnsHandler : MonoBehaviour
    {
        public bool IsWorking;

        private const float SecondsBetweenEnemiesMove = 0.5f;
        private Queue<BaseEntity> TurnsQueue;

        private GridManagement ActiveWorld;
        private GridManagement InactiveWorld;

        void Start()
        {
            TurnsQueue = new Queue<BaseEntity>();
            IsWorking = true;
            StartCoroutine(TurnsCoroutine());
        }

        public IEnumerator TurnsCoroutine()
        {
            yield return PlayerMove();
            while (IsWorking)
            {

                ReloadWorldReferences();
                yield return InitiateQueueAndTelegraph();
                Debug.Log("Enemies telegraph is over");

                yield return PlayerMove();
                Debug.Log("Player moved");

                yield return MakeTurns();
                Debug.Log("Enemies turn is over");
            }

        }

        private void ReloadWorldReferences()
        {
            foreach (GridManagement World in FindObjectsOfType<GridManagement>())
            {
                if (World.Active)
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

        public IEnumerator AddToQueueAndTelegraphFromWorld(GridManagement world)
        {
            if(world is null)
            {
                yield break;
            }
            foreach (TurnPriority priority in Enum.GetValues(typeof(TurnPriority)))
            {
                if(priority == TurnPriority.Player)
                {
                    continue;
                }
                foreach (BaseEntity entity in world.GetComponentsInChildren<BaseEntity>())
                {
                    if (entity.Priority == priority)
                    {
                        yield return entity.PrepareAndTelegraphMove();
                        TurnsQueue.Enqueue(entity);
                        yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
                    }
                }
            }
        }

        private IEnumerator PlayerMove()
        {
            PlayerEntity player = FindObjectOfType<PlayerEntity>();
            yield return player.ListenToInputAndMakeAMove();
        }

        public IEnumerator MakeTurns()
        {
            while (TurnsQueue.Count > 0)
            {
                BaseEntity entity = TurnsQueue.Dequeue();
                entity.ExecuteMove();
                yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
            }

            yield return new WaitForSeconds(SecondsBetweenEnemiesMove);
        }
    }
}
