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
        }

        [System.Serializable]
        public class SaveClass
        {
            List<KeySave> keySaves = new List<KeySave>();
        }

        public InputBank bank;
        //public Unity

        public void SetKey(KeySave keySave)
        {
            //Vérifier si la save existe déjà, si oui, la remplacer
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
