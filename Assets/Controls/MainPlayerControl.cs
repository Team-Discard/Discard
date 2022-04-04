//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Controls/MainPlayerControl.inputactions
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

public partial class @MainPlayerControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainPlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainPlayerControl"",
    ""maps"": [
        {
            ""name"": ""Standard"",
            ""id"": ""7b76e420-e7ea-4bab-b3bf-ca6aece4c047"",
            ""actions"": [
                {
                    ""name"": ""Locomotion"",
                    ""type"": ""Value"",
                    ""id"": ""971d02d4-2a66-4c67-bcd9-cd4f8e062635"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Camera Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""015448ed-9893-4aa9-bc3a-9e54c5d48711"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Toggle Lock On"",
                    ""type"": ""Button"",
                    ""id"": ""7f659f9d-e845-4e68-b20d-af93c3d3d5d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""3c9fd7ef-adc1-45fc-8e20-5d8c6a218a33"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Card South"",
                    ""type"": ""Button"",
                    ""id"": ""69cf1c0a-8918-4940-a589-e85473c794e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Card East"",
                    ""type"": ""Button"",
                    ""id"": ""ded331da-9f50-4d28-9593-794e63105c17"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Card North"",
                    ""type"": ""Button"",
                    ""id"": ""0a9b5e8b-4f6b-460f-ac76-33e220b06f53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Card West"",
                    ""type"": ""Button"",
                    ""id"": ""130e126e-2fd5-40d5-899e-20d4361e34a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""d67fcd6b-6093-467e-b671-2f00460756d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""48c72741-423e-4b76-91f8-5e5460e7aa9a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""181549ad-0101-4e89-bea6-2c949509fe8e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""73d9b969-2827-40df-96aa-e7367db69278"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""57972c29-dea2-4172-897d-3a063b55c0ba"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c59d03a8-92d6-4aa9-be98-af297ed24dfd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""db371e9b-e30a-43d4-bade-d19020fed91f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Locomotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f59cb5dc-d31d-481a-9c26-41cbb0a87a3b"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d03fc1ad-4497-48d1-bd82-14c62ef944af"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=15,y=15)"",
                    ""groups"": """",
                    ""action"": ""Camera Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83fb6976-3f5f-40b9-94c3-4583a96d26c3"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbc6ab79-b3cb-4414-af2c-307bbe3b95f7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Card South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""779b09e5-b258-4601-9798-1525ac76f8c8"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Card East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6178facd-c0d7-49c8-bef5-5d9e55e7e7d8"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Card North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e041489a-726a-42c0-9981-c13e68261484"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Card West"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""217fd9df-a435-4e46-b9c8-2f8fe5e7887b"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Lock On"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca33aae5-39f8-423c-9239-719393242f29"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
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
        m_Standard_Locomotion = m_Standard.FindAction("Locomotion", throwIfNotFound: true);
        m_Standard_CameraRotate = m_Standard.FindAction("Camera Rotate", throwIfNotFound: true);
        m_Standard_ToggleLockOn = m_Standard.FindAction("Toggle Lock On", throwIfNotFound: true);
        m_Standard_Roll = m_Standard.FindAction("Roll", throwIfNotFound: true);
        m_Standard_CardSouth = m_Standard.FindAction("Card South", throwIfNotFound: true);
        m_Standard_CardEast = m_Standard.FindAction("Card East", throwIfNotFound: true);
        m_Standard_CardNorth = m_Standard.FindAction("Card North", throwIfNotFound: true);
        m_Standard_CardWest = m_Standard.FindAction("Card West", throwIfNotFound: true);
        m_Standard_Interact = m_Standard.FindAction("Interact", throwIfNotFound: true);
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
    private readonly InputAction m_Standard_Locomotion;
    private readonly InputAction m_Standard_CameraRotate;
    private readonly InputAction m_Standard_ToggleLockOn;
    private readonly InputAction m_Standard_Roll;
    private readonly InputAction m_Standard_CardSouth;
    private readonly InputAction m_Standard_CardEast;
    private readonly InputAction m_Standard_CardNorth;
    private readonly InputAction m_Standard_CardWest;
    private readonly InputAction m_Standard_Interact;
    public struct StandardActions
    {
        private @MainPlayerControl m_Wrapper;
        public StandardActions(@MainPlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Locomotion => m_Wrapper.m_Standard_Locomotion;
        public InputAction @CameraRotate => m_Wrapper.m_Standard_CameraRotate;
        public InputAction @ToggleLockOn => m_Wrapper.m_Standard_ToggleLockOn;
        public InputAction @Roll => m_Wrapper.m_Standard_Roll;
        public InputAction @CardSouth => m_Wrapper.m_Standard_CardSouth;
        public InputAction @CardEast => m_Wrapper.m_Standard_CardEast;
        public InputAction @CardNorth => m_Wrapper.m_Standard_CardNorth;
        public InputAction @CardWest => m_Wrapper.m_Standard_CardWest;
        public InputAction @Interact => m_Wrapper.m_Standard_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Standard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(StandardActions set) { return set.Get(); }
        public void SetCallbacks(IStandardActions instance)
        {
            if (m_Wrapper.m_StandardActionsCallbackInterface != null)
            {
                @Locomotion.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnLocomotion;
                @Locomotion.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnLocomotion;
                @Locomotion.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnLocomotion;
                @CameraRotate.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnCameraRotate;
                @CameraRotate.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnCameraRotate;
                @CameraRotate.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnCameraRotate;
                @ToggleLockOn.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnToggleLockOn;
                @ToggleLockOn.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnToggleLockOn;
                @ToggleLockOn.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnToggleLockOn;
                @Roll.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnRoll;
                @CardSouth.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardSouth;
                @CardSouth.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardSouth;
                @CardSouth.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardSouth;
                @CardEast.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardEast;
                @CardEast.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardEast;
                @CardEast.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardEast;
                @CardNorth.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardNorth;
                @CardNorth.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardNorth;
                @CardNorth.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardNorth;
                @CardWest.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardWest;
                @CardWest.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardWest;
                @CardWest.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnCardWest;
                @Interact.started -= m_Wrapper.m_StandardActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_StandardActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_StandardActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_StandardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Locomotion.started += instance.OnLocomotion;
                @Locomotion.performed += instance.OnLocomotion;
                @Locomotion.canceled += instance.OnLocomotion;
                @CameraRotate.started += instance.OnCameraRotate;
                @CameraRotate.performed += instance.OnCameraRotate;
                @CameraRotate.canceled += instance.OnCameraRotate;
                @ToggleLockOn.started += instance.OnToggleLockOn;
                @ToggleLockOn.performed += instance.OnToggleLockOn;
                @ToggleLockOn.canceled += instance.OnToggleLockOn;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @CardSouth.started += instance.OnCardSouth;
                @CardSouth.performed += instance.OnCardSouth;
                @CardSouth.canceled += instance.OnCardSouth;
                @CardEast.started += instance.OnCardEast;
                @CardEast.performed += instance.OnCardEast;
                @CardEast.canceled += instance.OnCardEast;
                @CardNorth.started += instance.OnCardNorth;
                @CardNorth.performed += instance.OnCardNorth;
                @CardNorth.canceled += instance.OnCardNorth;
                @CardWest.started += instance.OnCardWest;
                @CardWest.performed += instance.OnCardWest;
                @CardWest.canceled += instance.OnCardWest;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public StandardActions @Standard => new StandardActions(this);
    public interface IStandardActions
    {
        void OnLocomotion(InputAction.CallbackContext context);
        void OnCameraRotate(InputAction.CallbackContext context);
        void OnToggleLockOn(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnCardSouth(InputAction.CallbackContext context);
        void OnCardEast(InputAction.CallbackContext context);
        void OnCardNorth(InputAction.CallbackContext context);
        void OnCardWest(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
