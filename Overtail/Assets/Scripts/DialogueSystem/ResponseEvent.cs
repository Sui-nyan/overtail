using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Overtail.Dialogue
{
    [System.Serializable]
    public class ResponseEvent
    {
        [HideInInspector] public string name;
        [SerializeField] public UnityEvent onPickedResponse;

        public UnityEvent OnPickedResponse => onPickedResponse;
    }

}
