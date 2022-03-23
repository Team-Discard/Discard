using UnityEngine;

namespace InteractionSystem
{
    public class InteractableCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private int interactableObjId;

        public int InteractableObjId => interactableObjId;

        private void Start()
        {
            // subscribe to interaction related events
            InteractionEventSystem.SubscribeToOnStartInteraction(MyStartInteraction);
            InteractionEventSystem.SubscribeToOnEndInteraction(MyEndInteraction);
        }

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
