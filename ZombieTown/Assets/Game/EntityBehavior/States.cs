using UnityEngine;
using System.Collections;

public abstract class States{

    protected string nom;
    protected Personnage personnage;
    public Personnage target;

    public States(Personnage personnage)
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

    public void MoveTo(Vector3 pos)
    {
        personnage.GetComponent<MyAIPath>().SetTarget(pos);
    }

    public void Stop()
    {
        personnage.GetComponent<MyAIPath>().Stop();
    }

    public void LookAt()
    {

    }
}
