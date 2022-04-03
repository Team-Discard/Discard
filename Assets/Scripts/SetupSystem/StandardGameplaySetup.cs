using PlayerSystem;
using UnityEngine;
using Uxt.PropertyDrawers;

namespace SetupSystem
{
    public class StandardGameplaySetup : MonoBehaviour
    {
        [Header("Player Controls"), SerializeField]
        private Camera _playerControlCamera;

        [Header("Internal (Changes only allowed in prefab)"), SerializeField, EditInPrefabOnly]
        private StandardPlayer _player;

        private void Awake()
        {
            _player.BindControlCamera(_playerControlCamera.transform);
        }
    }
}