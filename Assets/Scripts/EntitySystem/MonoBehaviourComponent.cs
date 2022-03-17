using UnityEngine;

namespace EntitySystem
{
    public abstract class MonoBehaviourComponent : MonoBehaviour, IInitialize 
    {
        protected bool EnabledInternal => isActiveAndEnabled;
        public bool Destroyed { get; private set; }
        
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