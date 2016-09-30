using UnityEngine;
using System.Collections;

public abstract class States
{

    protected string nom;
    protected Personnage personnage;
    protected MyAIPath aiPath;
    public Personnage target;

    public States(Personnage personnage)
    {
        this.personnage = personnage;
        aiPath = personnage.GetComponent<MyAIPath>();
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
        if (aiPath != null) aiPath.SetTarget(pos);
    }

    public void Stop()
    {
        if (aiPath != null) aiPath.Stop();
    }

    public void LookAt(Transform target)
    {
        if (aiPath != null) aiPath.lookTarget = target;
    }

    public void StopLooking()
    {
        if (aiPath != null) aiPath.lookTarget = null;
    }
}
