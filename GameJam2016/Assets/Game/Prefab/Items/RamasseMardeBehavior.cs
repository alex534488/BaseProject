using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RamasseMardeBehavior : ItemBehavior {
    int modifiant = 15;


    public override void OnUse()
    {
        base.OnUse();
        Universe.Empire.Add(Empire_ResourceType.citizenProgress, 100);
    }

    public string PrintMarde()
    {
        return "TEST";
    }
}
