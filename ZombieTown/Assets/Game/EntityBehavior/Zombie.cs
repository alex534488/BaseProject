using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Zombie : Personnage {
    public GameObject sprite;

    // Use this for initialization
    void Start () {
        // Variables
        damage = 2;
        hp = 10;
        movementSpeed = 0.5;

        // Behavior
        onEnemyNearby.AddListener(Attack);
        enemyTags = new List<string>(2);
        enemyTags.Add("Policier");
        enemyTags.Add("Civil");
    }
	
	void Update ()
    {
        
    }

    void Attack()
    {
        comportement.ChangeState(comportement.GetStatesByName("Attack"));
    }
}
