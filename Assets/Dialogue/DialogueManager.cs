using System;
using InteractionSystem;
using UnityEngine;
using Yarn.Unity;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        
        [SerializeField] private DialogueRunner dialogueRunner;
        private IInteractable _currentFocusedInteractable;

        public void StartDialogueWithCharacter(IInteractable inter)
        {
            _currentFocusedInteractable = inter;
            dialogueRunner.StartDialogue("Gala.Start");
        }

        public void EndCurrentCharacterInteraction()
        {
            if (null != _currentFocusedInteractable)
            {
                InteractionEventSystem.TriggerOnEndInteraction(_currentFocusedInteractable.InteractableObjId, _currentFocusedInteractable.Type);
                _currentFocusedInteractable = null;
            }
        }

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
    }
}
