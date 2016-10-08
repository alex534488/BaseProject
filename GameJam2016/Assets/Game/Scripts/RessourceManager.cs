using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RessourceManager : IUpdate {

    private List<Village> listeVillage;
    private List<Ligne> tableauRessource;

    public Transform masterVillage;

    public enum Ressource_Type
    {
        gold, food, army, happiness
    }

    public struct Stats
    {
        public string nom;
        public int nombre;
    }

    public struct Ligne
    {
        public string nomVille;
        public List<Stats> listeStats;
    }

	void Start ()
    {
        for (int i = 0; i < masterVillage.childCount; i++)
        {
            listeVillage.Add(masterVillage.GetChild(i).GetComponent<Village>());
            listeVillage[i].nom = tableauRessource[i].nomVille;
        }
    }
	
	public void Update ()
    {
        int i = 0;

        foreach (Village unVillage in listeVillage)
        {
            tableauRessource[i].
        }


	}
}
