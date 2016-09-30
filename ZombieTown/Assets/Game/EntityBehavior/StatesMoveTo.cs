using UnityEngine;
using System.Collections;

public class StatesMoveTo : States
{
    public StatesMoveTo(Personnage personnage) : base(personnage)
    {
        nom = "MoveTo";
        this.personnage = personnage;
    }

    public override void Enter()
    {

    }

    public void SetTarget(Personnage target)
    {
        this.target = target;
        MoveTo(target.transform.position);
    }

    public override void Update()
    {
        if(personnage.gameObject.transform.position == target.transform.position)
        {
            personnage.comportement.ChangeState<StatesIdle>();
        }
    }

    public override void Exit()
    {
        
    }
}
