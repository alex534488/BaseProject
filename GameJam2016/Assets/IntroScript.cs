using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour {

    float startTime;
    string txt;

    

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        txt = GetComponent<Text>().text;

    }
	
	// Update is called once per frame
	void Update () {
	    if(Time.time - startTime > 0 && Time.time - startTime < 10)
        {
            GetComponent<Text>().text = "Rome, l'empire qui a fait régner la Pax Romana. "+ 
               " L'empire qui a conquis la Méditerranée.";
        }

        if (Time.time - startTime > 10 && Time.time - startTime < 20)
        {
            GetComponent<Text>().text = "Aujourd'hui, Rome commence sa chute." +
                "Aujourd'hui, Rome affronte la barbarie.";
        }

        if (Time.time - startTime > 20 && Time.time - startTime < 30)
        {
            GetComponent<Text>().text = "Aujourd'hui, vous prenez les rennes."+
                "Aujourd'hui, vous êtes nommé Empereur.";
        }

        if (Time.time - startTime > 30 && Time.time - startTime < 40)
        {
            GetComponent<Text>().text = "La question n'est pas 'si' mais 'quand' Rome tombera." +
                "Et la réponse, ce sera votre gouvernance qui la donnera.";
        }
        if (Time.time - startTime > 40 && Time.time - startTime < 60)
        {
            GameObject.Find("Titre").GetComponent<Text>().text = "RUINA ROMA " +
                "Gouvernance d'un Empereur";

        }
        if(Time.time - startTime > 50)
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        
    }
}
