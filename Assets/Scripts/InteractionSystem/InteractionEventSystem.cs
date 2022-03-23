using System;

namespace InteractionSystem
{
    public static class InteractionEventSystem
    {
        // bool variable that ONLY allow a single interaction with an interactable at a time
        private static bool _IsInteracting = false;
        
        // on start interaction event
        private static event Action<int> OnStartInteraction;
        public static void SubscribeToOnStartInteraction(Action<int> subscriber) => OnStartInteraction += subscriber;
        public static void UnSubscribeToOnStartInteraction(Action<int> subscriber) => OnStartInteraction -= subscriber;
        public static void TriggerOnStartInteraction(int interactableObjId)
        {
            // if we are already interacting with something, do not trigger more interactions
            if (_IsInteracting)
            {
                return;
            }
            
            // else set interacting to true and trigger interaction event
            _IsInteracting = true;
            OnStartInteraction?.Invoke(interactableObjId);
        }

        // on end interaction event
        private static event Action<int> OnEndInteraction;
        public static void SubscribeToOnEndInteraction(Action<int> subscriber) => OnEndInteraction += subscriber;
        public static void UnSubscribeToOnEndInteraction(Action<int> subscriber) => OnEndInteraction -= subscriber;

        public static void TriggerOnEndInteraction(int interactableObjId)
        {
            _IsInteracting = false;
            OnEndInteraction?.Invoke(interactableObjId);
        }
    }
}
