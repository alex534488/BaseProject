using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatesIdle : States {

    public StatesIdle(Personnage personnage) : base(personnage)
    {
        nom = "Idle";
        this.personnage = personnage;
    }

    public override void Enter()
    {
        Stop();
        personnage.OnIdle();
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
