using System;

namespace InteractionSystem
{
    public static class InteractionEventSystem
    {
        // bool variable that ONLY allow a single interaction with an interactable at a time
        // may change in the future
        public static bool IsInteracting { get; private set; } = false;

        // on start interaction event
        private static event Action<int> OnStartInteraction;
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
        private static event Action<int> OnEndInteraction;
        public static void SubscribeToOnEndInteraction(Action<int> subscriber) => OnEndInteraction += subscriber;
        public static void UnSubscribeToOnEndInteraction(Action<int> subscriber) => OnEndInteraction -= subscriber;

        public static void TriggerOnEndInteraction(int interactableObjId, InteractionType type)
        {
            IsInteracting = false;
            OnEndInteraction?.Invoke(interactableObjId);
        }
    }
}
