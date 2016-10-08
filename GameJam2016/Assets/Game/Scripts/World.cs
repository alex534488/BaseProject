using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    public static World main;
    public Empire empire;
    public BarbareManager barbareManager;

    public List<Barbare> listBarbare = new List<Barbare>();

    void Awake()
    {
        if (main == null) main = this;
    }

	void Start ()
    {
        empire = new Empire();
        empire.Start();
    }

    public void Update()
    {
        empire.Update();
    }

    public Village GiveTarget() // Verifie la liste des villages et retourne le village frontiere le plus faible de la liste
    {
        float minimalArmy = Mathf.Infinity;
        Village bestTarget = null;

        foreach (Village leVillage in empire.listVillage)
        {
            if (leVillage.army <= minimalArmy && leVillage.isFrontier)
            {
                bestTarget = leVillage;
                minimalArmy = leVillage.army;
            }
        }

        return bestTarget;
    }

    


}
