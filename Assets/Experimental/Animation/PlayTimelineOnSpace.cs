using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace Experimental.Animation
{
    public class PlayTimelineOnSpace : MonoBehaviour
    {
        private ExperimentControls _controls;
        private PlayableDirector _director;
        
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
                PlayTimeline();
            }
        }
        
        private void PlayTimeline()
        {
            Debug.Log("[Experimental.Animation] Played timeline");
            _director.Play();
        }

        private void OnDestroy()
        {
            _controls.Standard.KbdSpace.performed -= HandleSpaceClicked;
        }
    }
}