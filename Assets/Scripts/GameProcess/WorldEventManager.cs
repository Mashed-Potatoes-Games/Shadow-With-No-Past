using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShadowWithNoPast.Entities;
using System;

[RequireComponent(typeof(WorldManagement))]
public class WorldEventManager : MonoBehaviour
{
    private WorldManagement world;
    private void Awake()
    {
        world = GetComponent<WorldManagement>();

        world.Activated += world => WorldActivates?.Invoke(world);
        world.Inactivated += world => WorldDeactivates?.Invoke(world);

        world.ObjectAdded += OnWorldObjectAdded;
        world.ObjectRemoved += OnWorldObjectRemoved;
    }
    public event Action<WorldManagement> WorldActivates;
    public event Action<WorldManagement> WorldDeactivates;

    public event Action<GridObject> ObjectAdded;
    public event Action<GridObject> ObjectRemoved;

    public event Action<GridObject> ObjectMoved;

    public event Action<GridEntity> EntityDied;

    private void OnWorldObjectAdded(GridObject obj)
    {
        ObjectAdded?.Invoke(obj);
        obj.Moved += OnObjectMovement;

        if (obj is GridEntity entity)
        {
            entity.Died += OnEntityDied;
        }
    }

    private void OnWorldObjectRemoved(GridObject obj)
    {
        ObjectRemoved?.Invoke(obj);

        obj.Moved -= OnObjectMovement;

        if (obj is GridEntity entity)
        {
            entity.Died -= OnEntityDied;
        }
    }

    private void OnEntityDied(GridEntity obj)
    {
        EntityDied?.Invoke(obj);
    }

    private void OnObjectMovement(GridObject obj)
    {
        ObjectMoved?.Invoke(obj);
    }
}
