using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Overtail.Items;
using Overtail.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugObj : MonoBehaviour
{
    private static DebugObj _instance;

    public List<Item> itemDb;

    void Awake()
    {
        MonoBehaviourExtension.MakeSingleton(this, ref _instance, destroyOnSceneZero: false);
        StartCoroutine(Repeat(5, () =>
        {
            if (itemDb.Count == 0) itemDb = ItemDatabase.GetAll() ?? null;
        }));
    }

    void Update()
    {
        for (int i = 282; i <= 293; i++)
        {
            if (Input.GetKeyDown((KeyCode) i)) SceneManager.LoadScene(i - 282);
        }
    }

    IEnumerator Repeat(float interval, Action f)
    {
        var t = new WaitForSeconds(interval);
        while (true)
        {
            f();
            yield return t;
        }
    }
}