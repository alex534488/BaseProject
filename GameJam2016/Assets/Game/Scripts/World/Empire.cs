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
        UpdateRessource();
        
    // autre chose ?
}

    private void UpdateRessource()
    {
        // Pour chaque village
        for (int i = 0; i < listVillage.Count; i++)
        {
            // Si c'est une capitale
            if (listVillage[i].IsCapital())
            {
                Add(Resource_Type.science, listVillage[i].Get(Resource_Type.scienceProd));
            } else // Sinon
            {
                Add(Resource_Type.material, listVillage[i].Get(Resource_Type.materialProd));
                // Verification du compteur de creation de citoyen, s'il est superieur a son max, on doit generer un
                // evennement et redemarer le compteur
                if ((citizenProgress + listVillage[i].Get(Resource_Type.food)) >= (int)citizenProgress.MAX)
                {
                    int temp = citizenProgress + listVillage[i].Get(Resource_Type.food);
                    temp = temp - (int)citizenProgress.MAX;
                    citizenProgress.Set(temp);
                    // Evenement de production d'un citoyen ICI !
                }
            }
            Add(Resource_Type.gold, listVillage[i].Get(Resource_Type.goldProd));
        }
    }

    /* ?
    public Village GetVillage(int mapPosition)
    {
        // probleme entre une liste de village et une array de territoire
    }
    */

    #region Stats Method
    public int Get(Resource_Type type)
    {
        switch (type)
        {
            default:
                return 0;
            case Resource_Type.science:
                return science;
            case Resource_Type.gold:
                return gold;
            case Resource_Type.material:
                return material;
            case Resource_Type.citizenProgress:
                return citizenProgress;
            case Resource_Type.happiness:
                return happiness;
            case Resource_Type.reputation:
                return reputation;
        }
    }

    public void Set(Resource_Type type, int value)
    {
        switch (type)
        {
            default:
                return;
            case Resource_Type.science:
                science.Set(value);
                return;
            case Resource_Type.gold:
                gold.Set(value);
                return;
            case Resource_Type.material:
                material.Set(value);
                return;
            case Resource_Type.citizenProgress:
                citizenProgress.Set(value);
                return;
            case Resource_Type.happiness:
                happiness.Set(value);
                return;
            case Resource_Type.reputation:
                reputation.Set(value);
                return;
        }
    }

    public void Add(Resource_Type type, int value)
    {
        switch (type)
        {
            default:
                return;
            case Resource_Type.science:
                science.Set(science + value);
                return;
            case Resource_Type.gold:
                gold.Set(gold + value);
                return;
            case Resource_Type.material:
                material.Set(material + value);
                return;
            case Resource_Type.citizenProgress:
                citizenProgress.Set(citizenProgress + value);
                return;
            case Resource_Type.happiness:
                happiness.Set(happiness + value);
                return;
            case Resource_Type.reputation:
                reputation.Set(reputation + value);
                return;
        }
    }

    public virtual Stat<int>.StatEvent GetOnSet(Resource_Type type)
    {
        switch (type)
        {
            default:
                return null;
            case Resource_Type.science:
                return science.onSet;
            case Resource_Type.gold:
                return gold.onSet;
            case Resource_Type.material:
                return material.onSet;
            case Resource_Type.citizenProgress:
                return citizenProgress.onSet;
            case Resource_Type.happiness:
                return happiness.onSet;
            case Resource_Type.reputation:
                return reputation.onSet;
        }
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
