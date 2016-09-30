using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using CCC.Manager;

public class StatesAttack : States
{
    public UnityEvent onEnemyKilled;
    public UnityEvent onHittingTarget;
    private int cooldown = 0;

    public StatesAttack(Personnage personnage) : base(personnage)
    {
        nom = "Attack";
        this.personnage = personnage;
    }

    public void Init(Personnage target)
    {
        this.target = target;

    }

    public override void Enter()
    {
        if(Vector3.Distance(target.transform.position,personnage.gameObject.transform.position) > personnage.range)
        {
            MoveTo(target.transform.position);
        }
    }

    public override void Update()
    {
        cooldown++;
        if (cooldown >= 60){ Hit(); cooldown = 0; }

        if (Vector3.Distance(target.transform.position, personnage.gameObject.transform.position) < personnage.range)
        {
            Stop();
        }
    }

    public override void Exit()
    {
        onEnemyKilled.RemoveAllListeners();
    }

    public void Hit()
    {
        onHittingTarget.Invoke();
        if (target.hp <= 0)
        {
            onEnemyKilled.Invoke();
        }
    }
}