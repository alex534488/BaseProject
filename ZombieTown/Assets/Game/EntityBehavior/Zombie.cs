using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Pathfinding.RVO;

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
    public Sprite chiefSprite;
    public SpriteRenderer spriteRenderer;

    public UnityEvent onLevelUp = new UnityEvent();
    public UnityEvent onChiefNearby = new UnityEvent();

    void Start()
    {
        // Set Variables
        damage = 2;
        hp = 10;
        movementSpeed = 0.5;
        nbfollowers = 0;
        if (chiefVfx != null) chiefVfx.SetActive(false);

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
        spriteRenderer.sprite = chiefSprite;
        if (chiefVfx != null) chiefVfx.SetActive(true);
        if (GetComponent<RVOController>()) GetComponent<RVOController>().radius *= 1.35f; //grossie
        transform.localScale *= 1.35f; // Grossie
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
            if(nbchiefs >= nbchiefsmax && lvl == 4) // Cant lvl up, too many chiefs
            {
                XP--;
                return;
            }

            if(lvl >= 4) //Will be OR is already chief
            {
                if (!IsChief()) BecomeChief(); //Become a chief if you aren't
                else lvl++;

                unclaimedLevelUps++; //Get level up bonus
                onLevelUp.Invoke();

            }
            else //Not a chief, nor will be
            {
                lvl++;
                XP = 0;
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

    public bool IsChief()
    {
        return lvl >= 5;
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
