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

    private float animationUpdateRateInSeconds = 0.02f;
    private float damageAnimationLengthInSeconds = 0.5f;
    private float healAnimationLengthInSeconds = 0.25f;
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
            Image image = Instantiate(fill, transform);
            image.enabled = true;
            StartCoroutine(AnimateHeal(image));
            return;
        }

        SetActiveInstant();
    }

    private IEnumerator AnimateHeal(Image image)
    {
        Vector3 initialPos = image.transform.position;
        Vector3 distance = new Vector3(0, 0.25f);
        image.transform.position += distance;

        float step = healAnimationLengthInSeconds * animationUpdateRateInSeconds;
        for (float secLeft = healAnimationLengthInSeconds; secLeft > 0; secLeft -= animationUpdateRateInSeconds)
        {
            float completion = 1 - (secLeft / healAnimationLengthInSeconds);
            image.transform.position = initialPos + (distance * (1 - completion));
            var newColor = image.color;
            newColor.a = completion;
            image.color = newColor;
            yield return new WaitForSeconds(animationUpdateRateInSeconds);
        }
        Destroy(image.gameObject);
        if(Active)
        {
            fill.enabled = true;
        }
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
        AnimateDamage();
    }

    private void AnimateDamage()
    {
        if(Application.isPlaying)
        {
            foreach (var maskSprite in HPObj.MasksToCreateParticles)
            {
                SpriteRenderer sprite = CreateCutSprite(maskSprite);
                StartCoroutine(AnimateElementFall(sprite));
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

    private IEnumerator AnimateElementFall(SpriteRenderer sprite)
    {
        Vector3 distance = new Vector3(UnityEngine.Random.Range(-1f, 1f), -0.5f);

        float step = damageAnimationLengthInSeconds * animationUpdateRateInSeconds;
        for (float secLeft = damageAnimationLengthInSeconds; secLeft >= 0; secLeft -= animationUpdateRateInSeconds)
        {
            float completion = secLeft / damageAnimationLengthInSeconds;

            sprite.transform.position += distance * step;
            sprite.transform.Rotate(Vector3.forward * step * 180f);
            var newColor = sprite.color;
            newColor.a = completion;
            sprite.color = newColor;
            yield return new WaitForSeconds(animationUpdateRateInSeconds);
        }
        Destroy(sprite.gameObject);
    }
    public void UpdateInEdtior()
    {
        EditorUtil.UpdateInEditor(fill);
        EditorUtil.UpdateInEditor(background);
    }
}
