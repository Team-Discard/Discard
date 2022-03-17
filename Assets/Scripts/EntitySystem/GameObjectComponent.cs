using UnityEngine;

namespace EntitySystem
{
    [DisallowMultipleComponent]
    public class GameObjectComponent<T> : MonoBehaviour, IInitialize
        where T : IComponent
    {
        protected bool EnabledInternal => isActiveAndEnabled;
        public bool Destroyed { get; private set; }

        public GameObjectComponent()
        {
            Debug.Assert(typeof(T) == GetType(), "typeof(T) == GetType()");
        }

        public virtual void Init()
        {
            Destroyed = false;
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
            Destroyed = true;
        }

        protected virtual void OnDestroy()
        {
            Destroyed = true;
        }
    }
}