using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour {
    public Comportement comportement;
    public UnityEvent onEnemyNearby;
    public List<string> enemyTags;

    public int damage;
    public int hp;
    protected double movementSpeed;

    private static List<Personnage> inactivePersonnage = new List<Personnage>();

    public static void Spawn(GameObject character)
    {
        Personnage nouveauPersonnage;
        if (inactivePersonnage.Count == 0)
        {
            nouveauPersonnage = Instantiate(character.gameObject).GetComponent<Personnage>();
        }
        else
        {
            //nouveauPersonnage = inactivePersonnage[0];
            //inactivePersonnage.RemoveAt(0);
            //nouveauPersonnage.gameObject.SetActive(true);
        }
        //nouveauPersonnage.Awake();
    }

    void Awake ()
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
        gameObject.SetActive(false);
        return true;
    }
}
