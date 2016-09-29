using UnityEngine;
using System.Collections;

public class StatesFlee : States
{

    public StatesFlee(MonoBehaviour personnage) : base(personnage)
    {
        nom = "Flee";
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
