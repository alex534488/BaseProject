using UnityEngine;
using System.Collections;

public class StatesIdle : States {

    public StatesIdle(Personnage personnage) : base(personnage)
    {
        nom = "Idle";
        this.personnage = personnage;
    }

    public override void Enter()
    {
        Stop();
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
