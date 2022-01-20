using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IgnoreCollisionTest : MonoBehaviour
{

    [SerializeField] private GameObject[] ignoredColliders;

    // Just add this to any attached Monobehaviour script
    // OnCollisionEnter2D() with Physics2D.IgnoreCollision() inside
    void OnCollisionEnter2D(Collision2D col)
    {
        if (ignoredColliders.ToList().Contains(col.gameObject))
        {
            Debug.Log("Ignored");
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
    }
}
