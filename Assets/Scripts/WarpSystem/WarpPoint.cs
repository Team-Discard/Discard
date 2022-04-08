using InteractionSystem;
using PlayerSystem;
using UnityEngine;
using Uxt.Utils;

namespace WarpSystem
{
    public class WarpPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _warpTarget;
        
        public int InteractableObjId => -1;
        public InteractionType Type => InteractionType.Collectable;
        public GameObject MyGameObject => gameObject;

        public void StartInteraction()
        {
            var targetTransform = _warpTarget.ExtractRigidTransformData();
            StandardPlayer.Instance.Pawn.Warp(targetTransform);
        }

        public void EndInteraction()
        {
        }
    }
}