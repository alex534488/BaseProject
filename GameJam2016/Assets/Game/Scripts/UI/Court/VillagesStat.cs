using UnityEngine;
using System.Collections;
using CCC.Utility;

public class VillagesStat : BaseCourtStat
{
    public Village_ResourceType type = Village_ResourceType.custom;

    protected override void Init()
    {
        base.Init();

        textColor = Color.white; //GameResources.GetBrightColor(GameResources.Convert(type));

        //update 'value'
        value = empire.GetCumulation(type);

        //set le listener
        Stat<int>.StatEvent onSet = empire.GetCumulationEvent(type);
        if (onSet != null)
            onSet.AddListener(OnStatSet);

        //update le display
        UpdateDisplay();
    }

    protected override void OnNewDay()
    {
        base.OnNewDay();
        OnStatSet(empire.GetCumulation(type));
    }
}
