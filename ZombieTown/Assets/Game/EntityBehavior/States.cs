using UnityEngine;
using System.Collections;

public abstract class States{

    protected string nom;
    protected MonoBehaviour personnage;

    public States(MonoBehaviour personnage)
    {
        this.personnage = personnage;
    }

    public string getNom()
    {
        return nom;
    }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();

    protected void MoveTo(Vector2 pos)
    {
        // DO: deplacement du personnage a la nouvelle possition pos
    }
}
