using JetBrains.Annotations;
using UnityEngine;

namespace PrototypeScripting
{
    public class PROTO_SetChildrenActiveWhenLevelCompleted : MonoBehaviour
    {
        [UsedImplicitly]
        public void OnLevelCompleted()
        {
            foreach (var t in transform.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.SetActive(true);
            }
        }
    }
}