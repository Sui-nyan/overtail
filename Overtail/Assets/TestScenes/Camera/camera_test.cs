using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Camera;
using UnityEngine.UI;


public class camera_test : MonoBehaviour
{

    public HighlightCamera camera;
    public Text debug;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindObjectOfType<Camera>().gameObject.GetComponent<HighlightCamera>();
        debug = GameObject.Find("QueueCount").GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetInputRoutine();
        debug.text = $"{camera.QueueCount} targets in queue";
    }

    private bool GetInputRoutine()
    {
        // returns true if calling context should continue;
        // Test code for funsies

        if (Input.GetKeyDown(KeyCode.Tab)) camera.GoToNext();

        if (Input.GetKey(KeyCode.Space))
        {
            camera.SnapTo(GameObject.FindGameObjectWithTag("Player"));
            return false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) camera.SetTarget(GameObject.FindGameObjectWithTag("Player"), -1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) camera.SetTarget(GameObject.FindGameObjectWithTag("Dummy1"), -1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) camera.SetTarget(GameObject.FindGameObjectWithTag("Dummy2"), -1);
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            camera.Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
            camera.Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
            camera.GoToNext();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) camera.JumpQueue(GameObject.FindGameObjectWithTag("Dummy1"), .2f);
        if (Input.GetKeyDown(KeyCode.Alpha5)) camera.JumpQueue(GameObject.FindGameObjectWithTag("Dummy2"), .2f);
        if (Input.GetKeyDown(KeyCode.Escape)) camera.ClearAll();

        return true;
    }
}