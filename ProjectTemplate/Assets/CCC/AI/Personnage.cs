﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Personnage : MonoBehaviour {

    // Classes Externes
    public class PersonnageEvent : UnityEvent<Personnage> { }

    // Evennements
    public PersonnageEvent onDeath = new PersonnageEvent();
    
    // Detecteur
    public Detector detector;

    // Attributs
    public int damage = 1;
    public int hp = 10;

    // Apparence (Prefab)
    public GameObject sprite;

    // Etats
    private List<States> states = new List<States>();
    public States currentStates;

    // Enemy
    public List<string> enemyTags;

    ///////////////////////

    protected virtual void Awake()
    {
        // Ajout de tous les etats possibles
        states.Add(new StatesIdle(this));
        ChangeState(0);
    }

    protected virtual void Update()
    {
        currentStates.Update();
    }

    ///////////////////////

    public States ChangeState(States newState)
    {
        if (currentStates != null) currentStates.Exit();
        currentStates = newState;
        newState.Enter();
        return currentStates;
    }

    public States ChangeState(int index)
    {
        return ChangeState(states[index]);
    }

    public States ChangeState<T>() where T : States
    {
        return ChangeState(GetStatesByType<T>());
    }

    public States ChangeState(string name)
    {
        return ChangeState(GetStatesByName(name));
    }

    public States GetStatesByName(string name)
    {
        foreach (States etat in states)
        {
            if (etat.getNom() == name)
            {
                return etat;
            }
        }
        Debug.LogError("Erreur 01: Incapable de trouver le States " + name);
        return null;
    }

    public States GetStatesByType<T>() where T : States
    {
        foreach (States etat in states)
        {
            if (etat is T)
            {
                return etat;
            }
        }
        Debug.LogError("Erreur 01: Incapable de trouver le States ");
        return null;
    }

    ///////////////////////

    // Ajouter fonctions specifique aux attributs ou aux personnages
}
