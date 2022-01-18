using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPosition : MonoBehaviour
{
    /**
    private const string SAVE_SEPERATOR = "#SAVE-VALUE#";

    [SerializeField] private GameObject unitGameObject;
    private IUnit unit;

    private void Awake()
    {
        unit = unitGameObject.GetComponent<IUnit>();
    }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Save()
    {
        Vector3 playerPosition = unit.GetPosition();

        PlayerPrefs.SetFloat("playerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("playerPositionY", playerPosition.y);

        PlayerPrefs.Save();
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(playerPositionX))
        {
            float playerPositionX = PlayerPrefs.GetFloat("playerPositionX");
            float playerPositionY = PlayerPrefs.GetFloat("playerPositionY");
            Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY);

            unit.SetPosition(playerPosition);
        }
        else
        {
            CMDebug.TextPopupMouse("No save");
        }
    }  
        
    private class SaveObject
    {
        
    }*/
}
