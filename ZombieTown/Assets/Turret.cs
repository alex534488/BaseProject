using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Turret : Personnage
{
    private int baseHp;
    private int baseDamage;
    public GameObject bullet;

    void Start()
    {
        baseHp = 1000;
        baseDamage = 50;

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
    }

    void OnEnable()
    {
        damage = baseDamage;
        hp = baseHp;
        attackRange = 1;
    }

    void BonusStats(int a)
    {
        damage = baseDamage * a;
        hp = baseHp * a;
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

    void Shoot()
    {
        Bullet.Shoot(damage, transform.forward, transform.position, bullet);
    }
}
