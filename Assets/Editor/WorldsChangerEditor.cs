using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldsChanger))]
public class WorldsChangerEditor : Editor
{
    WorldsChanger changer;

    private void OnEnable()
    {
        changer = (WorldsChanger)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Switch active world")) {
            changer.ToggleActive();
            EditorUtility.SetDirty(changer);
        }
    }
}