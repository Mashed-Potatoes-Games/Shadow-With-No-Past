using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShadowWithNoPast.Entities;

[RequireComponent(typeof(WorldManagement))]
public class WorldEventManager : MonoBehaviour
{
    public class CoordinateUnityEvent : UnityEvent<GridEntity> { }

    public UnityEvent WorldActivates = new UnityEvent();
    public UnityEvent WorldDeactivates = new UnityEvent();

    public CoordinateUnityEvent EntityMoved = new CoordinateUnityEvent();
}
