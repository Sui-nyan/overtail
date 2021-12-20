using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Serialization example
/// </summary>
public class mainSerializationTest : MonoBehaviour
{
    public DialogueObject SaveThisToFile;
    public DialogueObject LoadFromFile;

    private void Awake()
    {
        // Cannot (Should not) save to resource folder during runtime btw
        SaveTo(@"./.tempAssets/testDia.json");

        //Load text from a JSON file (Assets/Resources/testDia.json)
        LoadFrom("testDia");
    }

    private void SaveTo(string filepath)
    {
        // Create directory if does not extist yet
        string directory = Path.GetDirectoryName(filepath);
        Directory.CreateDirectory(directory);
        Debug.Log($"Asset can be found in {directory}");

        string jsonString = JsonUtility.ToJson(SaveThisToFile);
        Debug.Log($"Output json: {jsonString}");

        System.IO.File.WriteAllText(filepath, jsonString);
        Debug.Log($"Saved {filepath}");
    }

    private void LoadFrom(string fileName)
    {
        //Load text from a JSON file (Assets/Resources/testDia.json)
        TextAsset jsonTextFile = Resources.Load<TextAsset>(fileName);


        // Creates new instance of that class with json data
        // doesnt work with scriptable objects
        // use FromJsonOverwrite() instead
        // newObj = JsonUtility.FromJson<DialogueObject>(jsonTextFile.ToString());


        // Use this line if you dont want to overwrite original scriptableObject
        LoadFromFile = ScriptableObject.CreateInstance(typeof(DialogueObject)) as DialogueObject;
        
        JsonUtility.FromJsonOverwrite(jsonTextFile.ToString(), LoadFromFile);

        Debug.Log($"Loaded JSON: {jsonTextFile}");
    }
}
