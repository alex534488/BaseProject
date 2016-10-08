using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RessourceManager : IUpdate {

    private List<Village> listeVillage;

    public Transform masterVillage;

    public enum Ressource_Type
    {
        gold, food, army, happiness
    }

    struct Stats
    {
        string nom;
        int nombre;
    }

    struct Ligne
    {
        string nomVille;
        List<Stats> listeStats;
    }

    // Use this for initialization
	void Start ()
    {
        for (int i = 0; i < masterVillage.childCount; i++)
        {
            listeVillage.Add(masterVillage.GetChild(i).GetComponent<Village>());
        }
    }
	
	// Update is called once per frame
	public void Update ()
    {
	
	}
}
