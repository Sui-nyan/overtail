using System.Collections;
using System.Collections.Generic;
using Overtail;
using Overtail.PlayerModule;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{

    public string SceneName;
    public Vector2 position;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("COLLISION");
        if (collider.gameObject.TryGetComponent<Player>(out var player))
        {
            Debug.Log("- with player");
            StartCoroutine(TeleportPlayer(player));
        }
    }

    IEnumerator TeleportPlayer(Player p)
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(SceneName);
        p.transform.position = position;

        yield return null;
        Destroy(this.gameObject);
    }
}
