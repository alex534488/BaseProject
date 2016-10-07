using UnityEngine;
using System.Collections;

public class Village : IUpdate {
    public int id = 0;

    public int or = 50;
    public int nourriture = 50;
    public int army = 10;

    public int productionOr = 2;
    public int productionNourriture = 1;

    public int random = 0;

    public bool isAttacked = false;

    public Empire empire;
    // public Seigneur lord;

    public Village(Empire empire, int id)
    {
        this.empire = empire;
        this.id = id;

        // Ressource de depart aleatoire
        AddGold((int)(Random.value * 100));
        AddFood((int)(Random.value * 100));
        AddArmy((int)(Random.value * 10));

        // Nouveau random constant pour l'update des ressources
        random = (int)(Random.value * 100);
    }
	
	public virtual void Update ()
    {
        random = (int)(Random.value * 100);



        // lord.Update();

        UpdateResources();
	}

    public void DestructionVillage(){ empire.DeleteVillage(this); }

    // Fonction modifiant les attributs

    void DecreaseGold(int amount){ or -= amount; }

    void AddGold(int amount){ or += amount; }

    void DecreaseFood(int amount){ nourriture -= amount; }

    void AddFood(int amount){ nourriture += amount; }

    void DecreaseArmy(int amount){ army -= amount; }

    void AddArmy(int amount){ army += amount; }

    void UpdateResources()
    {
        // Ajout de resources aleatoire
        if (productionOr > 0) AddGold(productionOr * random);
        if (productionOr < 0) DecreaseGold(productionOr * random);

        if (productionNourriture > 0) AddFood(productionNourriture * random);
        if (productionNourriture < 0) DecreaseFood(productionNourriture * random);
    }
}
