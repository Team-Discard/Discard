using System;
using Animancer;
using UnityEngine;

namespace Experimental.Animation
{
    [SelectionBase]
    [AddComponentMenu("Experimental/" + nameof(PlayDefaultAnimation) + " (Experimental)")]
    public class PlayDefaultAnimation : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _defaultClip;
        
        private void Start()
        {
            _animancer.Play(_defaultClip);
        }
    }
}