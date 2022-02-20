using System;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace Experimental.Animation
{
    [AddComponentMenu("Experimental/" + nameof(PlayTimelineOnSpace) + " (Experimental)")]
    public class PlayTimelineOnSpace : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private AnimancerComponent _animancer;
        
        [SerializeField] private bool _useAnimancer;
        [SerializeField] private PlayableAssetTransition _playableAssetTransition;
        
        [SerializeField] private AvatarMask _upperBodyMask;
        
        private ExperimentControls _controls;

        private void Awake()
        {
            _controls = new ExperimentControls();
            _controls.Standard.Enable();
        }

        private void Start()
        {
            _controls.Standard.KbdSpace.performed += HandleSpaceClicked;
        }

        private void HandleSpaceClicked(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Invoke(nameof(PlayTimeline), 0.5f);
            }
        }
        
        private void PlayTimeline()
        {
            var note = _useAnimancer ? "using animancer" : "using playable director";
            Debug.Log($"[Experimental.Animation] Played timeline ({note})");
            if (_useAnimancer)
            {
                _animancer.Play(_playableAssetTransition);
            }
            else
            {
                _director.Play();
            }
        }

        private void OnDestroy()
        {
            _controls.Standard.KbdSpace.performed -= HandleSpaceClicked;
        }
    }
}