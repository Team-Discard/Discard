using Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace InteractionSystem
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager Instance;
        private IInteractable _currentFocusedInteractable;
        [SerializeField] private GameObject interactionHint;
        [SerializeField] private Transform _playerTransform;
        private RectTransform _interactionHintRectTransform;
        private Image _interactionHintImage;

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
            _interactionHintRectTransform = interactionHint.GetComponent<RectTransform>();
            _interactionHintImage = interactionHint.GetComponent<Image>();
        }

        public void SetCurrentFocusedInteractable(IInteractable inter)
        {
            _currentFocusedInteractable = inter;
        }

        public void InteractWithCurrentFocusedInteractable()
        {
            // hide interaction hint
            _interactionHintImage.enabled = false;
            
            // if interacting with a character, prepare the dialogue manager for dialogue
            if (_currentFocusedInteractable.Type == InteractionType.Character)
            {
                DialogueManager.Instance.StartDialogueWithCharacter(_currentFocusedInteractable);
            }
            
            _currentFocusedInteractable?.StartInteraction();
            
            _currentFocusedInteractable = null;
        }

        public void DisplayInteractionHintIfNeeded()
        {
            if (_currentFocusedInteractable == null)
            {
                _interactionHintImage.enabled = false;
                return;
            }

            var targetTransform = _currentFocusedInteractable.MyGameObject.transform;

            // display interaction hint if currently we have a focused interaction target
            if (null == Camera.main)
            {
                Debug.Log("No main camera found!");
                return;
            }

            var screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
            _interactionHintRectTransform.position = screenPoint;
            _interactionHintImage.enabled = true;
        }
    }
}