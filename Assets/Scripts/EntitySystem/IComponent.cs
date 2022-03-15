namespace EntitySystem
{
    public interface IComponent
    {
        public IEntity Entity { get; }
        public sealed bool Destroyed => Entity.Destroyed;
    }
}