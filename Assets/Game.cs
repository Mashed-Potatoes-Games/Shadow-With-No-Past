using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GameProcess
{
    public class Game : MonoBehaviour
    {

        private static WorldsChanger worldsChanger;

        public static WorldsChanger WorldsChanger
        {
            get
            {
                if (worldsChanger == null)
                {
                    Debug.Log("WorldsChanger was not initialized, or destroyed, returning null");
                    return null;
                }
                return worldsChanger;
            }
        }

        void Awake()
        {
            if (worldsChanger != null)
            {
                Debug.LogWarning("There are 2 or more worldChanger instances on the scene! ");
                return;
            }
            worldsChanger = GetComponent<WorldsChanger>();
        }
    }
}