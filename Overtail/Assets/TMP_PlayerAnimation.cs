using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMP_PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

    }
}
