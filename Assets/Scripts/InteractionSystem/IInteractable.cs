using UnityEngine;

namespace InteractionSystem
{
    public enum InteractionType
    {
        Character,
        Collectable
    }
    
    public interface IInteractable
    {
        public int InteractableObjId { get; }
        public int InteractionPriority => 0;
        public InteractionType Type { get; }
        public GameObject MyGameObject { get; }
        public string HintText => "Interact";
        public void StartInteraction();
        public void EndInteraction();
    }
}
