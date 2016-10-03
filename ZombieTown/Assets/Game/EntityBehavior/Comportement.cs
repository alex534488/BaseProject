using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Comportement{
    private List<States> states = new List<States>();
    public States currentStates;

    public Comportement(Personnage personnage)
    {
        states.Add(new StatesIdle(personnage));
        states.Add(new StatesMoveTo(personnage));
        states.Add(new StatesFollow(personnage));
        states.Add(new StatesAttack(personnage));
        ChangeState(0);
    }
	
	public void Update ()
    {
        currentStates.Update();
	}

    public States ChangeState(States newState)
    {
        Debug.Log("Enter state: " + newState.getNom());
        if(currentStates != null) currentStates.Exit();
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
