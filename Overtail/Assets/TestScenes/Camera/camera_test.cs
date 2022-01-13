using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Camera;
using UnityEngine.UI;


public class camera_test : MonoBehaviour
{

    public HighlightCamera Camera;
    public Text DebugTetxt;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.FindObjectOfType<Camera>().gameObject.GetComponent<HighlightCamera>();
        DebugTetxt = GameObject.Find("QueueCount").GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetInputRoutine();
        DebugTetxt.text = $"{Camera.QueueCount} targets in queue";
    }

    private bool GetInputRoutine()
    {
        // returns true if calling context should continue;
        // Test code for funsies

        if (Input.GetKeyDown(KeyCode.Tab)) Camera.GoToNext();

        if (Input.GetKey(KeyCode.Space))
        {
            Camera.SnapTo(GameObject.FindGameObjectWithTag("Player"));
            return false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) Camera.SetTarget(GameObject.FindGameObjectWithTag("Player"), -1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Camera.SetTarget(GameObject.FindGameObjectWithTag("Dummy1"), -1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Camera.SetTarget(GameObject.FindGameObjectWithTag("Dummy2"), -1);
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
            Camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
            Camera.GoToNext();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) Camera.JumpQueue(GameObject.FindGameObjectWithTag("Dummy1"), .2f);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Camera.JumpQueue(GameObject.FindGameObjectWithTag("Dummy2"), .2f);
        if (Input.GetKeyDown(KeyCode.Escape)) Camera.ClearAll();

        return true;
    }
}