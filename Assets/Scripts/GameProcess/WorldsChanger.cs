using System;
using UnityEngine;

[ExecuteAlways]
public class WorldsChanger : MonoBehaviour
{
    public WorldManagement CurrentlyActive;
    public WorldManagement CurrentlyInactive;

    void Start()
    {
        InitializeWorldsValue();
    }

    private void InitializeWorldsValue()
    {
        CurrentlyActive = null;
        CurrentlyInactive = null;

        FindAndSetWorlds();

        if(CurrentlyActive is null || CurrentlyInactive is null)
        {
            Debug.LogError($"One of the worlds is missing, this will lead to unexpected behaviour!");
        }
    }

    private void FindAndSetWorlds()
    {
        var worlds = GetComponentsInChildren<WorldManagement>();

        foreach (var world in worlds)
        {
            if (world.Active)
            {
                SetActiveValueTo(world);
            }

            if (!world.Active)
            {
                SetInactiveValueTo(world);
            }
        }
    }

    private void SetActiveValueTo(WorldManagement world)
    {
        if (!(CurrentlyActive is null))
        {
            Debug.LogError($"More than 1 world is active, this will lead to unexpected behaviour!");
        }
        CurrentlyActive = world;
    }
    private void SetInactiveValueTo(WorldManagement world)
    {
        if (!(CurrentlyInactive is null))
        {
            Debug.LogError($"More than 1 world is inactive, this will lead to unexpected behaviour!");
        }
        CurrentlyInactive = world;
    }

    public void ToggleActive()
    {
        CurrentlyActive.SetInactive();
        CurrentlyInactive.SetActive();

        (CurrentlyActive, CurrentlyInactive) = (CurrentlyInactive, CurrentlyActive);
    }
}