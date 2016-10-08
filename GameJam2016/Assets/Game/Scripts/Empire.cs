using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class Empire : IUpdate {

    public int nbVillage = 12;

    List<string> nomvillage = new List<string>{ "BREMA", "GOSLARIA", "TREMONIA", "BRUXELLAE", "HAUNIAE", "HISPALIS", "CHRISTINEA", "LUTETIA", "PARTISCUM", "MONAECUM", "AMSTELODAMUM", "EBURACUM" };
    List<string> nomseigneur = new List<string> { "Maximus", "Tullus", "Lucius", "Marcus", "Valentinus", "Decimus ", "Caeso", "Septimus", "Sextus", "Tiberius", "Faustus", "Octavius" };

    public List<Village> listVillage = new List<Village>();
    public Capitale capitale;
    public VillageMap map;

	public void Start ()
    {
        for (int i = 0; i < nbVillage; i++)
        {
            listVillage.Add(new Village(this,i, nomvillage[i], nomseigneur[i])); // le village numero 0 correspond a listVillage[0]
        }
        capitale = new Capitale(this,0);

        map = new VillageMap(capitale, listVillage.ToArray());
	}
	
	public void Update ()
    {
        foreach (Village village in listVillage)
        {
            village.Update();
        }
        capitale.Update();
    }

    public void DeleteVillage(Village destroyedVillage)
    {
        listVillage.Remove(destroyedVillage);
        map.removeVillage(destroyedVillage);
    }
}
