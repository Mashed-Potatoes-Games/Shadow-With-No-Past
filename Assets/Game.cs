using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GameProcess
{
    [RequireComponent(typeof(WorldsChanger), typeof(SceneLoader))]
    public class Game : MonoBehaviour
    {
        public static WorldsChanger WorldsChanger => ReturnComponent(worldsChanger);
        public static SceneLoader SceneLoader => ReturnComponent(sceneLoader);

        private static WorldsChanger worldsChanger;
        private static SceneLoader sceneLoader;

        void Awake()
        {
            InputControls.Enable();
            SetComponentValue(ref worldsChanger);
            SetComponentValue(ref sceneLoader);
        }

        private static T ReturnComponent<T>(T component) where T : MonoBehaviour
        {
            if (component == null)
            {
                Debug.LogWarning($"{component.GetType()} was not initialized, or destroyed, returning null");
                return null;
            }
            return component;
        }

        private void SetComponentValue<T>(ref T component) where T : MonoBehaviour
        {
            if (worldsChanger != null)
            {
                Debug.LogWarning("There are 2 or more worldChanger instances on the scene!");
                return;
            }
            component = GetComponent<T>();
        }
    }
}