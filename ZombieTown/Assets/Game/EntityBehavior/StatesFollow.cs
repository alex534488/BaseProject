using UnityEngine;
using System.Collections;
using CCC.Utility;

[System.Serializable]
public class StatesFollow : States
{
    Vector3 pos; // Position du zombie dans la horde qui suit le chef
    Vector3 offset;

    public StatesFollow(Personnage personnage) : base(personnage)
    {
        nom = "Follow";
        this.personnage = personnage;
    }

    public void Init(Personnage target)
    {
        this.target = target;
        this.target.onFollowerListChange.AddListener(SetOffset);
        SetOffset(target);
    }

    public override void Enter()
    {
        MoveTo(pos);
    }

    public override void Update()
    {
        pos = target.transform.position + offset;

        MoveTo(pos);
    }

    public override void Exit()
    {

    }

    public void SetOffset(Personnage chief)
    {
        int nb = chief.listFollower.IndexOf(personnage);

        offset = UnitFormation.GetCircularOffset(nb);

        pos = target.transform.position + offset;
    }
}
