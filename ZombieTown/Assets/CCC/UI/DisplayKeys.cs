using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

namespace CCC.UI
{
    public class DisplayKeys : MonoBehaviour
    {
        [Header("KEY BINDING BUTTONS")]
        public KeyButton buttonPrefab;
        [Header("FIRST POP UP SCREEN (START)")]
        public GameObject StartScreen;

        private InputManager manager = MasterManager.master.GetManager<InputManager>() as InputManager;
        private KeyCode newKeyCode;
        private UnityEvent onKeyDown;
        private Key currentKey;

        void Awake()
        {
            foreach (Key key in manager.GetAllKeys())
            {
                KeyButton button = Instantiate(this.buttonPrefab.gameObject).GetComponent<KeyButton>();
                button.key = key;
                button.onClick.AddListener(BindingStart);
                // DO: Changer l'emplacement du nouveau bouton pour faire un vertical layout group
            }
        }


        void BindingStart(Key key)
        {
            currentKey = key;
            StartScreen.SetActive(true);
            onKeyDown.AddListener(Binding);
            StartCoroutine(ReceiveInput()); // Waiting for player to input a key
        }

        void Binding()
        {
            onKeyDown.RemoveListener(Binding);
            // Check conflict
            manager.SetKey(currentKey, newKeyCode);
        }

        // Co-routine qui permet d'attendre que le joueur entre une touche
        IEnumerator ReceiveInput()
        {
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
            newKeyCode = FetchKey();
            onKeyDown.Invoke();
        }

        // Fonction cherchant le KeyCode entrée par le joueur
        KeyCode FetchKey()
        {
            //int e = System.Enum.GetNames(typeof(KeyCode)).Length;
            for (int i = 0; i < 330; i++)
            {
                if (Input.GetKey((KeyCode)i))
                {
                    return (KeyCode)i;
                }
            }

            return KeyCode.None;
        }

        /*
        void CheckConflict(Key aKey)
        {
            foreach (Key key in manager.bank.keys)
            {
                if (key.GetKeyCode() == newKeyCode)
                {
                    StopScreen.SetActive(true);
                    return;
                }
            }
        }
        */
    }
}