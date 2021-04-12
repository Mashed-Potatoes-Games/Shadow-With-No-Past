using ShadowWithNoPast.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Healthpoint : MonoBehaviour
{
    public bool Active { get; private set; }

    public HealthpointObject HPObj;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image fill;

    private readonly float damageAnimationLenthInSeconds = 0.5f;
    private readonly float healAnimationLengthInSeconds = 0.25f;

    private readonly List<LinearAnimation> runningAnimations = new List<LinearAnimation>();


    private void Update()
    {
        for(int i = runningAnimations.Count - 1; i >= 0; i--)
        {
            if(runningAnimations[i].ContinueAnimation())
            {
                runningAnimations.RemoveAt(i);
            }
        }
    }

    public void Redraw(float elementSize, Color color)
    {
        if(background is null)
        {
            background = CreateHealthImageObject("Background", elementSize);
        }
        if(fill is null)
        {
            fill = CreateHealthImageObject("Fill", elementSize);
        }
        background.sprite = HPObj.HealthpointBackground;
        fill.sprite = HPObj.HealthpointFill;
        fill.color = color;
    }

    private Image CreateHealthImageObject(string name, float size)
    {
        var obj = new GameObject(name, typeof(Image));
        obj.transform.SetParent(transform, false);
        var image = obj.GetComponent<Image>();
        var rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(size, size);
        return image;
    }

    public void SetActive(bool animated = true)
    {
        if(animated)
        {
            SetActiveAnimated();
        }
        else
        {
            SetActiveInstant();
        }
    }

    public void SetInactive(bool animated = true)
    {
        if(animated)
        {
            SetInactiveAnimated();
        } else
        {
            SetInactiveInstant();
        }
    }

    private void SetActiveInstant()
    {
        fill.enabled = true;
        Active = true;
    }

    private void SetActiveAnimated()
    {
        if(Active)
        {
            return;
        }
        if (Application.isPlaying)
        {
            Active = true;
            GameObject obj = new GameObject("HealParticle");
            obj.transform.SetParent(transform, false);
            obj.transform.position = obj.transform.position;

            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sortingOrder++;
            renderer.sprite = fill.sprite;
            renderer.material = fill.materialForRendering;
            renderer.color = fill.color;


            Color32 newColor = renderer.color;
            newColor.a = 0;
            StartHealAnimation(obj);
            return;
        }

        SetActiveInstant();
    }

    private void StartHealAnimation(GameObject obj)
    {
        Vector3 initialPos = obj.transform.position;
        Vector3 distance = new Vector3(0, 0.25f);
        obj.transform.position += distance;

        Renderer renderer = obj.GetComponent<Renderer>();

        var color = renderer.material.color;
        color.a = 1;

        runningAnimations.Add(new LinearAnimation(
            obj,
            initialPos,
            null,
            renderer,
            color,
            healAnimationLengthInSeconds,
            () =>
            {
                Destroy(obj);
                if (Active)
                {
                    fill.enabled = true;
                    SetActiveInstant();
                }
            }));
    }

    private void SetInactiveInstant()
    {
        fill.enabled = false;
        Active = false;
    }

    private void SetInactiveAnimated()
    {
        if(!Active)
        {
            return;
        }
        SetInactiveInstant();
        StartDamageAnimation();
    }

    private void StartDamageAnimation()
    {
        if(Application.isPlaying)
        {
            foreach (var maskSprite in HPObj.MasksToCreateParticles)
            {
                SpriteRenderer sprite = CreateCutSprite(maskSprite);
                AnimateElementFall(sprite.gameObject);
            }
        }

    }

    private SpriteRenderer CreateCutSprite(Sprite maskSprite)
    {
        SpriteRenderer sprite = new GameObject("Cut", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        sprite.sprite = fill.sprite;
        sprite.color = fill.color;
        sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        sprite.transform.SetParent(transform, false);

        SpriteMask mask = new GameObject("Mask", typeof(SpriteMask)).GetComponent<SpriteMask>();
        mask.transform.SetParent(sprite.transform, false);
        mask.sprite = maskSprite;

        return sprite;
    }

    private void AnimateElementFall(GameObject obj)
    {
        Vector3 distance = new Vector3(UnityEngine.Random.Range(-1f, 1f), -0.5f);
        Vector3 endPos = obj.transform.position + distance;
        Vector3 rotation = Vector3.forward * UnityEngine.Random.Range(-360f, 360f);
        Vector3 endRotation = obj.transform.rotation.eulerAngles + rotation;
        Renderer renderer= obj.GetComponent<Renderer>();
        Color32 color = renderer.material.color;
        color.a = 0;

        runningAnimations.Add(new LinearAnimation(
            obj,
            endPos,
            endRotation,
            renderer,
            color,
            damageAnimationLenthInSeconds,
            () => 
            {
                Destroy(obj);
            }));

    }
    public void UpdateInEdtior()
    {
        EditorUtil.UpdateInEditor(fill);
        EditorUtil.UpdateInEditor(background);
    }

    private class LinearAnimation {

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
                               Vector3? endPos,
                               Vector3? endRotation,
                               Renderer renderer,
                               Color32? endColor,
                               float lengthInSec,
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
            this.renderer = renderer;

            this.startColor = renderer.material.color;
            this.endColor = endColor;
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
}
