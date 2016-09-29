using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Civil : Personnage
{
    public GameObject sprite;

    // Use this for initialization
    void Start()
    {
        // Variables
        damage = 0;
        hp = 5;
        movementSpeed = 1;

        // Behavior
        onEnemyNearby.AddListener(Attack);
        enemyTags = new List<string>(1);
        enemyTags.Add("Zombie");
    }

    void Update()
    {

    }

    void Attack()
    {
        comportement.ChangeState(comportement.GetStatesByName("Flee"));
    }
}
