using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Utils
{
    public static class RendererUtil
    {
        public static void ChangeRenderToLayer(Renderer renderer, string worldLayerName)
        {
            string sortingLayer = renderer.sortingLayerName;
            int layerID = GetSortingLayerID(worldLayerName, sortingLayer);

            if (layerID == 0)
            {
                Debug.LogError($"Code tried to apply layer to renderer, which doesn't exist.");
                return;
            }
            renderer.sortingLayerID = layerID;
        }

        public static void ChangeCanvasToLayer(Canvas canvas, string worldLayerName)
        {
            string sortingLayer = canvas.sortingLayerName;
            int layerID = GetSortingLayerID(worldLayerName, sortingLayer);

            if (layerID == 0)
            {
                Debug.LogError($"Code tried to apply layer to renderer, which doesn't exist.");
                return;
            }
            canvas.sortingLayerID = layerID;
        }

        private static int GetSortingLayerID(string worldLayerName, string sortingLayer)
        {
            string[] splittedStrings = sortingLayer.Split('/');

            if (CheckNestingLength(splittedStrings))
            {
                string layerToApply = worldLayerName + splittedStrings[1];
                return SortingLayer.NameToID(layerToApply);
            }
            return 0;
        }

        private static bool CheckNestingLength(string[] splitted)
        {
            if (splitted.Length != 2)
            {
                Debug.LogError($"Element has {splitted} layers applied which is not supported on worlds switch, " +
                               $"set appropriate layer name or modify the logic");
                return false;
            }
            return true;
        }
    }
}
