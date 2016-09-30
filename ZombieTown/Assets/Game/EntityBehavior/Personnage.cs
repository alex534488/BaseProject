﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour
{
    public Comportement comportement;
    public UnityEvent onEnemyNearby;
    public List<string> enemyTags;

    public int damage;
    public int hp;
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

        if (CheckEnemyNearby())
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

        if (hp < 0)
        {
            Die();
            return true;
        } // Dead

        return false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
