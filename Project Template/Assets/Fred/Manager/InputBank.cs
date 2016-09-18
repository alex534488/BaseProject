using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Fred.Manager
{
    [System.Serializable]
    public class Key
    {
        private string name;
        private KeyCode keyCode;
        public bool openInInspector = false;

        public static bool Compare(Key a, Key b)
        {
            return a.name == b.name;
        }

        public Key(string name, KeyCode keycode)
        {
            this.name = name;
            this.keyCode = keycode;
        }

        public void Copy(Key key)
        {
            name = key.name;
            keyCode = key.keyCode;
        }

        public KeyCode GetKeyCode() { return keyCode; }
        public void SetKeyCode(KeyCode keycode) { this.keyCode = keycode; }
        public string GetName() { return name; }
        public void SetName(string name)
        {
            if(Application.isPlaying)
            {
                Debug.LogError("Cannot modify the name of a Key in runtime.");
                return;
            }
            this.name = name;
        }
        
        public bool GetDown()
        {
            return Input.GetKeyDown(keyCode);
        }
        public bool GetUp()
        {
            return Input.GetKeyUp(keyCode);
        }
        public bool Get()
        {
            return Input.GetKey(keyCode);
        }
    }

    [CreateAssetMenu(menuName = "Input Bank")]
    public class InputBank : ScriptableObject
    {
        //public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
        public List<Key> keys;

        public void MoveBy(Key key, int amount)
        {
            if (!keys.Contains(key)) return;

            int a = keys.IndexOf(key);
            int b = a + amount;
            b = Mathf.Clamp(b, 0, keys.Count - 1);
            if (a == b) return;

            Key bKey = keys[b];

            keys.Remove(key);
            keys.Insert(b, key);
            keys.Remove(bKey);
            keys.Insert(a, bKey);
        }
    }

    [CustomEditor(typeof(InputBank))]
    public class InputBankEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            InputBank bank = target as InputBank;

            //Create List
            if(bank.keys == null)
            {
                bank.keys = new List<Key>();
            }

            //Keys
            for(int i=0; i<bank.keys.Count; i++)
            {
                if (i >= bank.keys.Count) continue;

                Key key = bank.keys[i];

                key.openInInspector = EditorGUILayout.Foldout(key.openInInspector, "key: " + key.GetName());

                if (key.openInInspector)
                {
                    key.SetName(EditorGUILayout.TextField("name", key.GetName()));
                    key.SetKeyCode((KeyCode)EditorGUILayout.EnumPopup("KeyCode", key.GetKeyCode()));
                    GUILayout.BeginHorizontal();
                    if (!Application.isPlaying && GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        bank.keys.Remove(key); // Remove Key
                    }
                    if (GUILayout.Button("^", GUILayout.Width(30)))
                    {
                        bank.MoveBy(key, -1); // Move up
                    }//
                    if (GUILayout.Button("v", GUILayout.Width(30)))
                    {
                        bank.MoveBy(key, 1); // Move down
                    }

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }

            //Add Key
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!Application.isPlaying && GUILayout.Button("Add Key"))
            {
                bank.keys.Add(new Key("", KeyCode.Asterisk)); // Add
            }
        }
    }
}
