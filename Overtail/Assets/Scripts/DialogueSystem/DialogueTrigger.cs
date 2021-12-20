using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{

    [SerializeField] DialogueObject dialogueObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.TryGetComponent(out Player player))
        {
            Debug.Log("Player in Range");
            player.interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out Player player))
        {
            Debug.Log("Player out of Range");
            if(player.interactable is DialogueTrigger dialogueActivatior && dialogueActivatior == this)
            {
                player.interactable = null;
            }
        }
    }

    public void Intectact(Player player)
    {
        player.DialogueManager.StartDialogue(dialogueObject);
    }
}
