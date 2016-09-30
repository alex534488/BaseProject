using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Zombie : Personnage {

    public bool starter = false;
    private int XP = 0;
    public int lvl = 1;
    public int followersmax;
    private int nbfollowers;
    private static int nbchiefs;
    public int nbchiefsmax;

    void Awake () {
        // Set Variables
        damage = 2;
        hp = 10;
        movementSpeed = 0.5;
        nbfollowers = 0;

        // Zombie initial
        if(starter == true){ lvl = 5; } 

        // Set Behaviors
        onEnemyNearby.AddListener(OnEnemyNearby);
        enemyTags = new List<string>(2);
        enemyTags.Add("Policier");
        enemyTags.Add("Civil");
    }

    void Start()
    {
        if(lvl < 5)
        {
            // Assigner le zombie a son chief
            comportement.ChangeState<StatesFollow>();
        }
    }
	
	void Update ()
    {
        
    }

    void OnEnemyNearby()
    {
        if(!(comportement.currentStates is StatesMoveTo))
        {
            comportement.ChangeState(comportement.GetStatesByName("Attack"));
            (comportement.currentStates as StatesAttack).onHittingTarget.AddListener(Attack);
            (comportement.currentStates as StatesAttack).onEnemyKilled.AddListener(GainXP);
        }
    }

    void Attack()
    {
        (comportement.currentStates as StatesAttack).target.LoseHP(damage);
    }

    void GainXP()
    {
        XP++;
        if (XP == Mathf.CeilToInt(Mathf.Pow(lvl, 2) / 2))
        {
            if(!((lvl+1)==5 && nbchiefs == nbchiefsmax)) { lvl++; }
            XP = 0;
        }
    }
}
