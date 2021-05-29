using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ShadowWithNoPast.Entities;

public class GlobalEventManager : MonoBehaviour
{
    public event Action<GridObject, WorldPos> ObjectAdded;
    public event Action<GridObject, WorldPos> ObjectRemoved;

    public event Action<GridObject, WorldPos, WorldPos> ObjectMoved;

    public event Action<GridEntity, WorldPos> EntityDied;

    private List<WorldEventManager> eventManagers;

    void Start()
    {
        eventManagers = FindObjectsOfType<WorldEventManager>().ToList();
        foreach (var eventManager in eventManagers)
        {
            eventManager.ObjectAdded += ObjectAdded;
            eventManager.ObjectMoved += ObjectMoved;
            eventManager.ObjectRemoved += ObjectRemoved;
            eventManager.EntityDied += EntityDied;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
