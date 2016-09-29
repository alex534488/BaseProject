using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour {
    public Comportement comportement;
    public UnityEvent onEnemyNearby;
    public List<string> enemyTags;

    protected int damage;
    protected int hp;
    protected double movementSpeed;

    void Start ()
    {
        comportement = new Comportement(this);
    }
	
	void Update ()
    {
        comportement.Update();

        if(CheckEnemyNearby() == true)
        {
            onEnemyNearby.Invoke();
        }
    }

    private bool CheckEnemyNearby()
    {
        // DO: verifie la zone si les enemy tags sont la
        return true;
    }
}
