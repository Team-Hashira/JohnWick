namespace Hashira.Entities.Interacts
{
    public interface IInteractable
    {
        public void Interaction(Entity entity);
        public void OnInteractable();
        public void OffInteractable();
    }
    public interface IHoldInteractable
    {
        public void HoldInteractionStart(Entity entity);
        public void HoldInteractionEnd();
    }
}
