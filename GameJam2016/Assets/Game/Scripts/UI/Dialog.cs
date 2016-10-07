using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class Dialog : MonoBehaviour {

    public class Choix
    {
        public Choix(string text, UnityAction callback)
        {
            this.text = text;
            this.callback = callback;
        }
        public string text;
        public UnityAction callback;
    }

    public static void Text(string messageComplet, List<Choix> listeChoix = null) { }

}
