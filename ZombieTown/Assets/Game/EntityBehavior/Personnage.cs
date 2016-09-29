using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour {
    public Comportement comportement;
    public UnityEvent onEnemyNearby;
    public List<string> enemyTags;

    public int damage;
    protected int hp;
    protected double movementSpeed;

    void Start ()
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

        if (hp < 0) { return Die(); } // Dead

        return false;
    }

    private bool Die()
    {
        // DO: Faire disparaitre le personnage
        return true;
    }
}
