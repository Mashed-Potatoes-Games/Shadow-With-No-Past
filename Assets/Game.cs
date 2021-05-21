using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GameProcess
{
    public class Game : MonoBehaviour
    {

        private static WorldsChanger worldsChanger;
        [SerializeField]
        private GameObject PauseMenu;

        void Awake()
        {
            InputControls.Enable();
            InputControls.CancelButton.Add(Pause);
            InitiateWorldChanger();
        }
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

        private void InitiateWorldChanger()
        {
            if (worldsChanger != null)
            {
                Debug.LogWarning("There are 2 or more worldChanger instances on the scene! ");
                return;
            }
            worldsChanger = GetComponent<WorldsChanger>();
        }

        private void Pause()
        {
            Time.timeScale = 0;
            Debug.Log("GamePaused");
            InputControls.CancelButton.Remove(Pause);
            InputControls.CancelButton.AddInterrupting(Resume);
            if(PauseMenu == null)
            {
                Debug.LogWarning("PauseMenu wasn't assigned!");
                return;
            }
            PauseMenu.SetActive(true);
        }

        private bool Resume()
        {
            Time.timeScale = 1;
            Debug.Log("GameUnpaused");
            InputControls.CancelButton.Remove(Resume);
            InputControls.CancelButton.Add(Pause);
            if (PauseMenu == null)
            {
                Debug.LogWarning("PauseMenu wasn't assigned!");
                return true;
            }
            PauseMenu.SetActive(false);
            return true;
        }
    }
}