using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarbareManager : MonoBehaviour {

    public List<Barbare> listeBarbare = new List<Barbare>();

    public void Initialize()
    {
        int child = this.transform.childCount;

        for (int i=0;i<child;i++)
        {
            Barbare unBarbare = this.transform.GetChild(i).GetComponent<Barbare>();
            listeBarbare.Add(unBarbare);
        }
	}

    public void AddClanBarbare()
    {
        Barbare unBarbare = new Barbare();
        listeBarbare.Add(unBarbare);
    }

    public int NombreTotalBarbare()
    {
        int barbareTotal = 0;

        for (int i = 0; i<listeBarbare.Count; i++)
        {
            barbareTotal = barbareTotal + listeBarbare[i].nbBarbares;
        }

        return barbareTotal;
    }
	
}
