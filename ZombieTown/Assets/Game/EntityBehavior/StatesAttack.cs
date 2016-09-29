using UnityEngine;
using System.Collections;

public class StatesAttack : States
{

    public StatesAttack(MonoBehaviour personnage) : base(personnage)
    {
        nom = "Attack";
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