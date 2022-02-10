using System;
using UnityEngine;

namespace Unstable
{
    public class DamageablePartCollisionHandler : MonoBehaviour
    {
        [SerializeField] private DamageablePart _parent;
        
        public DamageablePart DamageablePart => _parent;
    }
}