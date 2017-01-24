using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;
using System.Runtime.Serialization;

[System.Serializable]
public class Empire : INewDay
{
    [System.NonSerialized]
    static public int TEAM = 1; //est-ce que TEAM sera bien à 1 lors d'un reload ?

    #region Stats
    private Stat<int> science = new Stat<int>(0);
    private Stat<int> gold = new Stat<int>(0);
    private Stat<int> material = new Stat<int>(0);
    private Stat<int> armyCost = new Stat<int>(1);
    private Stat<int> armyHitRate = new Stat<int>(60);
    private Stat<int> citizenProgress = new Stat<int>(0, -1, 99, BoundMode.MaxLoop);
    private Stat<int> happiness = new Stat<int>(100, 0, 100, BoundMode.Cap);
    private Stat<int> reputation = new Stat<int>(0, -10, 10, BoundMode.Cap);
    #endregion

    private List<Village> villageList = new List<Village>();

    private CartsManager cartManager = null;

    //Events
    [System.NonSerialized]
    private Stat<int>.StatEvent onMaterialProdSet = new Stat<int>.StatEvent();
    [System.NonSerialized]
    private Stat<int>.StatEvent onScienceProdSet = new Stat<int>.StatEvent();
    [System.NonSerialized]
    private Stat<int>.StatEvent onFoodProdSet = new Stat<int>.StatEvent();
    [System.NonSerialized]
    private Stat<int>.StatEvent onGoldProdSet = new Stat<int>.StatEvent();

    //Fonction appelé lors des events
    private void OnMaterialProdSet(int input) { onMaterialProdSet.Invoke(GetCumulation(Village_ResourceType.materialProd)); }
    private void OnScienceProdSet(int input) { onScienceProdSet.Invoke(GetCumulation(Village_ResourceType.scienceProd)); }
    private void OnFoodProdSet(int input) { onFoodProdSet.Invoke(GetCumulation(Village_ResourceType.food)); }
    private void OnGoldProdSet(int input) { onGoldProdSet.Invoke(GetCumulation(Village_ResourceType.goldProd)); }

    //Appelé 1 seul fois PAR MONDE, au départ
    public Empire()
    {
        //à enlever
        BuildCity(2);
        BuildCity(4,true);
        BuildCity(5);

        cartManager = new CartsManager(6);
        
        SetListeners();
    }

    //Appelé à chaque recharge de sauvegarde
    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {
        SetListeners();
    }

    public void SetListeners()
    {
        citizenProgress.onMaxReached.AddListener(this.NewCitizen);
        foreach (Village village in villageList)
        {
            village.GetOnSetEvent(Village_ResourceType.food).AddListener(OnFoodProdSet);
            village.GetOnSetEvent(Village_ResourceType.scienceProd).AddListener(OnScienceProdSet);
            village.GetOnSetEvent(Village_ResourceType.materialProd).AddListener(OnMaterialProdSet);
            village.GetOnSetEvent(Village_ResourceType.goldProd).AddListener(OnGoldProdSet);
        }
    }

    // Empire avance d'une journee
    public virtual void NewDay()
    {
        UpdateResource();

        // autre chose ?
    }

    public void FirstDay()
    {
        UpdateResource();
    }

    private void UpdateResource()
    {
        // Pour chaque village
        for (int i = 0; i < villageList.Count; i++)
        {
            // Update les resources
            villageList[i].UpdateResource();
        }
    }

    void NewCitizen(int currentCitizenProgress)
    {
        // +1 citizen yay !
        // note: pas besoin de set la variable 'citizenProgress', elle devrais déja être ajusté
    }

    public void BuildCity(int position, bool capitale = false)
    {
        Universe.Map.ChangeTerritoryOwner(position, TEAM);
        villageList.Add(new Village(this, Universe.Map.PositionToRegionName(position), position, capitale));
    }

    public void DestroyCity(int position)
    {
        foreach (Village village in villageList)
        {
            if(village.GetMapPosition() == position)
            {
                village.Destroy();
                villageList.Remove(village);
                Universe.Map.ChangeTerritoryOwner(position, BarbareManager.TEAM);
            }
        }
    }

    public Village GetVillageAtPos(int mapPosition)
    {
        for (int i = 0; i < villageList.Count; i++)
        {
            if (villageList[i].GetMapPosition() == mapPosition) return villageList[i];
        }
        return null;
    }

    public Village Capitale()
    {
        for (int i = 0; i < villageList.Count; i++)
        {
            if (villageList[i].IsCapital()) return villageList[i];
        }
        return null;
    }

    public CartsManager CartsManager
    {
        get
        {
            return cartManager;
        }
    }

    public bool Has(Village village)
    {
        return villageList.Contains(village);
    }

    public Village GetVillageByName(string name)
    {
        foreach (Village village in villageList)
        {
            if (village.Name == name) return village;
        }
        return null;
    }

    public List<Village> VillageList
    {
        get { return villageList; }
    }

    #region Stats Method

    public Stat<int> GetStat(Empire_ResourceType type)
    {
        switch (type)
        {
            default:
                return null;
            case Empire_ResourceType.science:
                return science;
            case Empire_ResourceType.gold:
                return gold;
            case Empire_ResourceType.material:
                return material;
            case Empire_ResourceType.citizenProgress:
                return citizenProgress;
            case Empire_ResourceType.happiness:
                return happiness;
            case Empire_ResourceType.reputation:
                return reputation;
            case Empire_ResourceType.armyCost:
                return armyCost;
            case Empire_ResourceType.armyHitRate:
                return armyHitRate;
        }
    }

    public int Get(Empire_ResourceType type)
    {
        Stat<int> stat = GetStat(type);
        return stat != null ? stat : 0;
    }

    public int GetCumulation(Village_ResourceType type)
    {
        int value = 0;
        foreach (Village village in villageList)
        {
            value += village.Get(type);
        }
        return value;
    }

    public Stat<int>.StatEvent GetCumulationEvent(Village_ResourceType type)
    {
        switch (type)
        {
            case Village_ResourceType.scienceProd:
                return onScienceProdSet;
            case Village_ResourceType.goldProd:
                return onGoldProdSet;
            case Village_ResourceType.materialProd:
                return onMaterialProdSet;
            case Village_ResourceType.food:
                return onFoodProdSet;
            default:
                return null;
        }
    }

    public void Set(Empire_ResourceType type, int value)
    {
        Stat<int> stat = GetStat(type);
        if (stat != null)
            stat.Set(value);
    }

    public void Add(Empire_ResourceType type, int value)
    {
        Set(type, Get(type) + value);
    }

    public Stat<int>.StatEvent GetOnSetEvent(Empire_ResourceType type)
    {
        Stat<int> stat = GetStat(type);
        return stat != null ? stat.onSet : null;
    }
    #endregion
}

/* ANCIEN SYSTEME!!!

public static Empire instance;

private int nbVillage = 5;

List<string> nomvillage = new List<string>{ "Mediolanum", "Cremona", "Aquileia", "Neopolis", "Tarentum", "HISPALIS", "CHRISTINEA", "LUTETIA", "PARTISCUM", "MONAECUM", "AMSTELODAMUM", "EBURACUM" };
List<string> nomseigneur = new List<string> { "Maximus", "Tullus", "Lucius", "Marcus", "Valentinus", "Decimus ", "Caeso", "Septimus", "Sextus", "Tiberius", "Faustus", "Octavius" };

public List<Village> listVillage = new List<Village>();
public Capitale capitale;
public VillageMap map;

public int valeurNouriture = 2;
public int valeurOr = 1;
public int valeurSoldat = 4;

public void Start ()
{
    instance = this;
    for (int i = 0; i < nbVillage; i++)
    {
        listVillage.Add(new Village(this,i+1, nomvillage[i], nomseigneur[i])); // le village numero 0 correspond a listVillage[0]
    }
    capitale = new Capitale(this,0);

    map = new VillageMap(capitale, listVillage.ToArray());
}

public void NewDay ()
{
    for(int i = 0; i < listVillage.Count; i++)
    {
        Village ancienVillage = listVillage[i];
        listVillage[i].NewDay();
        if (ancienVillage.isDestroyed)
        {
            i = i - 2;
            Debug.Log("test");
        }
    }
    capitale.NewDay();
}

public void DeleteVillage(Village destroyedVillage)
{
    listVillage.Remove(destroyedVillage);
    map.removeVillage(destroyedVillage);
}

public Village GetVillageByName(string name)
{
    foreach(Village village in listVillage)
    {
        if (village.nom == name) return village;
    }
    return null;
}
}

*/
