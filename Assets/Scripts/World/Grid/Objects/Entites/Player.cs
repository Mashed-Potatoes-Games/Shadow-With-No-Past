using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [ExecuteAlways]
    public class Player : MonoBehaviour
    {
        private static GridEntity entity;

        static public GridEntity Entity
        {
            get
            {
                if (entity == null)
                {
                    Debug.Log("Player entity was not initialized, or destroyed, returning null");
                    return null;
                }
                return entity;
            }
        }
        public void Awake()
        {
            if (entity != null)
            {
                Debug.LogWarning("There are 2 or more player instances on the scene! " +
                    "Enemies will ignore them, except the last one, and something unexpected may happen aswell!");
            }
            entity = GetComponent<GridEntity>();
        }
    }
}