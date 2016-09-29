using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using CCC.Manager;

public class StatesAttack : States
{
    public Personnage target;
    public UnityEvent onEnemyKilled;
    public UnityEvent onHittingTarget;
    public float range;
    private int cooldown;

    public StatesAttack(Personnage personnage) : base(personnage)
    {
        nom = "Attack";
    }

    public void Init(Personnage target,float range, int cooldown)
    {
        this.target = target;
        this.range = range;
        this.cooldown = cooldown;
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        cooldown++;
        if (cooldown >= 60){ Hit(); cooldown = 0; }
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