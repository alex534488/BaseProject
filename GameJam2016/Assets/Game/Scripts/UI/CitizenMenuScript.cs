using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CitizenMenuScript : MonoBehaviour {

    public Canvas resources;
    private bool resourcesDisplay;
    public Canvas villages;
    private bool villagesDisplay;

    public VillageButton buttonPrefab;

    private List<Village> listVillage = new List<Village>();

	void Start () {
        listVillage = Universe.Empire.GetAllVillage();

        resourcesDisplay = false;
        villagesDisplay = true;

        resources.gameObject.SetActive(resourcesDisplay);
        villages.gameObject.SetActive(villagesDisplay);

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        foreach(Village village in listVillage)
        {
            // create a button with info
        }
    }
}
