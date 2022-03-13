using CombatSystem;
using EntitySystem;
using UnityEngine;
using Unstable;
using Unstable.Entities;

namespace FlowControl
{
    public class GameFlow : MonoBehaviour, IComponentRegistry
    {
        [SerializeField] private PlayerMasterController _playerController;

        private GameObject _currentLevelRoot;
        private LevelFlow _currentLevelFlow;

        private void Start()
        {
            _playerController.AddTo(this);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (PlayerEnteredNewLevel(out var newLevelRoot))
            {
                // We would not handle the situation where the player switch levels, YET
                Debug.Assert(_currentLevelFlow == null);

                _currentLevelRoot = newLevelRoot;
                _currentLevelFlow = new LevelFlow(newLevelRoot);
            }

            _playerController.Tick(deltaTime);
            if (_currentLevelFlow != null)
            {
                _currentLevelFlow.Tick(deltaTime);
            }

            DamageManager.TickInvincibilityFrames(deltaTime);
            DamageManager.ResolveDamages();
        }

        private bool PlayerEnteredNewLevel(out GameObject levelRoot)
        {
            levelRoot = _currentLevelRoot;

            // Technically there could be more than one level in the queue
            // but for now we only really care about the latest one
            while (LevelEnterManager.TryDequeueEnterLevelEvent(out var enteredLevel))
            {
                levelRoot = enteredLevel;
            }

            return levelRoot != _currentLevelRoot;
        }
    }
}