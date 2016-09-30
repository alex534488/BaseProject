using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class OnEntityClick : MonoBehaviour
{ 
    // Personnage presentement selectionner
    public Zombie zombie; // Personnage ou se trouve ce script
    public GameObject selectAnimation;
    public bool clickable = false;

    void Awake()
    {
        UnSelect();
    }

    void Update()
    {
        if(zombie.lvl >= 5)
        {
            clickable = true;
        }
    }

    public void Select()
    {
        if(selectAnimation != null) selectAnimation.SetActive(true);
    }

    public void UnSelect()
    {
        if (selectAnimation != null) selectAnimation.SetActive(false);
    }
}
