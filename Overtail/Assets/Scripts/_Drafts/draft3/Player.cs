using System.Collections;
using UnityEngine;

namespace Draft3.Unityless
{
    [RequireComponent(typeof(CombatStatComponent))]
    public class Player : MonoBehaviour
    {
        CombatStatComponent stats;
        private void Start()
        {
           //test
            stats = gameObject.GetComponent<CombatStatComponent>();
        }
    }
}