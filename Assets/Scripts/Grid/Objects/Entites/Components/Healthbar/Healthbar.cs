using ShadowWithNoPast.Entities;
using ShadowWithNoPast.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Healthbar : MonoBehaviour
{
    [HideInInspector]
    public int StoredMaxHealth = 0;
    [HideInInspector]
    public int StoredHealth = 0;

    public float SecondsWaitBetweenSingleChanges = 0.1f;

    public float ElementSize = 0.25f;
    public float DistanceBetweenElement = 0.15f;

    public Color32 HealthColor = new Color32(255, 255, 255, 255);

    [SerializeField]
    private Healthpoint[] healthpoints = new Healthpoint[GridEntity.MAX_HEALTH];


    private void Start()
    {
    }

    public void Redraw()
    {
        for(int pos = 0; pos < GridEntity.MAX_HEALTH; pos++)
        {
            bool active = pos < StoredMaxHealth;
            SetHealthpointActive(pos, active);
            RedrawHealthpoint(pos);
        }

        SetHealth(StoredHealth);
    }

    private void RedrawHealthpoint(int pos)
    {
        healthpoints[pos].Redraw(ElementSize, HealthColor);
        EditorUtil.UpdateInEditor(healthpoints[pos]);
        healthpoints[pos].UpdateInEdtior();
    }

    public void SetMaxHealth(int inMaxHhealth)
    {
        ChangeHealthpointsVisibility(inMaxHhealth);

        StoredMaxHealth = inMaxHhealth;
        if (StoredHealth > inMaxHhealth)
        {
            StoredHealth = inMaxHhealth;
        }

        ArrangeHealthbar();

        EditorUtil.UpdateInEditor(this);
    }

    private void ChangeHealthpointsVisibility(int health)
    {
        for (int pos = 0; pos < GridEntity.MAX_HEALTH; pos++)
        {
            int currHealthpoint = pos + 1;
            if (currHealthpoint <= health && currHealthpoint > StoredMaxHealth)
            {
                SetHealthpointActive(pos, true);
            }

            if (currHealthpoint > health && currHealthpoint <= StoredMaxHealth)
            {
                SetHealthpointActive(pos, false);
            }
        }
    }

    private void SetHealthpointActive(int pos, bool value)
    {
        healthpoints[pos].gameObject.SetActive(value);
        EditorUtil.UpdateInEditor(healthpoints[pos].gameObject);
    }

    public void SetHealth(int health, bool animate = true)
    {
        if( health < 0)
        {
            Debug.LogError($"Health of healthbar ({health}) can't be negative!");
            return;
        }

        if(health > StoredMaxHealth)
        {
            health = StoredMaxHealth;
        }
        for(int i = 0; i < GridEntity.MAX_HEALTH; i++)
        {
            if(i < health)
            {
                healthpoints[i].SetActive(animate);
            }
            else if (i >= health && i < StoredHealth)
            {
                healthpoints[i].SetInactive(animate);
            }
            else
            {
                healthpoints[i].SetInactive(false);
            }
        }
        StoredHealth = health;
    }

    private void ArrangeHealthbar()
    {

        float healthbarLenght = ElementSize + (DistanceBetweenElement * (StoredMaxHealth - 1));

        float xOffset = (ElementSize - healthbarLenght) / 2;
        for(var i = 0; i < StoredMaxHealth; i++)
        {
            healthpoints[i].transform.localPosition = new Vector3(xOffset, 0);
            xOffset += DistanceBetweenElement;
        }
    }

    public void ChangeHealth(int health)
    {
        if(StoredHealth > health)
        {
            DealDamage(StoredHealth - health);
            return;
        }

        if(StoredHealth < health)
        {
            AddHealth(health - StoredHealth);
            return;
        }
    }

    public void DealDamage(int amount)
    {
        StartCoroutine(DealDamageCoroutine(amount));
    }

    private IEnumerator DealDamageCoroutine(int amount)
    {
        var neededHealth = StoredHealth - amount;
        if(neededHealth < 0)
        {
            neededHealth = 0;
        }

        for (int i = StoredHealth - 1; i >= neededHealth; i--)
        {
            healthpoints[i].SetInactive(true);

            // TODO: Animations/sound effect.

            yield return new WaitForSeconds(SecondsWaitBetweenSingleChanges);
        }
        StoredHealth = neededHealth;
    }

    public void AddHealth(int amount)
    {
        StartCoroutine(AddHealthCoroutine(amount));
    }

    private IEnumerator AddHealthCoroutine(int amount)
    {
        var neededHealth = StoredHealth + amount;
        if(neededHealth > StoredMaxHealth)
        {
            neededHealth = StoredMaxHealth;
        }

        for (int i = StoredHealth; i < neededHealth; i++)
        {
            healthpoints[i].SetActive(true);

            // TODO: Animations/sound effect.

            yield return new WaitForSeconds(SecondsWaitBetweenSingleChanges);
        }

        StoredHealth = neededHealth;
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(Healthbar))]
public class HealthbarEditor : Editor
{
    private Healthbar healthbar;
    private int cachedMaxHealth;
    private int cachedHealth;

    public override VisualElement CreateInspectorGUI()
    {
        healthbar = (Healthbar)target;
        cachedMaxHealth = healthbar.StoredMaxHealth;
        cachedHealth = healthbar.StoredHealth;
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EditorGUILayout.BeginHorizontal();

        int maxHealthInput = EditorGUILayout.IntSlider("Max Health", cachedMaxHealth, 1, GridEntity.MAX_HEALTH);
        if (maxHealthInput != cachedMaxHealth)
        {
            healthbar.SetMaxHealth(maxHealthInput);
            cachedMaxHealth = maxHealthInput;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        int healthInput = EditorGUILayout.IntSlider("Current Health", cachedHealth, 0, cachedMaxHealth);
        if (healthInput != cachedHealth) 
        {
            healthbar.SetHealth(healthInput);
            cachedHealth = healthInput;
        }

        EditorGUILayout.EndVertical();

        if(GUILayout.Button("Apply sprites changes"))
        {
            healthbar.Redraw();
            EditorUtility.SetDirty(healthbar);

        }
    }
}

#endif
