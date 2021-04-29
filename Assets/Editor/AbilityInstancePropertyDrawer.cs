using ShadowWithNoPast.Entities.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(AbilityInstance))]
public class AbilityInstancePropertyDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;

        foreach(SerializedProperty child in property)
        {
            totalHeight += EditorGUI.GetPropertyHeight(child, label, true) + EditorGUIUtility.standardVerticalSpacing;
        }

        return totalHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect propertyPosition = position;
        propertyPosition.height += position.height;
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(propertyPosition, label, property);
        
        Rect pos = position;

        foreach (SerializedProperty child in property)
        {
            var height = EditorGUI.GetPropertyHeight(child, true);
            pos.height = height;
            pos.y = pos.y += height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(pos, child);
        }

        // Set indent back to what it was
        //EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
