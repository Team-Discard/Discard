using System.Collections.Generic;
using InteractionSystem;
using UnityEngine;
using Yarn.Unity;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        
        // look-up table for dialogue start nodes
        [SerializeField] private List<CharacterDialogueEntry> characterDialogueEntries;
        private Dictionary<string, string> _characterNameToDialogueStartNode;

        [SerializeField] private DialogueRunner dialogueRunner;
        private IInteractable _currentFocusedInteractable;


        public void StartDialogueWithCharacter(IInteractable inter)
        {
            _currentFocusedInteractable = inter;

            var charName = inter.MyGameObject.GetComponent<InteractableCharacter>().CharacterName;
            
            dialogueRunner.StartDialogue(_characterNameToDialogueStartNode[charName]);
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
                
                // fill the dialogue look-up table
                _characterNameToDialogueStartNode = new Dictionary<string, string>();
                foreach (var pair in characterDialogueEntries)
                {
                    _characterNameToDialogueStartNode[pair.characterName] = pair.dialogueStartNode;
                }
            }
        }

        /*------------------------------- Below are example API for YarnSpinner ----------------------------------*/
        // Yarn can take string, int, float, bool, GameObject, Component as function arguments if needed
    
        // static function example: static function can be on any class, not necessarily a mono-behavior
        // syntax in Yarn script: <<static_example "argument string">>
        
        [YarnCommand("static_example")]
        public static void YarnSpinnerExampleStaticFunc(string arg)
        {
            Debug.Log(arg + ": YarnSpinner called static API example function");
        }
        
        [YarnCommand("non_static_example")]
        // non-static function example: MUST be on a mono-behavior
        // syntax in Yarn script: <<non_static_example DialogueManager "argument string">>
        // the name of the GameObject that this mono-behavior is attached to MUST NOT have any spaces in its name
        
        // todo:billy change everything to use underscore naming convention
        public void YarnSpinnerExampleFunc(string arg)
        {
            Debug.Log(arg + ": YarnSpinner called non-static API example function");
        }
    }
}
