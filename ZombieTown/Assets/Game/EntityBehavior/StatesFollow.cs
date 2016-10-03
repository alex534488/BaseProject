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

        offset = GetOffset(nb);

        pos = target.transform.position + offset;
    }

    static Vector3 GetOffset(int nb)
    {
        int etage = GetEtage(nb);
        float distance = etage * 5;
        float x = 0, z = 1;
        float newX, newZ;
        float angle;

        int membreSurEtage = etage * 6;
        angle = (nb % membreSurEtage) * (Mathf.PI * 2 / membreSurEtage);

        float randomness = 0.15f;

        angle *= Random.Range(1-randomness, 1 + randomness);
        distance *= Random.Range(1 - randomness, 1 + randomness);

        newX = x * Mathf.Cos(angle) - z * Mathf.Sin(angle);
        newZ = x * Mathf.Sin(angle) + z * Mathf.Cos(angle);

        return new Vector3(newX, 0, newZ) *distance;
    }

    static int GetEtage(int nb)
    {
        int nm = 0;
        for(int i=1; i<10000; i++)
        {
            nm += i * 6;
            if (nm > nb) return i;
        }
        return 10000;
    }
}

//personne total : (6 * n) + !
