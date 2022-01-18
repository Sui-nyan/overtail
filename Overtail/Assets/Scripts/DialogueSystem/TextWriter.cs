using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Overtail.Dialogue
{

    public class TextWriter : MonoBehaviour
    {
        [SerializeField] private float delayTime = 0.01f;

        public Coroutine Run(string textToType, TMP_Text textLabel)
        {
            return StartCoroutine(TypeText(textToType, textLabel));
        }

        /*
         * Text typed in a typewriter kind of effect letters appear one after each other
         */
        private IEnumerator TypeText(string textToType, TMP_Text textLabel)
        {
            textLabel.text = string.Empty;

            for (int i = 0; i < textToType.Length; i++)
            {
                textLabel.text = textToType.Substring(0, i + 1);
                textLabel.text += "<color=#00000000>" + textToType.Substring(i + 1) + "</color>";
                yield return new WaitForSeconds(delayTime);
            }
        }
    }

}

