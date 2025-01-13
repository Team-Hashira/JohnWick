namespace Hashira.Entities.Interacts
{
    public interface IInteractable
    {
        public void Interaction(Entity entity);
        public void OnInteractable();
        public void OffInteractable();
    }
}
