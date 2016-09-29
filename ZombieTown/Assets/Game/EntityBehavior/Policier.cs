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

    void Update()
    {

    }

    void OnEnemyNearby()
    {
        if (!(comportement.currentStates is StatesMoveTo))
        {
            comportement.ChangeState(comportement.GetStatesByName("Attack"));
            (comportement.currentStates as StatesAttack).onHittingTarget.AddListener(Shoot);
        }
    }

    void Shoot()
    {
        Bullet launchedBullet = Instantiate(bullet.gameObject).GetComponent<Bullet>();
        launchedBullet.Init(damage,,1);
    }
}


