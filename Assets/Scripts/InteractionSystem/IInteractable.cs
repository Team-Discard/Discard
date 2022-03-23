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
        public int InteractionPriority { get; }
        public InteractionType Type { get; }
        public void StartInteraction();
        public void EndInteraction();
    }
}
