namespace EntitySystem
{
    public class StandardComponent
    {
        public bool Destroyed { get; private set; } = false;
        public virtual void Destroy()
        {
            Destroyed = true;
        }
    }
}