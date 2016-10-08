using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Request : MonoBehaviour {

    List<string> message = new List<string>();
    List<Dialog.Choix> choix = new List<Dialog.Choix>();



    public Request (List<string> message, List<Dialog.Choix> choix)
    {
        this.message = message;
        this.choix = choix;
    }

    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
