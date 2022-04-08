using UnityEngine;
using Yarn.Unity;

namespace PrototypeScripting
{
    // todo: to:billy Cleanse all class with PROTO prefix.
    public class PROTO_EnableBehaviourWhenVariableGeq : MonoBehaviour
    {
        [SerializeField] private string _variable;
        [SerializeField] private int _threshold;
        [SerializeField] private DialogueRunner _dialogueRunner;
        [SerializeField] private MonoBehaviour _targetComponent;

        private void Update()
        {
            if (_dialogueRunner.VariableStorage.TryGetValue(_variable, out float val) && val >= _threshold)
            {
                _targetComponent.enabled = true;
            }
            else
            {
                _targetComponent.enabled = false;
            }
        }
    }
}