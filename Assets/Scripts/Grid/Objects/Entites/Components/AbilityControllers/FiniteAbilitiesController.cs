using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShadowWithNoPast.Entities.Abilities
{
    [RequireComponent(typeof(GridEntity))]
    public class FiniteAbilitiesController : MonoBehaviour, IAbilitiesController
    {
        public event Action<AbilityInstance> AbilityUsed;
        public event Action<AbilityInstance> AbilityUsedWithNoTarget;

        private GridEntity entity;
        private ITurnController turnController;

        public List<AbilityInstance> Abilities;


        public AbilityInstance this[int i] => Abilities[i];
        public int Count => Abilities.Count;


        void Awake()
        {
            entity = GetComponent<GridEntity>();
            turnController = GetComponent<ITurnController>();

            if(turnController != null)
            {
                turnController.TurnPassed += OnTurnPassed;
            }

            foreach(AbilityInstance instance in Abilities)
            {
                instance.Used += () =>
                {
                    AbilityUsed?.Invoke(instance);
                };
                instance.UsedWithNoTarget += () =>
                {
                    AbilityUsedWithNoTarget?.Invoke(instance);
                };
            }
        }

        public void ExecutePassiveAbilities()
        {
            throw new System.NotImplementedException();
        }

        public List<AbilityInstance> GetAbilities()
        {
            throw new System.NotImplementedException();
        }

        public List<AbilityInstance> GetReadyAbilities()
        {
            throw new System.NotImplementedException();
        }

        private void OnTurnPassed()
        {
            foreach (var ability in Abilities)
            {
                ability.OnTurnPassed();
            }
        }

        public IEnumerator<AbilityInstance> GetEnumerator()
        {
            return Abilities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Abilities.GetEnumerator();
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(FiniteAbilitiesController))]
    public class AbilitiesControllerEditor : Editor
    {
        private FiniteAbilitiesController abilitiesController;
        private int currentPickerWindow;
        private AbilityInstance focusedAbility;

        private void OnEnable()
        {
            abilitiesController = (FiniteAbilitiesController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add new ability"))
            {
                ShowPicker();
            }

            if (Event.current.commandName == "ObjectSelectorUpdated" && 
                EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
            {
                currentPickerWindow = -1;
                CreateNewAbilityInstance((Ability)EditorGUIUtility.GetObjectPickerObject());
                EditorUtility.SetDirty(abilitiesController);

            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(abilitiesController);
            }
        }

        private void ShowPicker()
        {
            //create a window picker control ID
            currentPickerWindow = GUIUtility.GetControlID(FocusType.Passive) + 100;

            EditorGUIUtility.ShowObjectPicker<Ability>(null, false, "", currentPickerWindow);
        }

        private void CreateNewAbilityInstance(Ability ability)
        {
            focusedAbility = new AbilityInstance(abilitiesController.GetComponent<GridEntity>(), ability, 1);
            abilitiesController.Abilities.Add(focusedAbility);
        }
    }
#endif

}