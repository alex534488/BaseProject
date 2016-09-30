using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comportement{
    private List<States> states = new List<States>();
    public States currentStates;

    public Comportement(Personnage personnage)
    {
        states.Add(new StatesIdle(personnage));
        states.Add(new StatesMoveTo(personnage));
        states.Add(new StatesFollow(personnage));
        states.Add(new StatesAttack(personnage));
    }

    void Start () {
        ChangeState(0);
    }
	
	public void Update ()
    {
        currentStates.Update();
	}

    public void ChangeState(States newState)
    {
        if(currentStates != null) currentStates.Exit();
        currentStates = newState;
        newState.Enter();
    }

    public void ChangeState(int index)
    {
        ChangeState(states[index]);
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
