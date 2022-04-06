using CardSystem;
using UnityEngine;

namespace InteractionSystem
{
    public class CardCollectable : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _uniqueId;
        [SerializeField] private Card _card;
        [SerializeField] private int _preferredSlot;
        
        // todo: to:billy use DI to inject card manager
        [SerializeField] private CardManager _cardManager;
        
        public int InteractableObjId => _uniqueId;
        public InteractionType Type => InteractionType.Collectable;
        public GameObject MyGameObject => gameObject;
        
        public void StartInteraction()
        {
            _cardManager.AcquireCard(_card, _preferredSlot);
            Destroy(gameObject);
        }

        public void EndInteraction()
        {
        }
    }
}