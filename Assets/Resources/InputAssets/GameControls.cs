// GENERATED AUTOMATICALLY FROM 'Assets/Resources/InputAssets/GameControls.inputactions'

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
                },
                {
                    ""name"": ""SkipMove"",
                    ""type"": ""Button"",
                    ""id"": ""a3281fb2-238a-461b-9481-47eb58002776"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""b3ec1cd8-8225-49e7-9069-e3f62feff9a5"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""InGameMenu"",
            ""id"": ""a5fb702c-0295-4cf0-a8a8-ca13f97ba7ae"",
            ""actions"": [
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""cdd0985a-c5f7-47c9-b80b-2ed26ab79909"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2073346e-bb7a-4332-bb13-28d933ed60ac"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""2f9a3428-6d97-4c2d-991a-5d2d3efb60ed"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b0504dfb-acc9-48b8-bb9a-00be2745c946"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""91a3e3af-40d3-46a5-ba31-0406503f1eb1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""070b70ef-1df4-4ab6-ba00-bdd89e3e7388"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""70727827-1e99-4c23-877b-dc19dc3c5224"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b47a9a21-5f41-4bb8-a956-c6fe65293b8e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""74ff44ea-b62b-4d0c-88dd-8984be490004"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5ab97c88-df1a-4908-a97a-8dbb5a822ac8"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
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
        m_AbilityUsage_SkipMove = m_AbilityUsage.FindAction("SkipMove", throwIfNotFound: true);
        // InGameMenu
        m_InGameMenu = asset.FindActionMap("InGameMenu", throwIfNotFound: true);
        m_InGameMenu_Cancel = m_InGameMenu.FindAction("Cancel", throwIfNotFound: true);
        m_InGameMenu_CameraMovement = m_InGameMenu.FindAction("CameraMovement", throwIfNotFound: true);
        m_InGameMenu_Scroll = m_InGameMenu.FindAction("Scroll", throwIfNotFound: true);
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
    private readonly InputAction m_AbilityUsage_SkipMove;
    public struct AbilityUsageActions
    {
        private @GameControls m_Wrapper;
        public AbilityUsageActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Ability1 => m_Wrapper.m_AbilityUsage_Ability1;
        public InputAction @SkipMove => m_Wrapper.m_AbilityUsage_SkipMove;
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
                @SkipMove.started -= m_Wrapper.m_AbilityUsageActionsCallbackInterface.OnSkipMove;
                @SkipMove.performed -= m_Wrapper.m_AbilityUsageActionsCallbackInterface.OnSkipMove;
                @SkipMove.canceled -= m_Wrapper.m_AbilityUsageActionsCallbackInterface.OnSkipMove;
            }
            m_Wrapper.m_AbilityUsageActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Ability1.started += instance.OnAbility1;
                @Ability1.performed += instance.OnAbility1;
                @Ability1.canceled += instance.OnAbility1;
                @SkipMove.started += instance.OnSkipMove;
                @SkipMove.performed += instance.OnSkipMove;
                @SkipMove.canceled += instance.OnSkipMove;
            }
        }
    }
    public AbilityUsageActions @AbilityUsage => new AbilityUsageActions(this);

    // InGameMenu
    private readonly InputActionMap m_InGameMenu;
    private IInGameMenuActions m_InGameMenuActionsCallbackInterface;
    private readonly InputAction m_InGameMenu_Cancel;
    private readonly InputAction m_InGameMenu_CameraMovement;
    private readonly InputAction m_InGameMenu_Scroll;
    public struct InGameMenuActions
    {
        private @GameControls m_Wrapper;
        public InGameMenuActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Cancel => m_Wrapper.m_InGameMenu_Cancel;
        public InputAction @CameraMovement => m_Wrapper.m_InGameMenu_CameraMovement;
        public InputAction @Scroll => m_Wrapper.m_InGameMenu_Scroll;
        public InputActionMap Get() { return m_Wrapper.m_InGameMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameMenuActions set) { return set.Get(); }
        public void SetCallbacks(IInGameMenuActions instance)
        {
            if (m_Wrapper.m_InGameMenuActionsCallbackInterface != null)
            {
                @Cancel.started -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnCancel;
                @CameraMovement.started -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.performed -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.canceled -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnCameraMovement;
                @Scroll.started -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_InGameMenuActionsCallbackInterface.OnScroll;
            }
            m_Wrapper.m_InGameMenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @CameraMovement.started += instance.OnCameraMovement;
                @CameraMovement.performed += instance.OnCameraMovement;
                @CameraMovement.canceled += instance.OnCameraMovement;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
            }
        }
    }
    public InGameMenuActions @InGameMenu => new InGameMenuActions(this);
    public interface IAbilityUsageActions
    {
        void OnAbility1(InputAction.CallbackContext context);
        void OnSkipMove(InputAction.CallbackContext context);
    }
    public interface IInGameMenuActions
    {
        void OnCancel(InputAction.CallbackContext context);
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
}
