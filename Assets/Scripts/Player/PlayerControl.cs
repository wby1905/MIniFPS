//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Player/PlayerControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""fa5699fe-67ca-45bc-a74b-02bacaedd53d"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7a012dd2-e4f4-4096-86f3-7175cfd69b86"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""e1110ca8-0ff2-4f8a-b5ad-e6a8cf0b789b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwitchCam"",
                    ""type"": ""Button"",
                    ""id"": ""b1c77411-e2c3-411e-8e16-80c861428caf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""9dbf7653-50b2-4168-b817-e01f5724c7b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""b8f6077a-f980-4a55-bb79-3f27937615c7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwitchPrev"",
                    ""type"": ""Button"",
                    ""id"": ""7499bab6-8609-46bb-a7e3-3d55415028d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchNext"",
                    ""type"": ""Button"",
                    ""id"": ""5c438483-cc82-4d87-b9a7-287d6853c64b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""2dab4656-c248-4704-994b-43c238915510"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c681f372-ca6f-4ae2-96fd-53c13e30613a"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""859c98c7-743a-4871-bd05-24897f262273"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""56b1bdab-d90a-4de6-ac68-8730383da9bf"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8ba61c8e-7205-4fb8-baea-2aa22f28f57e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""7aab5edc-6af4-4df2-bbef-01720c4fd5a0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb91802c-8907-469a-9666-a139656739dd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bb878ebd-f845-4b5f-8318-46346490a81c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bf8df08f-016f-438b-84fc-58c49fd5c144"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5bed6012-9640-4804-87fb-98c5939cb105"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6264db65-33e2-44a6-9f16-5c5308602d81"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13ace845-ce78-4619-8f4e-5c5a10ce1542"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0032565-0e11-4eec-8320-6451e7a7fda4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""997bc540-4bed-4f73-8d2c-659b48b25168"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53e44e1f-9ef9-48d3-9c31-24eed9171a1a"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchPrev"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c904c24d-43c9-4804-84e8-e7f6e3d2700f"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchNext"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_Move = m_gameplay.FindAction("Move", throwIfNotFound: true);
        m_gameplay_Look = m_gameplay.FindAction("Look", throwIfNotFound: true);
        m_gameplay_SwitchCam = m_gameplay.FindAction("SwitchCam", throwIfNotFound: true);
        m_gameplay_Jump = m_gameplay.FindAction("Jump", throwIfNotFound: true);
        m_gameplay_Scroll = m_gameplay.FindAction("Scroll", throwIfNotFound: true);
        m_gameplay_SwitchPrev = m_gameplay.FindAction("SwitchPrev", throwIfNotFound: true);
        m_gameplay_SwitchNext = m_gameplay.FindAction("SwitchNext", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // gameplay
    private readonly InputActionMap m_gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_gameplay_Move;
    private readonly InputAction m_gameplay_Look;
    private readonly InputAction m_gameplay_SwitchCam;
    private readonly InputAction m_gameplay_Jump;
    private readonly InputAction m_gameplay_Scroll;
    private readonly InputAction m_gameplay_SwitchPrev;
    private readonly InputAction m_gameplay_SwitchNext;
    public struct GameplayActions
    {
        private @PlayerControl m_Wrapper;
        public GameplayActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_gameplay_Move;
        public InputAction @Look => m_Wrapper.m_gameplay_Look;
        public InputAction @SwitchCam => m_Wrapper.m_gameplay_SwitchCam;
        public InputAction @Jump => m_Wrapper.m_gameplay_Jump;
        public InputAction @Scroll => m_Wrapper.m_gameplay_Scroll;
        public InputAction @SwitchPrev => m_Wrapper.m_gameplay_SwitchPrev;
        public InputAction @SwitchNext => m_Wrapper.m_gameplay_SwitchNext;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @SwitchCam.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchCam;
                @SwitchCam.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchCam;
                @SwitchCam.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchCam;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Scroll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScroll;
                @SwitchPrev.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchPrev;
                @SwitchPrev.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchPrev;
                @SwitchPrev.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchPrev;
                @SwitchNext.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchNext;
                @SwitchNext.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchNext;
                @SwitchNext.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchNext;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @SwitchCam.started += instance.OnSwitchCam;
                @SwitchCam.performed += instance.OnSwitchCam;
                @SwitchCam.canceled += instance.OnSwitchCam;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @SwitchPrev.started += instance.OnSwitchPrev;
                @SwitchPrev.performed += instance.OnSwitchPrev;
                @SwitchPrev.canceled += instance.OnSwitchPrev;
                @SwitchNext.started += instance.OnSwitchNext;
                @SwitchNext.performed += instance.OnSwitchNext;
                @SwitchNext.canceled += instance.OnSwitchNext;
            }
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnSwitchCam(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnSwitchPrev(InputAction.CallbackContext context);
        void OnSwitchNext(InputAction.CallbackContext context);
    }
}
