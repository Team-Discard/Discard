using System;
using UnityEngine;

namespace Unstable
{
    public class HurtBoxTrigger : MonoBehaviour
    {
        [SerializeField] private HurtBox _parent;
        
        public HurtBox HurtBox => _parent;
    }
}