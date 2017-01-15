using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public class Empire : INewDay
{
    public static Empire instance;

    #region Stats
    private Stat<int> science = new Stat<int>(0);
    private Stat<int> gold = new Stat<int>(0);
    private Stat<int> material = new Stat<int>(0);
    private Stat<int> citizenProgress = new Stat<int>(0,0, int.MaxValue);
    private Stat<int> citizenProgressMax = new Stat<int>(10);
    private Stat<int> happiness = new Stat<int>(100,0,100);
    private Stat<int> reputation = new Stat<int>(0,-10,10);
    #endregion

    private List<Village> listVillage = new List<Village>();

    private CartsManager cartManager;

    // Initialisation de l'empire
    public void Start()
    {
        instance = this;
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

    public Village GetVillage(int mapPosition)
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

    #region Stats Method
    public int Get(Empire_ResourceType type)
    {
        switch (type)
        {
            default:
                return 0;
            case Empire_ResourceType.science:
                return science;
            case Empire_ResourceType.gold:
                return gold;
            case Empire_ResourceType.material:
                return material;
            case Empire_ResourceType.citizenProgress:
                return citizenProgress;
            case Empire_ResourceType.citizenProgressMax:
                return citizenProgressMax;
            case Empire_ResourceType.happiness:
                return happiness;
            case Empire_ResourceType.reputation:
                return reputation;
        }
    }

    public void Set(Empire_ResourceType type, int value)
    {
        switch (type)
        {
            default:
                return;
            case Empire_ResourceType.science:
                science.Set(value);
                return;
            case Empire_ResourceType.gold:
                gold.Set(value);
                return;
            case Empire_ResourceType.material:
                material.Set(value);
                return;
            case Empire_ResourceType.citizenProgress:
                citizenProgress.Set(value);
                return;
            case Empire_ResourceType.citizenProgressMax:
                citizenProgressMax.Set(value);
                return;
            case Empire_ResourceType.happiness:
                happiness.Set(value);
                return;
            case Empire_ResourceType.reputation:
                reputation.Set(value);
                return;
        }
    }

    public void Add(Empire_ResourceType type, int value)
    {
        Set(type, Get(type) + value);
    }

    public virtual Stat<int>.StatEvent GetOnSet(ResourceType type)
    {
        switch (type)
        {
            default:
                return null;
            case ResourceType.science:
                return science.onSet;
            case ResourceType.gold:
                return gold.onSet;
            case ResourceType.material:
                return material.onSet;
            case ResourceType.citizenProgress:
                return citizenProgress.onSet;
            case ResourceType.citizenProgressMax:
                return citizenProgressMax.onSet;
            case ResourceType.happiness:
                return happiness.onSet;
            case ResourceType.reputation:
                return reputation.onSet;
        }
    }
    #endregion

    public static void Transfer(Village source, Village destinataire, ResourceType resource, int amount)
    {
        if (amount < 0) //Swap les deux village si le montant est negatif
        {
            Village temp = source;
            source = destinataire;
            destinataire = temp;
        }

        GiveToVillage(source, resource, -amount);
        GiveToVillage(destinataire, resource, amount);
    }

    public static void GiveToVillage(Village village, ResourceType resource, int amount)
    {
        if (village != null) village.Add(resource, amount);
    }
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
