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
        
        //Setup detector
        if (detector != null)
        {
            System.Type[] enemies =
            {
                typeof(Zombie)
            };
            System.Type[] allies =
            {
                //On ne met aucun allié parce que les humans (policier / civile) n'intéragisse pas ensemble pour l'instant
            };
            detector.Init(enemies, true, allies, false);
            detector.onEnemyEnter.AddListener(OnEnemyEnter);
        }

        // Set Initial Behaviors
        comportement.ChangeState<StatesIdle>();
    }

    void Shoot()
    {
        Bullet.Shoot(damage, transform.forward, transform.position, bullet);
    }

    void OnEnemyEnter(Personnage personnage)
    {
        //to do: Entre en mode d'attaque

        //J'ai mis cette ligne en commentaire parce qu'il y avait un bug et je testais d'autre shit

        //if (!(comportement.currentStates is StatesAttack))
        //{
        //    comportement.ChangeState<StatesAttack>();
        //    (comportement.currentStates as StatesAttack).onLauchingAttack.AddListener(Shoot);
        //}
    }
}


