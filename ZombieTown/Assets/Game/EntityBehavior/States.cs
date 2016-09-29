﻿using UnityEngine;
using System.Collections;

public abstract class States{

    protected string nom;
    protected Personnage personnage;

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
        // DO: deplacement du personnage a la nouvelle possition pos
    }
}
