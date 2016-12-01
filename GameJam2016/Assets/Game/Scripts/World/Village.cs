using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

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
    protected Stat<int> army = new Stat<int>(5); //S'assurer de modifier la 'foodProd' initiale en conséquent.
    protected Stat<int> reputation = new Stat<int>(100, 0, 100);
    #endregion

    #region Production 
    protected Stat<int> goldProd = new Stat<int>(0);
    protected Stat<int> foodProd = new Stat<int>(-2); //Tiens déjà en considération les 5 soldats de départ
    protected Stat<int> armyProd = new Stat<int>(0, 0, int.MaxValue);
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

    public void AddGold(int amount)
    {
        if (amount == 0) return;
        gold.Set(gold + amount);
    }
    public int GetGold() { return gold; }

    public void AddFood(int amount)
    {
        if (amount == 0) return;
        food.Set(food + amount);
    }
    public int GetFood() { return food; }
    public int GetFoodBilan() { return foodProd - (army * armyFoodCost); }

    public void SetArmy(int amount) { AddArmy(amount - army); }
    public void AddArmy(int amount)
    {
        if (amount == 0) return;
        army.Set(army + amount);
        AddFoodProd(-amount * armyFoodCost);

        //Si le changement est négatif et que les soldat sont à 0, il en acheter.
        //Ceci assure une quantité de soldat >= 0 tout en pénalisant le village.
        if (amount < 0 && army < 0)
        {
            BuyArmy(-1 * army);
        }
    }
    public void AddArmyProd(int amount)
    {
        if (amount == 0) return;
        armyProd.Set(armyProd + amount);
    }
    public int GetArmy() { return army; }
    public int GetArmyProd() { return armyProd; }

    public void AddReputation(int amount)
    {
        if (amount == 0) return;
        reputation.Set(reputation + amount);
    }
    public int GetReputation() { return reputation; }

    public void AddFoodProd(int amount)
    {
        if (amount == 0) return;
        foodProd.Set(foodProd + amount);
    }
    public int GetFoodProd() { return foodProd; }

    public void AddGoldProd(int amount)
    {
        if (amount == 0) return;
        goldProd.Set(goldProd + amount);
    }
    public int GetGoldProd() { return goldProd; }

    public virtual void AddResource(Resource_Type type, int amount)
    {
        switch (type)
        {
            default:
                return;
            case Resource_Type.army:
                AddArmy(amount);
                break;
            case Resource_Type.food:
                AddFood(amount);
                break;
            case Resource_Type.gold:
                AddGold(amount);
                break;
        }
    }

    public int AmountOfResources(Resource_Type type)
    {
        switch (type)
        {
            case Resource_Type.army:
                return army;
            case Resource_Type.food:
                return food;
            case Resource_Type.gold:
                return gold;
            case Resource_Type.reputation:
                return reputation;
            default:
                return 0;
        }
    }

    public virtual Stat<int>.StatEvent GetStatEvent(Resource_Type type)
    {
        switch (type)
        {
            default:
                return null;
            case Resource_Type.army:
                return army.onSet;
            case Resource_Type.armyProd:
                return armyProd.onSet;
            case Resource_Type.food:
                return food.onSet;
            case Resource_Type.foodProd:
                return foodProd.onSet;
            case Resource_Type.gold:
                return gold.onSet;
            case Resource_Type.goldProd:
                return goldProd.onSet;
            case Resource_Type.reputation:
                return reputation.onSet;
        }
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
        AddGold(GetGoldProd());
        AddFood(GetFoodProd());
        AddArmy(GetArmyProd());
    }

    #endregion

    #region Interaction avec UI
    public static void Transfer(Village source, Village destinataire, Resource_Type resource, int amount)
    {
        if (amount < 0) //Swap les deux village si le montant est negatif
        {
            Village temp = source;
            source = destinataire;
            destinataire = temp;
        }

        Give(source, resource, -amount);
        Give(destinataire, resource, amount);
    }

    public static void Give(Village village, Resource_Type resource, int amount)
    {
        if (village != null) village.LocalGive(resource, amount);
    }

    protected virtual void LocalGive(Resource_Type resource, int amount)
    {
        switch (resource)
        {
            case Resource_Type.gold:
                AddGold(amount);
                break;
            case Resource_Type.goldProd:
                AddGoldProd(amount);
                break;
            case Resource_Type.food:
                AddFood(amount);
                break;
            case Resource_Type.foodProd:
                AddFoodProd(amount);
                break;
            case Resource_Type.army:
                AddArmy(amount);
                break;
            case Resource_Type.armyProd:
                AddArmyProd(amount);
                break;
            case Resource_Type.reputation:
                AddReputation(amount);
                break;
        }
    }

    public virtual int GetResource(Resource_Type type)
    {
        switch (type)
        {
            default:
                return 0;
            case Resource_Type.army:
                return army;
            case Resource_Type.armyProd:
                return armyProd;
            case Resource_Type.food:
                return food;
            case Resource_Type.foodProd:
                return foodProd;
            case Resource_Type.gold:
                return gold;
            case Resource_Type.goldProd:
                return goldProd;
            case Resource_Type.reputation:
                return reputation;
            case Resource_Type.reputationCap:
                return 100;
        }
    }

    #endregion
}
