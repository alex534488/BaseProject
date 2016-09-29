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

    public void SetTarget(Vector3 target)
    {
        MoveTo(target);
    }

    public override void Update()
    {
        // Idle, fait rien
    }

    public override void Exit()
    {

    }
}
