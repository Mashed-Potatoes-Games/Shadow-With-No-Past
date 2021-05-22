using System;
using UnityEngine;

public class LinearAnimation {

    public GameObject AnimatedObject;

    private readonly float animationLengthInSeconds;
    private float animationTimeProgress = 0;
    private Vector3 startingPos;
    private Vector3? endPos;
    private Vector3 startingRotation;
    private Vector3? endRotation;
    private readonly Renderer renderer;
    private Color32 startColor;
    private Color32? endColor;
    private readonly Action endAction;

    public LinearAnimation(GameObject animatedObject,
                            float lengthInSec,
                            Vector3? endPos,
                            Vector3? endRotation = null,
                            Color32? endColor = null,
                            Action endAction = null) {
        if(animatedObject is null)
        {
            throw new Exception("Animated object can't be null!");
        }
        this.AnimatedObject = animatedObject;
        this.startingPos = animatedObject.transform.position;
        this.startingRotation = animatedObject.transform.eulerAngles;
        this.endPos = endPos;
        this.endRotation = endRotation;
        if(animatedObject.TryGetComponent(out renderer))
        {
        this.startColor = renderer.material.color;
        this.endColor = endColor;
        }

        if(lengthInSec <= 0)
        {
            throw new Exception("Animation length can't be less or equal to zero!");
        }

        this.animationLengthInSeconds = lengthInSec;
        this.endAction = endAction;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns true if animation is over</returns>
    public bool ContinueAnimation()
    {
        animationTimeProgress += Time.deltaTime;
        float percentileProgress = animationTimeProgress / animationLengthInSeconds;

        if(endPos != null)
        {
            AnimatedObject.transform.position = 
                Vector3.Lerp(startingPos, endPos.Value, percentileProgress);
        }

        if(endRotation != null)
        {
            var newRotation = Vector3.Lerp(startingRotation, endRotation.Value, percentileProgress);
            AnimatedObject.transform.rotation = Quaternion.Euler(newRotation);
        }

        if(endColor != null)
        {
            renderer.material.color = Color32.Lerp(startColor, endColor.Value, percentileProgress);
        }

        if(animationTimeProgress >= animationLengthInSeconds)
        {
            endAction?.Invoke();
            return true;
        }

        return false;
    }
}
