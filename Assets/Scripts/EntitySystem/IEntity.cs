namespace EntitySystem
{
    public interface IEntity
    {
        public void AddTo(IComponentRegistry registry);
    }
}