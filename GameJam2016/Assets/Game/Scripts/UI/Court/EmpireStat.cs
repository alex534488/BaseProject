using UnityEngine;
using System.Collections;
using CCC.Utility;

public class EmpireStat : BaseCourtStat
{
    public Empire_ResourceType type = Empire_ResourceType.custom;
    
    protected override void LinkToEmpire()
    {
        base.LinkToEmpire();

        //update 'value'
        value = empire.Get(type);

        //set le listener
        Stat<int>.StatEvent onSet = empire.GetOnSetEvent(type);
        if (onSet != null)
            onSet.AddListener(OnStatSet);

        //update le display
        UpdateDisplay();
    }

    protected override void OnNewDay()
    {
        base.OnNewDay();
        OnStatSet(empire.Get(type));
    }
}
