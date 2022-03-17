using UnityEngine;

namespace EntitySystem
{
    public abstract class MonoBehaviourComponent<T> : MonoBehaviour, IInitialize 
        where T : IComponent
    {
        protected bool EnabledInternal => isActiveAndEnabled;
        public bool Destroyed { get; private set; }

        public MonoBehaviourComponent()
        {
            Debug.Assert(typeof(T) == GetType());
        }

        public virtual void Init()
        {
            Destroyed = false;
        }

        public void Destroy()
        {
            Destroy(this);
            Destroyed = true;
        }

        protected void OnDestroy()
        {
            Destroyed = true;
        }
    }
}