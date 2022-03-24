using System.Collections;
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
        
        // not used yet, having this in case we want to do something about it
        public InteractionType Type => interactionType;

        public GameObject MyGameObject => gameObject;

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
            InteractionEventSystem.TriggerOnStartInteraction(interactableObjId, Type);
        }
        
        public void EndInteraction()
        {
            InteractionEventSystem.TriggerOnEndInteraction(interactableObjId, Type);
        }
        
        // in case we want to execute a coroutine for interaction, not used now
        private IEnumerator ExecuteInteractionCoroutine(int id)
        {
            if (interactableObjId != id)
            {
                yield return null;
            }
            
            yield return null;
        }
        
        // force stop all running coroutines on this script when interaction ends, now used now
        private void ForceStopAllCoroutines()
        {
            StopAllCoroutines();
        }

        private void MyStartInteraction(int id)
        {
            // if interactable object called on is not myself, do nothing
            if (interactableObjId != id)
            {
                return;
            }
        }

        private void MyEndInteraction(int id)
        {
            // if interactable object called on is not myself, do nothing
            if (interactableObjId != id)
            {
                return;
            }
        }
    }
}
