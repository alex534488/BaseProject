using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Zombie : Personnage {

    public bool starter = false;
    public int XP = 0;
    public int lvl = 1;
    public int nbchiefs;

    // Use this for initialization
    void Start () {
        damage = 2;
        hp = 10;
        movementSpeed = 0.5;

        if(starter == true)
        {
            lvl = 5;
        }

        // Behavior
        onEnemyNearby.AddListener(OnEnemyNearby);
        enemyTags = new List<string>(2);
        enemyTags.Add("Policier");
        enemyTags.Add("Civil");
    }
	
	void Update ()
    {
        
    }

    void OnEnemyNearby()
    {
        if(!(comportement.currentStates is StatesMoveTo))
        {
            comportement.ChangeState(comportement.GetStatesByName("Attack"));
            (comportement.currentStates as StatesAttack).onEnemyKilled.AddListener(GainXP);
        }
    }

    public void GainXP()
    {
        XP++;
        if (XP == Mathf.CeilToInt(Mathf.Pow(lvl, 2) / 2))
        {
            if(!((lvl+1)==5 && nbchiefs == 4)) { lvl++; }
            XP = 0;
        }
    }
}
