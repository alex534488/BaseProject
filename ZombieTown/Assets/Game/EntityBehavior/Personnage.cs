using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour {

    public class PersonnageEvent : UnityEvent<Personnage> { }

    public Comportement comportement;
    public PersonnageEvent onDeath = new PersonnageEvent();
    public List<string> enemyTags;
    public Detector detector;

    public int damage;
    public int hp;
    public int maxHp;
    protected double movementSpeed;
    public float attackRange;
    public int maxFollowers = -1;

    [SerializeField]
    protected List<Personnage> listFollower = new List<Personnage>();
    public PersonnageEvent onFollowerListChange = new PersonnageEvent();

    public static void Spawn(GameObject character)
    {
        //Personnage nouveauPersonnage = Instantiate(character.gameObject).GetComponent<Personnage>();
    }

    protected virtual void Awake()
    {
        comportement = new Comportement(this);
    }

    void Update()
    {
        comportement.Update();
    }

    private bool CheckEnemyNearby()
    {
        // DO: verifie la zone si les enemy tags sont la
        // comportement.currentStates.target = Personnage
        return false;
    }

    public bool LoseHP(int amount)
    {
        //vv CECI N'EST PAS BON!!! Il ne faut jamais qu'une classe de base (ex: point) fasse des test et des manipulation en rapport à une sous-classe (point coloré)
        if (!(this is Zombie))
        {
            if ((this as Zombie).masterChief != null)
                hp = hp - amount + this.GetComponent<Zombie>().masterChief.GetComponent<Zombie>().bonusHp;

            else
                hp = hp - amount;
        }

        else
            hp -= amount;

        print(hp);

        if (hp <= 0)
        {
            Die();
            return true;
        } // Dead

        return false;
    }

    private bool Die()
    {
        OnDeath();
        Destroy(gameObject);
        return true;
    }

    protected virtual void OnDeath()
    {
        onDeath.Invoke(this);
    }

    protected void AddFollower(Personnage follower)
    {
        listFollower.Add(follower);
        follower.Follow(this);
        onFollowerListChange.Invoke(follower);
    }

    protected void RemoveFollower(Personnage follower)
    {
        if (!listFollower.Contains(follower)) return;

        follower.UnFollow(this);
        listFollower.Remove(follower);
        onFollowerListChange.Invoke(follower);
    }

    public virtual void Follow(Personnage chief)
    {
        (comportement.ChangeState<StatesFollow>() as StatesFollow).Init(chief);
        chief.onDeath.AddListener(OnChiefDeath);
    }

    public virtual void UnFollow(Personnage chief)
    {
        chief.onDeath.RemoveListener(OnChiefDeath);
        comportement.ChangeState<StatesIdle>();
    }

    protected virtual void OnChiefDeath(Personnage chief)
    {
        chief.onDeath.RemoveListener(OnChiefDeath);
        comportement.ChangeState<StatesIdle>();
    }

    protected virtual void OnFollowerDeath(Personnage follower) // Modifie
    {
        RemoveFollower(follower);
    }

    public virtual bool IsFull()
    {
        return (maxFollowers >= 0 && listFollower.Count >= maxFollowers);
    }
}
