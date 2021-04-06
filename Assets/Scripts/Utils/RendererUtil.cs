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
            string[] splittedStrings = sortingLayer.Split('/');

            if (CheckNestingLength(splittedStrings))
            {
                string layerToApply = worldLayerName + splittedStrings[1];
                int layerID = SortingLayer.NameToID(layerToApply);
                if (layerID == 0)
                {
                    Debug.LogError($"Code tried to apply {layerToApply} layer to renderer, which doesn't exist.");
                    return;
                }
                renderer.sortingLayerID = layerID;
            }
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
