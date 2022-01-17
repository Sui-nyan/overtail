using System;
using Overtail.PlayerModule;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
public class Tmp_PlayerSprite : MonoBehaviour
{
    private Sprite frontSprite;
    private Sprite backSprite;

    private PlayerMovement pm;
    private SpriteRenderer sr;

    void Awake()
    {
        frontSprite = Resources.Load<Sprite>("Placeholders/player_m_front");
        backSprite = Resources.Load<Sprite>("Placeholders/player_m_back");
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();

        if (backSprite is null || frontSprite is null || pm is null || sr is null)
        {
            Debug.LogError("<color=red>Failed to load setup</color>");
            throw new ArgumentNullException();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var facingAway = pm.direction.y > 0;
        var facingScreen = pm.direction.y < 0;

        if (facingAway) sr.sprite = backSprite;
        if (facingScreen) sr.sprite = frontSprite;
    }
}
