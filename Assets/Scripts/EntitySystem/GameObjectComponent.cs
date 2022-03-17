using UnityEngine;

namespace EntitySystem
{
    [DisallowMultipleComponent]
    public class GameObjectComponent : MonoBehaviour, IInitialize
    {
        protected bool EnabledInternal => isActiveAndEnabled;
        public bool Destroyed { get; private set; }

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