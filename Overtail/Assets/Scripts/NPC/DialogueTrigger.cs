using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{

    [SerializeField] DialogueObject dialogueObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMove player))
        {
            player.interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMove player))
        {
            if(player.interactable is DialogueTrigger dialogueActivatior && dialogueActivatior == this)
            {
                player.interactable = null;
            }
        }
    }

    public void Intectact(PlayerMove player)
    {
        player.DialogueManager.StartDialogue(dialogueObject);
    }
}
