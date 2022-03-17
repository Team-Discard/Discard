namespace EntitySystem
{
    public class StandardComponent
    {
        public bool Destroyed { get; private set; } = false;
        public void Destroy()
        {
            Destroyed = true;
        }
    }
}