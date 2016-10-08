using UnityEngine;
using System.Collections;

public class Capitale : Village {

    public int capitaleOr = 10;
    public int capitaleNourriture = 20;
    public int capitaleArmy = 5;
    public int bonheur = 100;

    public int productionBonheur = 1;

    public Capitale(Empire empire, int id) : base(empire,id, "ROME", null)
    {
        this.empire = empire;
        this.id = id;

        or = capitaleOr;
        nourriture = capitaleNourriture;
        army = capitaleArmy;

        lord = new Seigneur(this);
    }
	
	public override void Update () // es ce que le override rajoute ou remplace?
    {
        base.Update();

        if(productionBonheur > 0) AddBonheur(productionBonheur * random);
        if (productionBonheur < 0) DecreaseBonheur(productionBonheur * random);
    }

    void DecreaseBonheur(int amount) { bonheur -= amount; }

    void AddBonheur(int amount) { bonheur += amount; }
}
