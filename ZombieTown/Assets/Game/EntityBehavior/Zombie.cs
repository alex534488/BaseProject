using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Pathfinding.RVO;

public class Zombie : Personnage
{
    public bool startsAsChief = false;
    public int lvl = 1;

    public System.Type[] enemies;

    private static int nbchiefs = 0;
    static public int nbchiefsmax = 4;

    public int unclaimedLevelUps = 0;
    private int XP = 0;

    [Header("Chief")]
    public Zombie masterChief;

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
        if (GetComponent<SphereCollider>()) GetComponent<SphereCollider>().radius *= 1.35f; //grossie
        if (spriteRenderer) spriteRenderer.transform.localScale *= 1.35f; //grossie
        masterChief = this;
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
        int totaldamage;

        if (masterChief != null)
            totaldamage = masterChief.bonusDmg + damage;

        else
            totaldamage = damage;

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
                if(masterChief != null && !IsChief())
                {
                    Follow(masterChief);
                }
                else comportement.ChangeState<StatesIdle>();
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
        if (masterChief != null)
            hp = hp + masterChief.bonusHeal;
        //Heal de base ?
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
                    maxFollowers += (int)boost.amount;
                    LookForNewFollowers();
                    break;
                }

            case LevelUp.Types.HealOnKill:
                {
                    bonusHeal = bonusHeal + (int)boost.amount;
                    break;
                }

        }
    }

    public override void Follow(Personnage chief)
    {
        if(chief is Zombie) masterChief = chief as Zombie;
        base.Follow(chief);
    }

    public bool IsChief()
    {
        return lvl >= 5;
    }

    public void LookForNewFollowers()
    {
        foreach (Personnage ally in detector.allyList)
        {
            if (!listFollower.Contains(ally))
            {
                AddFollower(ally);

                if (IsFull()) return;
            }
        }
    }

    #region Events

    void OnEnemyEnter(Personnage personnage) // Modifie
    {
        if (!(personnage is Policier)) return;

        Policier policier = personnage as Policier;

        if (!(comportement.currentStates is StatesMoveTo) && !(comportement.currentStates is StatesAttack))
        {
            StatesAttack state = (comportement.ChangeState<StatesAttack>() as StatesAttack);
            state.onLauchingAttack.AddListener(Attack);
            state.Init(policier);
        }
    }

    void OnAllyEnter(Personnage personnage) // Modifié
    {
        if (!(personnage is Zombie)) return;

        Zombie zombie = personnage as Zombie;

        if (IsChief() && !IsFull())
        {
            if (zombie.masterChief == null && !zombie.IsChief())           
            {
                AddFollower(personnage);
            }
        }   
    }

    protected override void OnFollowerDeath(Personnage follower) // Modifie
    {
        base.OnFollowerDeath(follower);

        LookForNewFollowers();
    }

    protected override void OnChiefDeath(Personnage chief)
    {
        base.OnChiefDeath(chief);

        masterChief = null;
    }

    #endregion
}
