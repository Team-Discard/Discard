using UnityEngine;

namespace InteractionSystem
{
    public class InteractableCharacter : MonoBehaviour, IInteractable
    {
        // for now, set these in inspector
        [SerializeField] private int interactableObjId;
        [SerializeField] private int interactionPriority;
        [SerializeField] private InteractionType interactionType;

        public int InteractableObjId => interactableObjId;
        public int InteractionPriority => interactionPriority;
        public InteractionType Type => interactionType;

        private void OnEnable()
        {
            // subscribe to interaction related events
            InteractionEventSystem.SubscribeToOnStartInteraction(MyStartInteraction);
            InteractionEventSystem.SubscribeToOnEndInteraction(MyEndInteraction);
        }

        private void OnDisable()
        {
            // unsubscribe to interaction related events
            InteractionEventSystem.UnSubscribeToOnStartInteraction(MyStartInteraction);
            InteractionEventSystem.UnSubscribeToOnEndInteraction(MyEndInteraction);
        }

        public void StartInteraction()
        {
            InteractionEventSystem.TriggerOnStartInteraction(interactableObjId);
        }
        
        public void EndInteraction()
        {
            InteractionEventSystem.TriggerOnEndInteraction(interactableObjId);
        }

        private void MyStartInteraction(int id)
        {
            // if object called on is not myself, do nothing
            if (interactableObjId != id)
            {
                return;
            }
        }

        private void MyEndInteraction(int id)
        {
            // if object called on is not myself, do nothing
            if (interactableObjId != id)
            {
                return;
            }
        }
    }
}
