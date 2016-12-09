using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class Empire : INewDay {

    public static Empire instance;

    public int nbVillage = 5;

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
