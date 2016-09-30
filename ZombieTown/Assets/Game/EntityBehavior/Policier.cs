using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Policier : Personnage
{
    public GameObject bullet;
    
    // Use this for initialization
    void Start()
    {
        // Set Variables
        damage = 3;
        hp = 8;
        movementSpeed = 1;

        // Set Behavior
        onEnemyNearby.AddListener(OnEnemyNearby);
        enemyTags = new List<string>(1);
        enemyTags.Add("Zombie");
    }

    void OnEnemyNearby()
    {
        if (!(comportement.currentStates is StatesAttack))
        {
            comportement.ChangeState<StatesAttack>();
            (comportement.currentStates as StatesAttack).onLauchingAttack.AddListener(Shoot);
        }
    }

    void Shoot()
    {
        Bullet.Shoot(damage, transform.forward, transform.position, bullet);
    }
}


