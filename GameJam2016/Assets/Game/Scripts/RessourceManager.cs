using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RessourceManager : IUpdate {

    private List<Village> listeVillage;
    private List<Ligne> tableauRessource;

    public Transform masterVillage;


    public class Stats
    {
        public string nom;
        public int nombre;
    }

    public class Ligne
    {
        public Village theVillage;
        public List<Stats> listeStats;
    }

	void Start ()
    {
        for (int i = 0; i < masterVillage.childCount; i++)
        {
            listeVillage.Add(masterVillage.GetChild(i).GetComponent<Village>());
            listeVillage[i] = tableauRessource[i].theVillage;

            // Or
            tableauRessource[0].listeStats[0].nom = "Total";
            listeVillage[0].or = tableauRessource[i].listeStats[0].nombre;

            tableauRessource[0].listeStats[1].nom = "Production";
            listeVillage[0].productionOr = tableauRessource[i].listeStats[0].nombre;

            tableauRessource[i].listeStats[2].nom = "Taxe";
            // listeVillage[0].taxeOr = tableauRessource[i].listeStats[0].nombre;

            // Nourriture
            tableauRessource[0].listeStats[0].nom = "Total";
            listeVillage[0].or = tableauRessource[i].listeStats[0].nombre;

            tableauRessource[i].listeStats[1].nom = "Production";
            listeVillage[0].productionOr = tableauRessource[i].listeStats[0].nombre;

            tableauRessource[i].listeStats[2].nom = "Taxe";
            // listeVillage[0].taxeNourriture = tableauRessource[i].listeStats[0].nombre;
        }
    }
	
	public void Update ()
    {
        int i = 0;

        foreach (Village unVillage in listeVillage)
        {
        }

	}
}
