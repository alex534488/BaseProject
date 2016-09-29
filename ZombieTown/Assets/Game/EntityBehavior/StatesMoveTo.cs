using UnityEngine;
using System.Collections;

public class StatesMoveTo : States
{

    public StatesMoveTo(Personnage personnage) : base(personnage)
    {
        nom = "MoveTo";
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
