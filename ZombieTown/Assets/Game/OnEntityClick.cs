using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class OnEntityClick : MonoBehaviour
{ 
    // Personnage presentement selectionner
    public Personnage character; // Personnage ou se trouve ce script
    public GameObject selectAnimation;
    public bool clickable = false;

    public void Select()
    {
        //selectAnimation.SetActive(true);
    }

    public void UnSelect()
    {
        //selectAnimation.SetActive(false);
    }
}
