using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Manager;

public class Walls : Personnage {

    private int baseHp;

    void Start()
    {
        baseHp = 100;
    }

	void OnEnable ()
    {
        damage = 0;
        hp = baseHp;
        attackRange = 0;
	}

    public void GainHP(int level)
    {
        hp = baseHp * level;
    }
	
}
