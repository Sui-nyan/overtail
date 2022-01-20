using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Dialogue
{
    public class PlayerDialoguer : MonoBehaviour, IInteractor
    {

        [SerializeField] private DialogueManager dialogueManager;

        public DialogueManager DialogueManager => dialogueManager;

        [SerializeField] public IInteractable interactable;

        private void Awake()
        {
            dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        }

        private void Start()
        {
            // Subscribe to input
            InputManager.Instance.KeyConfirm += Talk;
        }
        
        private void Talk()
        {
            if (!DialogueManager.IsOpen && interactable is DialogueTrigger)
            {
                interactable?.Interact(this);
            }
        }
    }
}


