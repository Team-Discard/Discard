using UnityEngine;

namespace EntitySystem
{
    public class StandardComponent<T> where T : IComponent
    {
        public StandardComponent()
        {
            Debug.Assert(typeof(T) == GetType());
        }
        
        public bool Destroyed { get; private set; } = false;
        public void Destroy()
        {
            Destroyed = true;
        }
    }
}