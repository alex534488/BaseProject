using UnityEngine;
using System.Collections;

public abstract class Village : IUpdate {

    public int or = 10;
    public int nourriture = 20;
    public int army = 5;

    public Empire empire;

    public Village(Empire empire)
    {
        this.empire = empire;
    }

	void Start ()
    {
	    
	}
	
	public void Update ()
    {
	    if(nourriture <= 0)
        {
            //famine
        }
	}



    // Fonction modifiant les attributs

    void DecreaseGold(int amount){ or -= amount; }

    void AddGold(int amount){ or += amount; }

    void DecreaseFood(int amount){ nourriture -= amount; }

    void AddFood(int amount){ nourriture += amount; }

    void DecreaseArmy(int amount){ army -= amount; }

    void AddArmy(int amount){ army += amount; }
}
