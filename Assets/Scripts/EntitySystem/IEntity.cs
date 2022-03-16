using UnityEngine;

namespace EntitySystem
{
    public interface IEntity
    {
        public void AddTo(IComponentRegistry registry);
        public bool Destroyed { get; }
        public void Destroy();
    }
}