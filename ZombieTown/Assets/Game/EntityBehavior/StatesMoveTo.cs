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
        this.target = null;
    }

    public void Init(Vector3 target)
    {
        MoveTo(target);
        personnage.GetComponent<MyAIPath>().onTargetReached.AddListener(OnTargetReached);
    }

    void OnTargetReached()
    {
        personnage.GetComponent<MyAIPath>().onTargetReached.RemoveListener(OnTargetReached);
        personnage.comportement.ChangeState<StatesIdle>();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        
    }
}
