using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Turret : Personnage
{
    private int baseHp;
    private int baseDamage;

    void Start()
    {
        baseHp = 1000;
        baseDamage = 50;
    }

    void OnEnable()
    {
        damage = baseDamage;
        hp = baseHp;
        range = 1;
    }

    void BonusStats(int a)
    {
        damage = baseDamage * a;
        hp = baseHp * a;
    }


}
