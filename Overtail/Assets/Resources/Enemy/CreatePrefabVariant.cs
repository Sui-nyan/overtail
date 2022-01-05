using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatePrefabVariant : MonoBehaviour
{
    [MenuItem("Cr/Enemy")]
    static void CreateEnemyVariant()
    {
        var src = Resources.Load("Enemy/BasicEnemy");
        Debug.Log(src);
    }
}
