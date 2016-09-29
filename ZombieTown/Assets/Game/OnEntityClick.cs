using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class OnEntityClick : MonoBehaviour
{
    public static OnEntityClick selectedChar = null; // Personnage presentement selectionner
    public Personnage character; // Personnage ou se trouve ce script
    public GameObject selectAnimation;
    public bool clickable = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // If right click
        {
            //character.comportement.currentStates.MoveTo();
        }
    }

    void OnMouseDown()
    {
        if (!Input.GetMouseButtonDown(0)){ return; }

        if (clickable)
        {
            if (selectedChar != character)
            {
            selectedChar = this;
            selectedChar.UnSelect();
            //selectAnimation.SetActive(true);
            }   
        }
    }

    public void UnSelect()
    {
        //selectAnimation.SetActive(false);
    }
}
