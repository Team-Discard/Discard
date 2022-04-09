using Dialogue;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager Instance;
        private IInteractable _currentFocusedInteractable;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private InteractionHintDisplay _hintDisplay;

        public static Transform PlayerTransform => Instance._playerTransform;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _hintDisplay.Hide();
        }

        public void SetCurrentFocusedInteractable(IInteractable inter)
        {
            _currentFocusedInteractable = inter;
        }

        public void InteractWithCurrentFocusedInteractable()
        {
            if (_currentFocusedInteractable == null) return;

            // hide interaction hint
            _hintDisplay.Hide();

            // if interacting with a character, prepare the dialogue manager for dialogue
            if (_currentFocusedInteractable.Type == InteractionType.Character)
            {
                DialogueManager.Instance.StartDialogueWithCharacter(_currentFocusedInteractable);
            }

            _currentFocusedInteractable.StartInteraction();

            _currentFocusedInteractable = null;
        }

        public void DisplayInteractionHintIfNeeded()
        {
            if (_currentFocusedInteractable == null)
            {
                _hintDisplay.Hide();
                return;
            }

            var targetTransform = _currentFocusedInteractable.MyGameObject.transform;

            // display interaction hint if currently we have a focused interaction target
            if (null == Camera.main)
            {
                Debug.Log("No main camera found!");
                return;
            }

            //var screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
            //_hintDisplay.Rect.position = screenPoint;
            
            _hintDisplay.SetText(_currentFocusedInteractable.HintText);
            _hintDisplay.Show();
        }
    }
}