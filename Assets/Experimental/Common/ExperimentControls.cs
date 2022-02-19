//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Experimental/Common/ExperimentControls.inputactions
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

public partial class @ExperimentControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ExperimentControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ExperimentControls"",
    ""maps"": [
        {
            ""name"": ""Standard"",
            ""id"": ""d2ee1399-c3ca-421b-996e-f891128a0fce"",
            ""actions"": [
                {
                    ""name"": ""KbdSpace"",
                    ""type"": ""Button"",
                    ""id"": ""94d274b0-2e7d-434b-abd5-e8b6ea833640"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e4d28842-5a45-4b06-968e-dad1e33878f6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KbdSpace"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Standard
        m_Standard = asset.FindActionMap("Standard", throwIfNotFound: true);
        m_Standard_KbdSpace = m_Standard.FindAction("KbdSpace", throwIfNotFound: true);
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

    // Standard
    private readonly InputActionMap m_Standard;
    private IStandardActions m_StandardActionsCallbackInterface;
    private readonly InputAction m_Standard_KbdSpace;
    public struct StandardActions
    {
        private @ExperimentControls m_Wrapper;
        public StandardActions(@ExperimentControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @KbdSpace => m_Wrapper.m_Standard_KbdSpace;
        public InputActionMap Get() { return m_Wrapper.m_Standard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(StandardActions set) { return set.Get(); }
        public void SetCallbacks(IStandardActions instance)
        {
            if (m_Wrapper.m_StandardActionsCallbackInterface != null)
            {
                @KbdSpace.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnKbdSpace;
                @KbdSpace.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnKbdSpace;
                @KbdSpace.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnKbdSpace;
            }
            m_Wrapper.m_StandardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @KbdSpace.started += instance.OnKbdSpace;
                @KbdSpace.performed += instance.OnKbdSpace;
                @KbdSpace.canceled += instance.OnKbdSpace;
            }
        }
    }
    public StandardActions @Standard => new StandardActions(this);
    public interface IStandardActions
    {
        void OnKbdSpace(InputAction.CallbackContext context);
    }
}
