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
        if (!(comportement.currentStates is StatesAttack))
        {
            comportement.ChangeState<StatesAttack>();
            (comportement.currentStates as StatesAttack).Init(personnage);
            (comportement.currentStates as StatesAttack).onLauchingAttack.AddListener(Shoot);
        }
    }
}


