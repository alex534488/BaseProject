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
    public int range;

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
        if (this.GetComponent<Zombie>().masterChief != null)
            hp = hp - amount + this.GetComponent<Zombie>().masterChief.GetComponent<Zombie>().bonusHp;

        else
            hp -= amount;

        print(hp);

        if (hp < 0)
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
}
