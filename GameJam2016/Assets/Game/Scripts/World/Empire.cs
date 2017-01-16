using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public class Empire : INewDay
{
    #region Stats
    private Stat<int> science = new Stat<int>(0);
    private Stat<int> gold = new Stat<int>(0);
    private Stat<int> material = new Stat<int>(0);
    private Stat<int> citizenProgress = new Stat<int>(0,0, 10, Stat<int>.BoundMode.MaxLoop);
    private Stat<int> happiness = new Stat<int>(100,0,100, Stat<int>.BoundMode.Cap);
    private Stat<int> reputation = new Stat<int>(0,-10,10, Stat<int>.BoundMode.Cap);
    #endregion

    private List<Village> listVillage = new List<Village>();

    private CartsManager cartManager;

    // Initialisation de l'empire
    public void Start()
    {
        citizenProgress.onMaxReached.AddListener(this.NewCitizen);
    }

    // Empire avance d'une journee
    public virtual void NewDay()
    {
        UpdateResource();
        
        // autre chose ?
    }

    private void UpdateResource()
    {
        // Pour chaque village
        for (int i = 0; i < listVillage.Count; i++)
        {
            // Update les resources
            listVillage[i].UpdateResource(this);
        }
    }

    void NewCitizen(int currentCitizenProgress)
    {
        // +1 citizen yay !
        // note: pas besoin de set la variable 'citizenProgress', elle devrais déja être ajusté
    }

    public Village GetVillageAtPos(int mapPosition)
    {
        for(int i = 0; i < listVillage.Count; i++)
        {
            if (listVillage[i].GetMapPosition() == mapPosition) return listVillage[i];
        }
        return null;
    }

    public Village GetCapitale()
    {
        for (int i = 0; i < listVillage.Count; i++)
        {
            if (listVillage[i].IsCapital()) return listVillage[i];
        }
        return null;
    }

    public CartsManager GetCartsManager()
    {
        return cartManager;
    }

    public bool Has(Village village)
    {
        return listVillage.Contains(village);
    }

    public Village GetVillageByName(string name)
    {
        foreach (Village village in listVillage)
        {
            if (village.Name == name) return village;
        }
        return null;
    }

    #region Stats Method

    private Stat<int> GetStat(Empire_ResourceType type)
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
        }
    }

    public int Get(Empire_ResourceType type)
    {
        Stat<int> stat = GetStat(type);
        return stat != null ? stat : 0;
    }

    public void Set(Empire_ResourceType type, int value)
    {
        Stat<int> stat = GetStat(type);
        if(stat != null)
            stat.Set(value);
    }

    public void Add(Empire_ResourceType type, int value)
    {
        Set(type, Get(type) + value);
    }

    public virtual Stat<int>.StatEvent GetOnSetEvent(Empire_ResourceType type)
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
