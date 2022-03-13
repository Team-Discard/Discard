using System.Collections.Generic;
using UnityEngine;

namespace FlowControl
{
    public static class LevelEnterManager
    {
        private static Queue<GameObject> _playerEnterLevelEvents;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _playerEnterLevelEvents = new Queue<GameObject>();
        }

        public static void NotifyPlayerEntersLevel(GameObject levelRoot)
        {
            _playerEnterLevelEvents.Enqueue(levelRoot);
        }

        public static bool TryDequeueEnterLevelEvent(out GameObject enteredLevel)
        {
            if (_playerEnterLevelEvents.Count == 0)
            {
                enteredLevel = default;
                return false;
            }

            enteredLevel = _playerEnterLevelEvents.Dequeue();
            return true;
        }
    }
}