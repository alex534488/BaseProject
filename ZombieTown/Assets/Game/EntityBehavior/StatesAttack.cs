using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StatesAttack : States
{
    public Personnage target;
    public UnityEvent onEnemyKilled;

    public StatesAttack(Personnage personnage) : base(personnage)
    {
        nom = "Attack";
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (target.LoseHP(personnage.damage))
        {
            onEnemyKilled.Invoke();
        }
    }

    public override void Exit()
    {
        onEnemyKilled.RemoveAllListeners();
    }

    public void SetTarget(Personnage personnage)
    {
        target = personnage;
    }
}