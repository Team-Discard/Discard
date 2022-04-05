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

        [SerializeField] private string characterName;

        private const float RotationSpeed = 1f;

        public int InteractableObjId => interactableObjId;
        public int InteractionPriority => interactionPriority;
        
        // not used yet, having this in case we want to do something about it
        public InteractionType Type => interactionType;

        public GameObject MyGameObject => gameObject;

        public string CharacterName => characterName;

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

        private Coroutine _lookAtPlayerCoroutine;

        private void StartRotation()
        {
            if (null != _lookAtPlayerCoroutine)
            {
                StopCoroutine(_lookAtPlayerCoroutine);
            }

            _lookAtPlayerCoroutine = StartCoroutine(LookAtPlayer());
        }
        private IEnumerator LookAtPlayer()
        {
            var playerPosition = InteractionManager.PlayerTransform.position;
            var position = transform.position;
            
            // clamp the y coordinate
            playerPosition.y = position.y;
            
            var lookRotation = Quaternion.LookRotation(playerPosition - position);
            var timeCount = 0f;
            while (timeCount < 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, timeCount);
                timeCount += RotationSpeed * Time.deltaTime;
                yield return null;
            }
        }

        private void MyStartInteraction(int id)
        {
            // if interactable object called on is not myself, do nothing
            if (interactableObjId != id)
            {
                return;
            }
            
            // rotate character towards player
            StartRotation();
        }
        
        // always called when dialogue is over
        private void MyEndInteraction(int id)
        {
            // if interactable object called on is not myself, do nothing
            if (interactableObjId != id)
            {
                return;
            }
            
            Debug.Log("Interaction with character: " + gameObject.name + " has ended");
        }
    }
}
