using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Zombie : Personnage
{

    public bool startsAsChief = false;
    public int lvl = 1;
    public int followersmax;

    public int nbfollowers;

    public System.Type[] enemies;

    private static int nbchiefs = 0;
    static public int nbchiefsmax = 4;

    public int unclaimedLevelUps = 0;
    private int XP = 0;

    [Header("Vfx")]
    public GameObject chiefVfx;

    public UnityEvent onLevelUp = new UnityEvent();
    public UnityEvent onChiefNearby = new UnityEvent();

    void Start()
    {
        // Set Variables
        damage = 2;
        hp = 10;
        movementSpeed = 0.5;
        nbfollowers = 0;
        ChiefVfx(false);

        // Zombie initial
        if (startsAsChief == true) BecomeChief();

        // Set Initial Behaviors
        comportement.ChangeState<StatesIdle>();

        //Setup detector
        if (detector != null)
        {
            System.Type[] enemies =
            {
                typeof(Policier),
                typeof(Civil),
                typeof(Walls)
            };
            System.Type[] allies =
            {
                typeof(Zombie)
            };
            detector.Init(enemies, false, allies, false);
            detector.onAllyEnter.AddListener(OnAllyEnter);
            detector.onEnemyEnter.AddListener(OnEnemyEnter);
        }
    }

    void BecomeChief()
    {
        lvl = 5;
        ChiefVfx(true);
    }

    void Update()
    {
        //TEMPORAIRE POUR TESTER
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GainXP();
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
            if (!((lvl + 1) == 5 && nbchiefs == nbchiefsmax))
            {
                lvl++;
                XP = 0;

                //Get Boosts if lvl >= 5
                if (lvl > 5)
                {
                    ChiefVfx(true);
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

    //Recoie l'ordre de follow le chief
    public void Follow(Personnage chief)
    {
        //to do: mettre le chief comme target
        comportement.ChangeState<StatesFollow>();
    }

    void ChiefVfx(bool state)
    {
        if (chiefVfx != null) chiefVfx.SetActive(state);
    }

    #region Events

    void OnEnemyEnter(Personnage personnage)
    {
        //to do: Entre en mode d'attaque

        //J'ai mis cette ligne en commentaire parce qu'il y avait un bug et je testais d'autre shit

        //if (!(comportement.currentStates is StatesMoveTo) && !(comportement.currentStates is StatesAttack))
        //{
        //    comportement.ChangeState<StatesAttack>();
        //    (comportement.currentStates as StatesAttack).onLauchingAttack.AddListener(Attack);
        //}
    }

    void OnAllyEnter(Personnage personnage)
    {
        //to do: si t'es un chief, demande lui de te follow
    }

    void OnFollowerDeath()
    {
        //to do: si t'es un chef, libère un de tes spots.
        //Compare la liste de tes followers avec la liste de tes allié en range dans 'detector' ajoute qq1 à tes followers si nécessaire
    }

    #endregion
}
