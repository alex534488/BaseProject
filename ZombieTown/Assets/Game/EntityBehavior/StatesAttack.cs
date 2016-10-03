using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using CCC.Manager;

[System.Serializable]
public class StatesAttack : States
{
    public UnityEvent onLauchingAttack = new UnityEvent();
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
        LookAt(target.transform); // S'assure que le sprite regarde vers la cible
        if (Vector3.Distance(target.transform.position, personnage.gameObject.transform.position) > personnage.attackRange)
        {
            MoveTo(target.transform.position);
        }
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if(target == null) { personnage.comportement.ChangeState<StatesIdle>(); }

        counter++;
        if (counter <= skipupdate) return;

        counter = 0;

        if (Vector3.Distance(target.transform.position, personnage.gameObject.transform.position) <= personnage.attackRange)
        {
            Stop();
            if (cooldown >= 1) { Hit(); cooldown = 0; }
        }
        cooldown += Time.deltaTime * skipupdate;
    }

    public override void Exit()
    {
        onLauchingAttack.RemoveAllListeners();
    }

    public void Hit()
    {
        onLauchingAttack.Invoke();
    }
}