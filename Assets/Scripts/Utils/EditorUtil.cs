using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShadowWithNoPast.Utils
{
    public static class EditorUtil
    {
        public static void UpdateInEditor(Object obj)
        {
            if(!Application.isPlaying)
            {
                EditorUtility.SetDirty(obj);
            }
        }
    }
}