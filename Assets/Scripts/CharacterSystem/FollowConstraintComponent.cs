using EntitySystem;
using UnityEngine;

namespace CharacterSystem
{
    public class FollowConstraintComponent :
        StandardComponent, IComponent<FollowConstraintComponent>
    {
        private readonly Transform _follower;
        private readonly Transform _target;
        private readonly bool _isSmooth;
        public FollowConstraintComponent(IComponentRegistry registry, Transform follower, Transform target, bool isSmooth = true)
        {
            registry.AddComponent(this);
            
            _follower = follower;
            _target = target;
            _isSmooth = isSmooth;
        }

        public void Tick(float deltaTime)
        {
            if (_follower == null || _target == null)
            {
                Destroy();
                return;
            }
            
            if (_isSmooth)
            {
                _follower.position = Vector3.Lerp(_follower.position, _target.position, Time.deltaTime * 15.0f);
            }
            else
            {
                _follower.position = _target.position;
            }
        }
    }
}