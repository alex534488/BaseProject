using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comportement{
    private List<States> states;
    public States currentStates;

    public Comportement(Personnage personnage)
    {
        states.Add(new StatesIdle(personnage));
    }

    // Use this for initialization
    void Start () {
        currentStates = states[0]; // au debut, un personnage a un comportement Idle
    }
	
	public void Update ()
    {
        currentStates.Update();
	}

    public void ChangeState(States newState)
    {
        currentStates.Exit();
        currentStates = newState;
        newState.Enter();
    }

    public void ChangeState<T>() where T : States
    {
        ChangeState(GetStatesByType<T>());
    }

    public void ChangeState(string name)
    {
        ChangeState(GetStatesByName(name));
    }

    public States GetStatesByName(string name)
    {
        foreach (States etat in states) {
            if(etat.getNom() == name)
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

}
