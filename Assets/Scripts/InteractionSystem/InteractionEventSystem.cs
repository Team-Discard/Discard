using System;
using UnityEngine;

namespace InteractionSystem
{
    public static class InteractionEventSystem
    {
        // bool variable that ONLY allow a single interaction with an interactable at a time
        // may change in the future
        public static bool IsInteracting { get; private set; } = false;

        // to:george to:billy (DO NOT REMOVE)
        // Events can be public, so you don't need methods that wrap around them.
        // Other classes would only be able to subscribe to them, not invoking them.
        private static event Action<int> OnStartInteraction;
        private static event Action<int> OnEndInteraction;
        
        // to:george to:billy (DO NOT REMOVE)
        // the following is necessary to initialize the static variable. Unity does not reload 
        // c# scripts when entering playmode, meaning that static constructors are not called.
        // You can enable that, but that would make entering play mode REALLY slow.
        // 
        // See: https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html and
        //      https://docs.unity3d.com/Manual/DomainReloading.html
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            IsInteracting = false;
            OnStartInteraction = null;
            OnEndInteraction = null;
        }
        
        public static void SubscribeToOnStartInteraction(Action<int> subscriber) => OnStartInteraction += subscriber;
        public static void UnSubscribeToOnStartInteraction(Action<int> subscriber) => OnStartInteraction -= subscriber;
        public static void TriggerOnStartInteraction(int interactableObjId, InteractionType type)
        {
            // if we are already interacting with something, do not trigger more interactions
            if (IsInteracting)
            {
                return;
            }
            
            // else set interacting to true and trigger interaction event
            IsInteracting = true;
            OnStartInteraction?.Invoke(interactableObjId);
        }

        // on end interaction event
        public static void SubscribeToOnEndInteraction(Action<int> subscriber) => OnEndInteraction += subscriber;
        public static void UnSubscribeToOnEndInteraction(Action<int> subscriber) => OnEndInteraction -= subscriber;

        public static void TriggerOnEndInteraction(int interactableObjId, InteractionType type)
        {
            IsInteracting = false;
            OnEndInteraction?.Invoke(interactableObjId);
        }
    }
}
