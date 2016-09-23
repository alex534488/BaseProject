using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace CCC.Manager
{
    public class InputManager : BaseManager
    {
        public class KeySave
        {
            public string keyName;
            public KeyCode keycode;

            public KeySave(string name, KeyCode keycode)
            {
                this.keyName = name;
                this.keycode = keycode;
            }
        }

        [System.Serializable]
        public class SaveClass
        {
            public List<KeySave> keySaves = new List<KeySave>();

            public KeySave Get(Key key)
            {
                foreach(KeySave keySave in keySaves)
                {
                    if (keySave.keyName == key.GetName()) return keySave;
                }
                return null;
            }
        }

        public InputBank bank;
        SaveClass save;

        /// <summary>
        /// Set the specified key to the specified keycode. This does not save the change permanently. Call Save() if necessary.
        /// </summary>
        public void SetKey(Key key, KeyCode to)
        {
            //Vérifier si la save existe déjà, si oui, la remplacer
            KeySave keySave = save.Get(key);
            if(key == null)
            {
                keySave = new KeySave(key.GetName(), to);
                save.keySaves.Add(keySave);
            }
            else
            {
                keySave.keycode = to;
            }

            key.SetKeyCode(to);
        }

        public void SetDefault()
        {

        }

        public void Load()
        {

        }

        public void Save()
        {

        }
    }
}
