using UnityEngine;
using System.Collections;

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

    public void Init(Personnage target, int nb)
    {
        this.target = target;
        this.target.onFollowerChange.AddListener(SetOffset);
        SetOffset();
    }

    public override void Enter()
    {
        MoveTo(pos);
    }

    public override void Update()
    {
        pos = this.target.gameObject.transform.position + offset;

        MoveTo(pos);
    }

    public override void Exit()
    {

    }

    public void SetOffset()
    {
        int nb = 0; // **** A changer

        offset = GetOffset(nb);

        pos = this.target.gameObject.transform.position + offset;
    }

    static Vector3 GetOffset(int nb)
    {
        int etage = nb / 6;
        float distance = etage * 2 + 2;
        float x = 0, z = 1;
        float angle;

        angle = (nb % 6) * (Mathf.PI * 2 / 6);

        x = x * Mathf.Cos(angle) - z * Mathf.Sin(angle);
        z = x * Mathf.Sin(angle) + z * Mathf.Cos(angle);

        return new Vector3(x,0,z)*distance;
    }
}
