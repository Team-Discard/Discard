using System.Collections.Generic;
using GameRuleSystem;
using InteractionSystem;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Dialogue
{
    // todo: to:george we need a way to disallow interaction with a character based on a variable in yarn.
    //       don't make it sophisticated though because we don't know how characters will remember things
    //       between multiple play-through yet
    
    
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        
        // look-up table for dialogue start nodes
        [SerializeField] private List<CharacterDialogueEntry> characterDialogueEntries;
        private Dictionary<string, string> _characterNameToDialogueStartNode;

        [SerializeField] private DialogueRunner dialogueRunner;
        private IInteractable _currentFocusedInteractable;

        [SerializeField] private Button continueButton;


        public void StartDialogueWithCharacter(IInteractable inter)
        {
            _currentFocusedInteractable = inter;

            var charName = inter.MyGameObject.GetComponent<InteractableCharacter>().CharacterName;
            
            dialogueRunner.StartDialogue(_characterNameToDialogueStartNode[charName]);
            
            GameRuleManager.EnforceRule(GameRule.NoHUD, this);
        }

        public void EndCurrentCharacterInteraction()
        {
            if (null != _currentFocusedInteractable)
            {
                InteractionEventSystem.TriggerOnEndInteraction(_currentFocusedInteractable.InteractableObjId, _currentFocusedInteractable.Type);
                _currentFocusedInteractable = null;
            }
            
            GameRuleManager.RevokeRule(GameRule.NoHUD, this);
        }

        public void TryPressContinue()
        {
            if (continueButton.enabled)
            {
                continueButton.onClick?.Invoke();
            }
        }

        private void OnDestroy()
        {
            // todo: to:billy use the IDisposable pattern here
            // todo: we also need a way to disable camera rotation during rotation

            if (GameRuleManager.IsRuleEnforcedBy(GameRule.NoHUD, this))
            {
                GameRuleManager.RevokeRule(GameRule.NoHUD, this);
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
        
        // todo: to:billy change everything to use underscore naming convention
        public void YarnSpinnerExampleFunc(string arg)
        {
            Debug.Log(arg + ": YarnSpinner called non-static API example function");
        }
        
        // todo: to:billy this is very hacky because
        // a) it depends on finding a gameObject.
        // b) it doesn't work well with a save system.
        // c) There must a way to store state about whether certain conditions are met and show/hide UI based on that,
        //    instead of relying on a one-time destruction
        [YarnCommand("destroy_object"), UsedImplicitly]
        public static void DestroyObject(GameObject gameObject)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

    }
}
