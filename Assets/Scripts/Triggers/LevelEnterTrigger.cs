using FlowControl;
using UnityEngine;

namespace Triggers
{
    public class LevelEnterTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _levelRoot;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelEnterManager.NotifyPlayerEntersLevel(_levelRoot);
            }
        }
    }
}