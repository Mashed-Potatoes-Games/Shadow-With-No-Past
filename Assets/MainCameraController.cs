using ShadowWithNoPast.Entities;
using ShadowWithNoPast.GameProcess;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Camera))]
public class MainCameraController : Switchable
{
    [SerializeField]
    private Color32 edgeBackground;

    [SerializeField]
    private Color32 regularBackground;

    private LinearAnimation cameraMoving;

    private static float minCameraSize = 2f;
    private static float maxCameraSize = 10f;
    private float animationLengthInSeconds = 0.1f;
    private float animationTimeProgress;
    private float startingCameraSize;
    private float newCameraSize;

    private void Update()
    {
        var vector = InputControls.Instance.InGameMenu.CameraMovement.ReadValue<Vector2>();
        if(vector != Vector2.zero)
        {
            var pos = transform.position;
            pos.x += vector.x;
            pos.y += vector.y;
            cameraMoving = new LinearAnimation(gameObject, 0.1f, pos);

        }

        if (cameraMoving != null && cameraMoving.ContinueAnimation())
        {
            cameraMoving = null;
        }

        var mouseScroll = -InputControls.Instance.InGameMenu.Scroll.ReadValue<Vector2>().y;

        if(mouseScroll != 0)
        {
            startingCameraSize = Camera.main.orthographicSize;
            var modifier = 0.3f;
            var scrollForce = mouseScroll / 120f;
            newCameraSize = startingCameraSize + (startingCameraSize * (scrollForce * modifier));

            newCameraSize = Mathf.Min(newCameraSize, maxCameraSize);
            newCameraSize = Mathf.Max(newCameraSize, minCameraSize);

            animationTimeProgress = 0;
        }

        if (newCameraSize != 0 && animationTimeProgress >= 0)
        {
            animationTimeProgress += Time.deltaTime;
            float percentileProgress = animationTimeProgress / animationLengthInSeconds;

            Camera.main.orthographicSize = Mathf.Lerp(startingCameraSize, newCameraSize, percentileProgress);
            if(animationTimeProgress >= 1)
            {
                animationTimeProgress = 0;
                newCameraSize = 0;
            }
        }
    }

    protected override void SwitchTo(WorldType type)
    {
        Camera.main.backgroundColor = type switch { 
            WorldType.Regular => regularBackground,
            WorldType.TheEdge => edgeBackground,
            _ => throw new NotImplementedException()
        }; 
    }

    public void StartFollow(GridObject obj)
    {
        Follow(obj, obj.Pos, obj.Pos);
        obj.Moved += Follow;

    }

    public void StopFollow(GridObject obj)
    {
        obj.Moved -= Follow;
    }

    private void Follow(GridObject obj, WorldPos start, WorldPos end)
    {
        Vector3 Vector3Pos = obj.transform.position;
        Vector3Pos.z = transform.position.z;
        transform.position = Vector3Pos;
        Game.WorldsChanger.SetActive(end.World);
    }

}
