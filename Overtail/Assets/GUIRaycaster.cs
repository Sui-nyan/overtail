//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Overtail.Util;

namespace Overtail.GUI
{
    public class GUIRaycaster : MonoBehaviour
    {
        private GraphicRaycaster _raycaster;
        [SerializeField] private PointerEventData _pointerEventData;
        private EventSystem _eventSystem;
        private static GUIRaycaster _instance;

        [SerializeField] private GameObject _recentlyClickedGameObject;
        public GameObject RecentlyClickedGameObject => _recentlyClickedGameObject;

        void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance);
            //Fetch the Raycaster from the GameObject (the Canvas)
            _raycaster = GetComponent<GraphicRaycaster>() ?? gameObject.AddComponent<GraphicRaycaster>();
            //Fetch the Event System from the Scene
            _eventSystem = GetComponent<EventSystem>() ?? FindObjectOfType<EventSystem>();
        }
        
        void Update()
        {
            var GetClick = Input.GetKeyDown(KeyCode.Mouse0);
            if (GetClick)
            {
                _recentlyClickedGameObject = GetFirstHit();
            }
        }


        public GameObject GetFirstHit()
        {
            //Set up the new Pointer Event
            _pointerEventData = new PointerEventData(_eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            _pointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            _raycaster.Raycast(_pointerEventData, results);

            return results.Count > 0 ? results[0].gameObject : null;
        }

        public bool RaycastInChildren(GameObject root)
        {
            var hit = GetFirstHit();
            return IsParentOf(test: hit, root: root);
        }

        public static bool IsParentOf(GameObject root, GameObject test)
        {
            if (test == null) return false;
            if (test == root) return true;
            var parent = test.transform.parent;
            if (parent == null) return false;
            if (parent.gameObject == root) return true;

            return IsParentOf(parent.gameObject, test);
        }
    }
}