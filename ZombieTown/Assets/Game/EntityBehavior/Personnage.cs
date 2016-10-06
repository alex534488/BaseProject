using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour
{

    public class PersonnageEvent : UnityEvent<Personnage> { }

    public Comportement comportement;
    public PersonnageEvent onDeath = new PersonnageEvent();
    public List<string> enemyTags;
    public Detector detector;

    public int damage = 1;
    public int hp = 10;
    public int maxHp = 10;
    protected double movementSpeed = 1;
    public float attackRange = 5;
    public int maxFollowers = -1;

    public List<Personnage> listFollower = new List<Personnage>();
    public PersonnageEvent onFollowerListChange = new PersonnageEvent();

    public static void Spawn(GameObject character)
    {
        //Personnage nouveauPersonnage = Instantiate(character.gameObject).GetComponent<Personnage>();
    }

    protected virtual void Awake()
    {
        comportement = new Comportement(this);
    }

    protected virtual void Update()
    {
        comportement.Update();
    }

    private bool CheckEnemyNearby()
    {
        // DO: verifie la zone si les enemy tags sont la
        // Important : comportement.currentStates.target = Personnage
        return false;
    }

    public virtual bool LoseHP(int amount)
    {
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
        onFollowerListChange.Invoke(this);
    }

    protected void RemoveFollower(Personnage follower)
    {
        if (!listFollower.Contains(follower)) return;

        follower.UnFollow(this);
        listFollower.Remove(follower);
        onFollowerListChange.Invoke(this);
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

    public virtual void OnIdle()
    {

    }

    public virtual void AttackState(Personnage target)
    {
        StatesAttack state = (comportement.ChangeState<StatesAttack>() as StatesAttack);
        state.onLauchingAttack.AddListener(OnAttack);
        state.Init(target);
    }
    public virtual void OnAttack()
    {

    }

    public virtual bool IsFull()
    {
        return (maxFollowers >= 0 && listFollower.Count >= maxFollowers);
    }
}
