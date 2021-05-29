using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShadowWithNoPast.Entities;
using System;

[RequireComponent(typeof(World))]
public class WorldEventManager : MonoBehaviour
{
    private World world;
    private void Start()
    {
        world = GetComponent<World>();

        world.Activated += world => WorldActivates?.Invoke(world);
        world.Inactivated += world => WorldDeactivates?.Invoke(world);

        world.GetObjects().ForEach(obj => AddListenersToObject(obj, obj.Pos));

        world.ObjectAdded += AddListenersToObject;
        world.ObjectRemoved += RemoveListenersToObject;
    }
    public event Action<World> WorldActivates;
    public event Action<World> WorldDeactivates;

    public event Action<GridObject, WorldPos> ObjectAdded;
    public event Action<GridObject, WorldPos> ObjectRemoved;

    public event Action<GridObject, WorldPos, WorldPos> ObjectMoved;

    public event Action<GridEntity, WorldPos> EntityDied;

    private void AddListenersToObject(GridObject obj, WorldPos pos)
    {
        ObjectAdded?.Invoke(obj, pos);
        obj.Moved += OnObjectMovement;

        if (obj is GridEntity entity)
        {
            entity.Died += OnEntityDied;
        }
    }

    private void RemoveListenersToObject(GridObject obj, WorldPos pos)
    {
        ObjectRemoved?.Invoke(obj, pos);

        obj.Moved -= OnObjectMovement;

        if (obj is GridEntity entity)
        {
            entity.Died -= OnEntityDied;
        }
    }

    private void OnEntityDied(GridEntity obj, WorldPos pos)
    {
        EntityDied?.Invoke(obj, pos);
    }

    private void OnObjectMovement(GridObject obj, WorldPos from, WorldPos to)
    {
        ObjectMoved?.Invoke(obj, from, to);
    }
}
