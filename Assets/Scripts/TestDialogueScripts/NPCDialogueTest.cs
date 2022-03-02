using UnityEngine;
using Unstable.Entities;
using Yarn.Unity;

namespace TestDialogueScripts
{
    public class NPCDialogueTest : MonoBehaviour
    {
        [SerializeField] private DialogueRunner dialogueRunner;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Cursor.lockState = CursorLockMode.None;
            ThirdPersonOrbitalCameraInputHandler.IsCameraMovable = false;

            dialogueRunner.StartDialogue("Gala.Start");
        }

        public void OnDialogueComplete()
        {
            Cursor.lockState = CursorLockMode.Locked;
            ThirdPersonOrbitalCameraInputHandler.IsCameraMovable = true;
        }
    }
}
