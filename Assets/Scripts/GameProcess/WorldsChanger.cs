using ShadowWithNoPast.Entities;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ShadowWithNoPast.Utils;

namespace ShadowWithNoPast.GameProcess
{
    [ExecuteAlways]
    public class WorldsChanger : MonoBehaviour
    {
        public event Action<World> WorldsSwitched;
        public WorldType Active => CurrentlyActive.Type;
        public World CurrentlyActive;
        public World CurrentlyInactive;

        void Start()
        {
            InitializeWorldsValue();
        }

        private void InitializeWorldsValue()
        {
            CurrentlyActive = null;
            CurrentlyInactive = null;

            FindAndSetWorlds();

            if (CurrentlyActive is null || CurrentlyInactive is null)
            {
                Debug.LogError($"One of the worlds is missing, this will lead to unexpected behaviour!");
            }
        }

        private void FindAndSetWorlds()
        {
            var worlds = GetComponentsInChildren<World>();

            foreach (var world in worlds)
            {
                world.gameObject.SetActive(true);
                if (world.Active)
                {
                    SetActiveValueTo(world);
                }

                if (!world.Active)
                {
                    SetInactiveValueTo(world);
                }
            }
        }

        private void SetActiveValueTo(World world)
        {
            if (!(CurrentlyActive is null))
            {
                Debug.LogError($"More than 1 world is active, this will lead to unexpected behaviour!");
            }
            CurrentlyActive = world;
        }
        private void SetInactiveValueTo(World world)
        {
            if (!(CurrentlyInactive is null))
            {
                Debug.LogError($"More than 1 world is inactive, this will lead to unexpected behaviour!");
            }
            CurrentlyInactive = world;
        }

        public void ToggleActive()
        {
            CurrentlyActive.SetActive(false);
            CurrentlyInactive.SetActive(true);

            (CurrentlyActive, CurrentlyInactive) = (CurrentlyInactive, CurrentlyActive);

            WorldsSwitched?.Invoke(CurrentlyActive);
        }

        public void SetActive(World world, bool active = false)
        {
            if(world == CurrentlyActive ^ active)
            {
                ToggleActive();
            }
        }

        public void MoveToOtherWorld(GridEntity obj)
        {
            World currentWorld = obj.World;
            World other = new List<World> { CurrentlyActive, CurrentlyInactive }
            .Find((world) => currentWorld != world);
            var vector = obj.Vector;
            if (other.GetCellStatus(vector) == CellStatus.Free)
            {
                currentWorld.Remove(obj);
                other.SetNewObjectTo(obj, vector);
                obj.SetNewPosition(new WorldPos(other, vector));
            }
        }
    }
}