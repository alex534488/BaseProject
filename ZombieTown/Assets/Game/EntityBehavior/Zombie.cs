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

    [Header("Chief")]
    public GameObject masterChief;
    private List<Personnage> listFollower = new List<Personnage>();

    [Header("Vfx")]
    public GameObject chiefVfx;
    public Sprite chiefSprite;
    public SpriteRenderer spriteRenderer;

    public UnityEvent onLevelUp = new UnityEvent();
    public UnityEvent onChiefNearby = new UnityEvent();

    [Header("Bonus")]
    public int bonusHp;
    public int bonusSpeed;
    public int bonusDmg;
    public int bonusHeal;

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

    void BecomeChief() // Modifie
    {
        lvl = 5;
        spriteRenderer.sprite = chiefSprite;
        if (chiefVfx != null) chiefVfx.SetActive(true);
        if (GetComponent<RVOController>()) GetComponent<RVOController>().radius *= 1.35f; //grossie
        transform.localScale *= 1.35f; // Grossie
        masterChief = this.gameObject;
    }

    void Update()
    {
        //TEMPORAIRE POUR TESTER
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GainXP();
        }
    }

    void Attack() // Modifié
    {

        int totaldamage = masterChief.GetComponent<Zombie>().bonusDmg + damage;

        if ((comportement.currentStates as StatesAttack).target.LoseHP(totaldamage))
        {
            GainXP();
            HealOnKill();

            //Vérifier s'il y a un autre enemy, sinon -> idle
            Personnage newTarget = detector.GetClosestEnemy();

            if (newTarget != null)
            {
                (comportement.currentStates as StatesAttack).Init(newTarget);
            }

            else
            {
                comportement.ChangeState<StatesIdle>();
            }
            
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

    void HealOnKill() // Modifie
    {
        hp = hp + masterChief.GetComponent<Zombie>().bonusHeal;
    }

    public void ClaimLevelUp(LevelUp.Boost boost) // Modifie et TO DO : Augmenter vitesse des goules et du chef
    {
        unclaimedLevelUps--;

        switch (boost.type)
        {
            case LevelUp.Types.Hp:
                {
                    bonusHp = bonusHp + (int)boost.amount;
                    break;
                }

            case LevelUp.Types.Speed:
                {
                    // Augmenter vitesse des goules et du chef
                    break;
                }

            case LevelUp.Types.Dmg:
                {
                    bonusDmg = bonusDmg + (int)boost.amount;
                    break;
                }

            case LevelUp.Types.CarryAmount:
                {
                    nbfollowers = nbfollowers + (int)boost.amount;
                    LookForNewFollowers();
                    break;
                }

            case LevelUp.Types.HealOnKill:
                {
                    bonusHeal = bonusHeal + (int)boost.amount;
                    break;
                }

        }

        //apply stats
    }

    public void Follow(Personnage chief) // Modifié
    {
        comportement.ChangeState<StatesFollow>();

        //to do: mettre le chief comme target
        (comportement.currentStates as StatesFollow).Init(masterChief.GetComponent<Personnage>());
    }

    public bool IsChief()
    {
        return lvl >= 5;
    }

    public bool IsRoom() // Modifie
    {
        if (listFollower.Count < followersmax)
            return true;

        else
            return false;
    }

    public void NewFollower(Personnage newFollower) // Modifie
    {
        newFollower.comportement.ChangeState<StatesFollow>();
        (newFollower.comportement.currentStates as StatesFollow).Init(this.GetComponent<Personnage>());
        newFollower.GetComponent<Zombie>().masterChief = this.gameObject;
    }

    public void LookForNewFollowers() // Modifie
    {
        foreach (Personnage ally in detector.allyList)
        {
            if (!listFollower.Contains(ally))
            {
                listFollower.Add(ally);
                NewFollower(ally);

                if (listFollower.Count >= nbfollowers)
                    return;
            }
        }
    }

    #region Events

    void OnEnemyEnter(Personnage personnage) // Modifie
    {
        //to do: Entre en mode d'attaque

        if (!(comportement.currentStates is StatesMoveTo) && !(comportement.currentStates is StatesAttack))
        {
            comportement.ChangeState<StatesAttack>();
            (comportement.currentStates as StatesAttack).onLauchingAttack.AddListener(Attack);
        }
    }

    void OnAllyEnter(Personnage personnage) // Modifié
    {
        if (IsChief() && IsRoom())
        {
            //to do: si t'es un chief, demande lui de te follow

            if (personnage.GetComponent<Zombie>().masterChief == null && !personnage.GetComponent<Zombie>().IsChief())           
            {
                listFollower.Add(personnage);
                personnage.onDeath.AddListener(OnFollowerDeath);

                NewFollower(personnage);
            }
        }   
    }

    void OnFollowerDeath(Personnage follower) // Modifie
    {
        listFollower.Remove(follower);

        //Compare la liste de tes followers avec la liste de tes alliés en range dans 'detector' ajoute qq1 à tes followers si nécessaire
        LookForNewFollowers();
      
    }

    protected override void OnDeath() // Modifié
    {
        base.OnDeath();

        foreach (Personnage follower in listFollower)
        {
            follower.onDeath.RemoveListener(OnFollowerDeath);
            follower.GetComponent<Zombie>().masterChief = null;
        }

    }

    #endregion
}
