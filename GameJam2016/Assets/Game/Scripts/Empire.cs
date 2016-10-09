using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class Empire : IUpdate {

    public static Empire instance;

    public int nbVillage = 12;

    List<string> nomvillage = new List<string>{ "BREMA", "GOSLARIA", "TREMONIA", "BRUXELLAE", "HAUNIAE", "HISPALIS", "CHRISTINEA", "LUTETIA", "PARTISCUM", "MONAECUM", "AMSTELODAMUM", "EBURACUM" };
    List<string> nomseigneur = new List<string> { "Maximus", "Tullus", "Lucius", "Marcus", "Valentinus", "Decimus ", "Caeso", "Septimus", "Sextus", "Tiberius", "Faustus", "Octavius" };

    public List<Village> listVillage = new List<Village>();
    public Capitale capitale;
    public VillageMap map;

    public int valeurNouriture = 2;
    public int valeurOr = 1;
    public int valeurSoldat = 4;

	public void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < nbVillage; i++)
        {
            listVillage.Add(new Village(this,i, nomvillage[i], nomseigneur[i])); // le village numero 0 correspond a listVillage[0]
        }
        capitale = new Capitale(this,0);

        map = new VillageMap(capitale, listVillage.ToArray());
	}
	
	public void Update ()
    {
        for(int i = 0; i < listVillage.Count; i++)
        {
            Village ancienVillage = listVillage[i];
            listVillage[i].Update();
            if (ancienVillage.isDestroyed)
            {
                i = i - 2;
                Debug.Log("test");
            }
        }
        capitale.Update();
    }

    public void DeleteVillage(Village destroyedVillage)
    {
        listVillage.Remove(destroyedVillage);
        map.removeVillage(destroyedVillage);
    }
}
