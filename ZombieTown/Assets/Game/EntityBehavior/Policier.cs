using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Policier : Personnage
{
    public GameObject sprite;

    // Use this for initialization
    void Start()
    {
        // Variables
        damage = 3;
        hp = 8;
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
        comportement.ChangeState(comportement.GetStatesByName("Attack"));
    }
}
