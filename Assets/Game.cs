using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GameProcess
{
    [RequireComponent(typeof(WorldsChanger), typeof(SceneLoader))]
    public class Game : MonoBehaviour
    {
        public static WorldsChanger WorldsChanger => TryReturnComponent(worldsChanger);
        public static SceneLoader SceneLoader => TryReturnComponent(sceneLoader);

        [NonSerialized]
        private static WorldsChanger worldsChanger;
        [NonSerialized]
        private static SceneLoader sceneLoader;

        void Awake()
        {
            InputControls.Enable();
            SetComponentValue(ref worldsChanger);
            SetComponentValue(ref sceneLoader);
        }

        private static T TryReturnComponent<T>(T component) where T : MonoBehaviour
        {
            if (component == null)
            {
                Debug.LogWarning($"{component.GetType()} was not initialized, or destroyed.");
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