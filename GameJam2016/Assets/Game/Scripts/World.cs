using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    private List<Village> listeVillage;

	// Use this for initialization
	void Start ()
    {
        Transform transformVillage = this.transform.Find("Villages");

        for (int i =0; i< transformVillage.childCount;i++)
        {
            listeVillage.Add(transformVillage.GetChild(i).GetComponent<Village>());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public Village GiveTarget() // Verifie la liste des villages et retourne le village frontiere le plus faible de la liste
    {
        int minimalArmy = 0;

        foreach (Village unVillage in listeVillage)
        {


        }

        Village unVillage = null;
        return unVillage;
    }
}
