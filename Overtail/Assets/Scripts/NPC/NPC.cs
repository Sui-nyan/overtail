using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : IInteractable
{
    [SerializeField] string name;
    [SerializeField] Sprite portrait;
    [SerializeField] Sprite Sprite;

    public void movementPattern()
    {

    }

    public void Intectact(Player player)
    {
        
    }
}
