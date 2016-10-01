using UnityEngine;
using System.Collections;

public class StatesFollow : States
{
    Vector3 position; // Position du zombie dans l'arme qui suit le chef

    public StatesFollow(Personnage personnage) : base(personnage)
    {
        nom = "Follow";
        this.personnage = personnage;
    }

    public void Init(Personnage target)
    {
        this.target = target;
        MoveTo(target.transform.position); // Bouge jusqu'a une distance du chef
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        // Check if chief is too far
            // if yes Move to correct position

    }

    public override void Exit()
    {

    }
}
