namespace InteractionSystem
{
    public interface IInteractable
    {
        public int InteractableObjId { get; }
        public void StartInteraction();
        public void EndInteraction();
    }
}
