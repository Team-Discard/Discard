using System;
using Dialogue;
using UnityEngine;
using Yarn.Unity;

namespace PrototypeScripting
{
    public class PROTO_IncrementVariableOnDestroy : MonoBehaviour
    {
        [SerializeField] private string _variable;
        [SerializeField] private DialogueRunner _dialogueRunner;

        private void OnDestroy()
        {
            _dialogueRunner.VariableStorage.IncrementFloat(_variable);
        }
    }
}