using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Zombie : Personnage {

    public bool startsAsChief = false;
    public int lvl = 1;
    public int followersmax;
    
    public int nbfollowers;

    private static int nbchiefs = 0;
    static public int nbchiefsmax = 4;

    public int unclaimedLevelUps = 0;
    private int XP = 0;
    public UnityEvent onLevelUp = new UnityEvent();

    protected override void Awake () {
        base.Awake();
        // Set Variables
        damage = 2;
        hp = 10;
        movementSpeed = 0.5;
        nbfollowers = 0;

        // Zombie initial
        if(startsAsChief == true){ lvl = 5; } 

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

    void Update()
    {
        //TEMPORAIRE POUR TESTER
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GainXP();
        }
    }

    void OnEnemyNearby()
    {
        if(!(comportement.currentStates is StatesMoveTo) && !(comportement.currentStates is StatesAttack))
        {
            comportement.ChangeState<StatesAttack>();
            (comportement.currentStates as StatesAttack).onLauchingAttack.AddListener(Attack);
        }
    }

    void Attack()
    {
        if ((comportement.currentStates as StatesAttack).target.LoseHP(damage))
        {
            GainXP();
            //Vérifier s'il y a un autre enemy, sinon -> idle
        }
    }

    void GainXP()
    {
        XP++;
        if (XP >= Mathf.CeilToInt(Mathf.Pow(lvl, 2) / 2))
        {
            //LEVEL UP!
            if(!((lvl+1)==5 && nbchiefs == nbchiefsmax))
            {
                lvl++;
                XP = 0;

                //Get Boosts if lvl >= 5
                if(lvl > 5)
                {
                    unclaimedLevelUps++;
                    onLevelUp.Invoke();
                }
            }
            else  //CANT LEVEL UP!
            {
                XP--;
            }
        }
    }

    public void ClaimLevelUp(LevelUp.Boost boost)
    {
        unclaimedLevelUps--;
        //apply stats
    }
}
