using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour {

    public class PersonnageEvent : UnityEvent<Personnage> { }

    public Comportement comportement;
    public PersonnageEvent onDeath = new PersonnageEvent();
    public UnityEvent onEnemyNearby = new UnityEvent();
    public List<string> enemyTags;

    public int damage;
    public int hp;
    protected double movementSpeed;
    public int range;

    public static void Spawn(GameObject character)
    {
        //Personnage nouveauPersonnage = Instantiate(character.gameObject).GetComponent<Personnage>();
    }

    protected virtual void Awake ()
    {
        comportement = new Comportement(this);
    }
	
	void Update ()
    {
        comportement.Update();

        if(CheckEnemyNearby())
        {
            onEnemyNearby.Invoke();
        }
    }

    private bool CheckEnemyNearby()
    {
        // DO: verifie la zone si les enemy tags sont la
        // comportement.currentStates.target = Personnage
        return true;
    }

    public bool LoseHP(int amount)
    {
        hp -= amount;

        print(hp);

        if (hp < 0) { return Die(); } // Dead

        return false;
    }

    private bool Die()
    {
        onDeath.Invoke(this);
        gameObject.SetActive(false);
        return true;
    }
}
