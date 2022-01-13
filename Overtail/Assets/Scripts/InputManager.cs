using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overtail.Items;
using Overtail.Util;
using UnityEngine;
namespace Overtail
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
        public static InputManager Instance => _instance;

        public KeyCode BindMenu = KeyCode.Escape;
        public KeyCode BindInventory = KeyCode.I;
        public KeyCode BindOptions = KeyCode.O;

        public KeyCode BindUp = KeyCode.W;
        public KeyCode BindDown = KeyCode.S;
        public KeyCode BindLeft = KeyCode.A;
        public KeyCode BindRight = KeyCode.D;

        public KeyCode BindConfirm = KeyCode.Space;
        public KeyCode BindCancel = KeyCode.Backspace;

        // GetKeyDown
        public event Action KeyMenu;
        public event Action KeyInventory;
        public event Action KeyOptions;

        public event Action KeyConfirm;
        public event Action KeyCancel;

        // GetKey
        public event Action KeyUp;
        public event Action KeyDown;
        public event Action KeyLeft;
        public event Action KeyRight;

        void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive: true, destroyOnSceneZero: true);
        }

        void Update()
        {
            TriggerOnce(BindMenu, KeyMenu);
            TriggerOnce(BindInventory, KeyInventory);
            TriggerOnce(BindOptions, KeyOptions);

            TriggerOnce(BindConfirm, KeyConfirm);
            TriggerOnce(BindCancel, KeyCancel);

            TriggerNAND(BindUp, KeyUp, BindDown, KeyDown);
            TriggerNAND(BindLeft, KeyLeft, BindRight, KeyRight);
        }


        private void TriggerOnce(KeyCode keyCode, Action keyEvent)
        {
            if (Input.GetKeyDown(keyCode)) keyEvent?.Invoke();
        }

        private void TriggerContinuously(KeyCode keyCode, Action keyEvent)
        {
            if (Input.GetKey(keyCode)) keyEvent?.Invoke();
        }

        private void TriggerNAND(KeyCode c1, Action e1, KeyCode c2, Action e2)
        {
            if (Input.GetKey(c1) && Input.GetKey(c2)) return;

            if (Input.GetKey(c1)) e1?.Invoke();
            if (Input.GetKey(c2)) e2?.Invoke();
        }
    }
}