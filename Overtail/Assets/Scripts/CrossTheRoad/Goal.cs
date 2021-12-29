using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        var rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        var bc = this.gameObject.AddComponent<BoxCollider2D>();
    }
}
