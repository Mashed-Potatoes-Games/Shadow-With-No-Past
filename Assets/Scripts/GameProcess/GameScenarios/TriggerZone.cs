using ShadowWithNoPast.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
#endif

[Serializable]
public class TriggerZone
{
    public World World;
    public List<Vector2Int> Zones;


    [SerializeField]
    private GridObject triggerObject;

    public SendToLevelAction TriggerAction = new SendToLevelAction();

    private GameObject triggerZonesGroup;

    private TargetType targetType = TargetType.Player;

    public bool Enabled { get; private set; } = false;



    public TriggerZone(World world, List<Vector2Int> zones, ITriggerAction action, TargetType targetType, params GridEntity[] entities)
    {
        World = world;
        Zones = zones;
        this.targetType = targetType;
    }

    public virtual void SetExploration()
    {
        if(!Enabled)
        {
            Activate();
            Enabled = true;
        }
    }

    public virtual void SetBattle()
    {
        if(Enabled)
        {
            Deactivate();
            Enabled = false;
        }
    }

    private void Deactivate()
    {
        switch (targetType)
        {
            case TargetType.Player:
                Player.Entity.Moved -= CheckTargetMove;
                break;
            case TargetType.AnyEntity:
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }

        if (triggerZonesGroup != null)
        {
            triggerZonesGroup.SetActive(false);
        }
    }

    private void Activate()
    {

        switch (targetType)
        {
            case TargetType.Player:
                Player.Entity.Moved += CheckTargetMove;
                break;
            case TargetType.AnyEntity:
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }

        if (triggerZonesGroup != null)
        {
            triggerZonesGroup.SetActive(true);
            return;
        }

        triggerZonesGroup = new GameObject("TriggerZones");
        triggerZonesGroup.transform.SetParent(World.transform);
        foreach(var pos in Zones)
        {
            if(triggerObject != null)
            {
                var instance = GameObject.Instantiate(triggerObject);
                if(instance.TryGetComponent<Collider2D>(out var collider)) {
                    collider.enabled = false;
                }
                instance.transform.SetParent(triggerZonesGroup.transform);
                instance.SetNewPosition(pos);
            }
        }
    }

    private void CheckTargetMove(GridObject trigerer, WorldPos startPos, WorldPos endPos)
    {
        if (Enabled && World == endPos.World && Zones.Any(pos => pos == endPos.Vector))
        {
            TriggerAction.Action(trigerer);
        }
    }
}
#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(TriggerZone))]
public class IngredientDrawerUIE : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
            return EditorGUI.GetPropertyHeight(property) + 20f;
        return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
        position.y += position.height;
        position.height = 20f;
        if(property.isExpanded)
        {
            if (GUI.Button(position, "Set SendToLevelAction"))
            {
                property.FindPropertyRelative("TriggerAction").SetValue<ITriggerAction>(new SendToLevelAction());
            }
        }
    }
}
#endif

public enum TargetType
{
    Player,
    AnyEntity
}