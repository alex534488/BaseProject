﻿using UnityEngine;
using System.Collections;

public class StatesFollow : States
{

    public StatesFollow(Personnage personnage) : base(personnage)
    {
        nom = "Follow";
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        // Idle, fait rien
    }

    public override void Exit()
    {

    }
}