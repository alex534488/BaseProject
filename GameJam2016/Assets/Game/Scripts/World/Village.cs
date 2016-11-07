using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public enum Ressource_Type
{
    or, nourriture, armé, bonheur, réputation
}

public struct Ligne
{
    public int total;
    public int profit;
    public int taxe;
}

public class Village : IUpdate
{

    public class StatEvent : UnityEvent<int> { }

    #region Identifiant
    public int id = 0;
    public string nom;
    #endregion

    #region Valeurs Initiales
    protected Stat<int> gold = new Stat<int>(5);
    protected Stat<int> food = new Stat<int>(10);
    protected Stat<int> army = new Stat<int>(5);
    protected Stat<int> reputation = new Stat<int>(100, 0, 100);
    #endregion

    #region Production 
    protected Stat<int> prodGold = new Stat<int>(0);
    protected Stat<int> prodFood = new Stat<int>(3);
    protected Stat<int> prodArmy = new Stat<int>(0);
    #endregion

    #region Conditions
    public bool isAttacked = false;
    public bool isDestroyed = false;
    public bool isFrontier = false;
    #endregion

    #region Classes
    public Seigneur lord;
    public Barbare barbares;
    #endregion

    public int random = 0;

    public int coutNourriture;
    public int costArmy;
    public int armyFoodCost = 1;


    public Village(Empire empire, int id, string nomvillage, string nomseigneur)
    {
        this.id = id;
        this.nom = nomvillage;

        int valN = empire.valeurNouriture;
        int valO = empire.valeurOr;
        int valS = empire.valeurSoldat;

        coutNourriture = empire.valeurNouriture;
        costArmy = empire.valeurSoldat;

        int nbPointProduction = 10;

        while (nbPointProduction > 0)
        {
            float choixRng = Random.value;

            if (nbPointProduction >= valN && choixRng < 0.3)
            {
                AddFoodProd(1);
                nbPointProduction -= valN;
            }
            else if (nbPointProduction >= valO)
            {
                AddGoldProd(1);
                nbPointProduction -= valO;
            }

        }


        lord = new Seigneur(this);
        this.lord.nom = nomseigneur;
    }

    public virtual void Update()
    {
        random = (int)(Random.value * 100);

        if (isDestroyed)
        {
            DestructionVillage();
        }
        else if (food < 0)
        {
            lord.Death();
            DestructionVillage();
        }

        UpdateResources();

        lord.Update();


    }

    #region Attack
    public virtual void DestructionVillage()
    {
        isDestroyed = true;
        Empire.instance.DeleteVillage(this);
    }

    public void BeingAttack(Barbare attaquant) { isAttacked = true; barbares = attaquant; }

    public void OnBecomesFrontier() { isFrontier = true; }
    #endregion

    #region Fonctions modifiant les attributs

    public void AddGold(int amount) { gold.Set(gold + amount); }
    public int GetGold() { return gold; }

    public void AddFood(int amount) { food.Set(food + amount); }
    public int GetFood() { return food; }

    public void SetArmy(int amount) { army.Set(army + amount); }
    public void AddArmy(int amount)
    {
        army.Set(army + amount);

        if (amount < 0 && army < 0)     //Faut-il achter des soldat ?
        {
            BuyArmy(-1 * army);
        }
    }
    public int GetArmy() { return army; }

    public void AddReputation(int amount) { reputation.Set(reputation + amount); }
    public int GetReputation() { return reputation; }

    public void AddFoodProd(int amount) { prodFood.Set(prodFood + amount); }
    public int GetFoodProd() { return prodFood; }

    public void AddGoldProd(int amount) { prodGold.Set(prodGold + amount); }
    public int GetGoldProd() { return prodGold; }

    public virtual void AddResource(Ressource_Type type, int amount)
    {
        switch (type)
        {
            default:
                return;
            case Ressource_Type.armé:
                AddArmy(amount);
                break;
            case Ressource_Type.nourriture:
                AddFood(amount);
                break;
            case Ressource_Type.or:
                AddGold(amount);
                break;
        }
    }

    public int AmountOfResources(Ressource_Type type)
    {
        switch (type)
        {
            case Ressource_Type.armé:
                return army;
            case Ressource_Type.nourriture:
                return food;
            case Ressource_Type.or:
                return gold;
            case Ressource_Type.réputation:
                return reputation;
            default:
                return 0;
        }
    }

    public virtual Stat<int>.StatEvent GetStatEvent(Ressource_Type type, bool isAlternative = false)
    {
        switch (type)
        {
            case Ressource_Type.armé:
                return army.onSet;
            case Ressource_Type.nourriture:
                return isAlternative ? prodFood.onSet : food.onSet;
            case Ressource_Type.or:
                return isAlternative ? prodGold.onSet : gold.onSet;
            case Ressource_Type.réputation:
                return reputation.onSet;
        }
        return null;
    }

    public virtual bool BuyArmy(int amount) // A modifier
    {
        int coupTotal = costArmy * amount;
        if (coupTotal > gold) return false;

        AddArmy(amount);
        AddGold(-coupTotal);
        return true;
    }
    #endregion

    // To do : Change or remove random
    #region Updates 
    protected void UpdateResources()
    {
        AddGold(prodGold);
        AddFood(prodFood - (army * armyFoodCost));
        // AddArmy(productionArmy);
    }

    #endregion

    #region Interaction avec UI
    public static void Transfer(Village source, Village destinataire, Ressource_Type ressource, int amount)
    {
        switch (ressource)
        {
            case Ressource_Type.or:
                if (source.GetGold() >= amount)
                {
                    source.AddGold(-amount);
                    destinataire.AddGold(amount);
                }
                break;
            case Ressource_Type.nourriture:
                if (source.GetFood() >= amount)
                {
                    source.AddFood(-amount);
                    destinataire.AddFood(amount);
                }
                break;
            case Ressource_Type.armé:
                if (source.GetArmy() >= amount)
                {
                    source.AddArmy(-amount);
                    destinataire.AddArmy(amount);
                }
                break;
        }
    }

    public virtual int GetResourceAlt(Ressource_Type type)
    {
        switch (type)
        {
            default:
                return 0;
            case Ressource_Type.armé:           //bilan de soldat par tour
                return prodArmy;
            case Ressource_Type.nourriture:     //bilan de bouffe par tour
                return prodFood - (army * armyFoodCost);
            case Ressource_Type.or:             //bilan d'or par tour
                return prodGold;
        }
    }
    public virtual int GetResource(Ressource_Type type)
    {
        switch (type)
        {
            default:
                return 0;
            case Ressource_Type.armé:
                return army;
            case Ressource_Type.nourriture:
                return food;
            case Ressource_Type.or:
                return gold;
        }
    }

    #endregion
}
