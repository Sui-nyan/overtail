using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class Class1 : MonoBehaviour
{
    void Awake()
    {
        string originalText = "{angry} I don't like this";

        Regex regex = new Regex(@"{[A-Za-z]*}");
        string matches = regex.Match(originalText).Value;
        Debug.Log(matches == "");

        string dialogue = originalText.Replace(matches, "").Trim();
        Debug.Log($"Clean: {dialogue}");

        string emotion = matches.Replace("{", "").Replace("}", "");
        Debug.Log($"Emotion : {emotion}");
    }
}