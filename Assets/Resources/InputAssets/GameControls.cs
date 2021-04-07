// GENERATED AUTOMATICALLY FROM 'Assets/Resources/InputActions/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""AbilityUsage"",
            ""id"": ""ce6be5a2-1f5a-438b-8bce-9ede0816c4ef"",
            ""actions"": [
                {
                    ""name"": ""Ability 1"",
                    ""type"": ""Button"",
                    ""id"": ""efb7eb3e-c743-4875-9c3e-d5daf9e03dff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""02131171-122f-4604-bbb8-42bc4ee33f99"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // AbilityUsage
        m_AbilityUsage = asset.FindActionMap("AbilityUsage", throwIfNotFound: true);
        m_AbilityUsage_Ability1 = m_AbilityUsage.FindAction("Ability 1", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // AbilityUsage
    private readonly InputActionMap m_AbilityUsage;
    private IAbilityUsageActions m_AbilityUsageActionsCallbackInterface;
    private readonly InputAction m_AbilityUsage_Ability1;
    public struct AbilityUsageActions
    {
        private @GameControls m_Wrapper;
        public AbilityUsageActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Ability1 => m_Wrapper.m_AbilityUsage_Ability1;
        public InputActionMap Get() { return m_Wrapper.m_AbilityUsage; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AbilityUsageActions set) { return set.Get(); }
        public void SetCallbacks(IAbilityUsageActions instance)
        {
            if (m_Wrapper.m_AbilityUsageActionsCallbackInterface != null)
            {
                @Ability1.started -= m_Wrapper.m_AbilityUsageActionsCallbackInterface.OnAbility1;
                @Ability1.performed -= m_Wrapper.m_AbilityUsageActionsCallbackInterface.OnAbility1;
                @Ability1.canceled -= m_Wrapper.m_AbilityUsageActionsCallbackInterface.OnAbility1;
            }
            m_Wrapper.m_AbilityUsageActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Ability1.started += instance.OnAbility1;
                @Ability1.performed += instance.OnAbility1;
                @Ability1.canceled += instance.OnAbility1;
            }
        }
    }
    public AbilityUsageActions @AbilityUsage => new AbilityUsageActions(this);
    public interface IAbilityUsageActions
    {
        void OnAbility1(InputAction.CallbackContext context);
    }
}
