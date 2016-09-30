using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using CCC.Manager;

public class StatesAttack : States
{
    public UnityEvent onEnemyKilled;
    public UnityEvent onHittingTarget;
    private float cooldown = 0;
    private int skipupdate = 6;
    private int counter = 0;

    public StatesAttack(Personnage personnage) : base(personnage)
    {
        nom = "Attack";
        this.personnage = personnage;
    }

    public void Init(Personnage target)
    {
        this.target = target;
        if (Vector3.Distance(target.transform.position, personnage.gameObject.transform.position) > personnage.range)
        {
            MoveTo(target.transform.position);
        }
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        counter++;
        if (counter <= skipupdate) return;

        counter = 0;

        if (Vector3.Distance(target.transform.position, personnage.gameObject.transform.position) <= personnage.range)
        {
            Stop();
            if (cooldown >= 1) { Hit(); cooldown = 0; }
        }
        cooldown += Time.deltaTime * skipupdate;
    }

    public override void Exit()
    {
        onEnemyKilled.RemoveAllListeners();
    }

    public void Hit()
    {
        onHittingTarget.Invoke();
        if (target.hp <= 0) // Only works on zombie
        {
            onEnemyKilled.Invoke();
        }
    }
}