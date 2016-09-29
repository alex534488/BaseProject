using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Manager;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CCC.UI
{
    public class KeyButton : MonoBehaviour
    {
        public class KeyEvent : UnityEvent<Key> { }

        public Key key;
        public KeyEvent onClick = new KeyEvent();

        void Start()
        {
            key.onModify.AddListener(UpdateDisplay);
        }

        private void UpdateDisplay()
        {
            this.GetComponentInChildren<Text>().text = "" + key.GetName() + ": " + key.GetKeyCode(); // KeyCode affiche quoi?
        }
    }
}
