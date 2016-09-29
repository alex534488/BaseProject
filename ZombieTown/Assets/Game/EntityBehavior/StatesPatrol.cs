using UnityEngine;
using System.Collections;

public class StatesPatrol : States
{

    public StatesPatrol(MonoBehaviour personnage) : base(personnage)
    {
        nom = "Patrol";
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
