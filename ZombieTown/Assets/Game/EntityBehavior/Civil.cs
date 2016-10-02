using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Civil : Personnage
{
    public GameObject sprite;
    public Vector2 mapSize;

    // Use this for initialization
    void Start()
    {
        // Variables
        damage = 0;
        hp = 5;
        movementSpeed = 1;

        // Set Initial Behaviors
        comportement.ChangeState<StatesIdle>();
    }


}
