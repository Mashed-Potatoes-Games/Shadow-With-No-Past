using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GameProcess
{
    [ExecuteAlways]
    [RequireComponent(typeof(WorldsChanger), typeof(SceneLoader), typeof(GlobalEventManager))]
    public class Game : MonoBehaviour
    {
        public static WorldsChanger WorldsChanger => TryReturnComponent(worldsChanger);
        public static SceneLoader SceneLoader => TryReturnComponent(sceneLoader);
        public static GlobalEventManager GlobalEventManager => TryReturnComponent(globalEventManager);
        public static TurnsHandler TurnsHandler => TryReturnComponent(turnsHandler);

        [NonSerialized]
        private static WorldsChanger worldsChanger;
        [NonSerialized]
        private static SceneLoader sceneLoader;
        [NonSerialized]
        private static GlobalEventManager globalEventManager;
        [NonSerialized]
        private static TurnsHandler turnsHandler;

        void Awake()
        {
            InputControls.Enable();
            SetComponentValue(ref worldsChanger);
            SetComponentValue(ref sceneLoader);
            SetComponentValue(ref globalEventManager);
            SetComponentValue(ref turnsHandler);
        }

        private static T TryReturnComponent<T>(T component) where T : MonoBehaviour
        {
            if (component == null)
            {
                // This may occur on the scene unloading, while some objects may try to unsubscribe on destroy.
                Debug.LogWarning($"{typeof(T)} was not initialized, or destroyed.");
                return null;
            }
            return component;
        }

        private void SetComponentValue<T>(ref T component) where T : MonoBehaviour
        {
            if (component != null)
            {
                Debug.LogWarning($"There are 2 or more {typeof(T)} instances on the scene!");
            }
            component = GetComponent<T>();
        }
    }
}