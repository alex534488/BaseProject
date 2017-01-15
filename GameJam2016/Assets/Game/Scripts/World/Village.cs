using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public class Village : INewDay
{
    public class StatEvent : UnityEvent<int> { }

    private bool capitale = false;

    #region Stats
    private Stat<int> armyPower = new Stat<int>(0);
    private Stat<int> armyCost = new Stat<int>(0);
    private Stat<int> goldProd = new Stat<int>(1);
    private Stat<int> materialProd = new Stat<int>(1);
    private Stat<int> scienceProd = new Stat<int>(0);
    private Stat<int> food = new Stat<int>(1);
    #endregion

    //private Architect architect;

    private int mapPosition;

    public virtual void NewDay()
    {
        // Verification sur le village a chaque tour (mort/destruction?)
    }

    public void UpdateResource(Empire empire)
    {
        // Si c'est une capitale
        if (IsCapital())
        {
            empire.Add(ResourceType.science, Get(ResourceType.scienceProd)); // Ajoute la production de science a l'empire
        }
        else // Sinon
        {
            Add(ResourceType.material, Get(ResourceType.materialProd)); // ajoute la production de materiaux a l'empire
            // Verification du compteur de creation de citoyen, s'il est superieur a son max, on doit generer un
            // evennement et redemarer le compteur
            if ((empire.Get(ResourceType.citizenProgress) + Get(ResourceType.food)) >= empire.Get(ResourceType.citizenProgressMax))
            {
                int temp = empire.Get(ResourceType.citizenProgress) + Get(ResourceType.food); // total
                temp = temp - empire.Get(ResourceType.citizenProgressMax); // difference du total avec le max
                empire.Set(ResourceType.citizenProgress,temp); // le reste est conserver
                // Evenement de production d'un citoyen ICI !
            }
        }
        empire.Add(ResourceType.gold, Get(ResourceType.goldProd)); // ajout de la production d'or de tous les villages a l'empire
    }

    public void SetAsCapital()
    {
        capitale = true;
        scienceProd.Set(1);
        armyCost.Set(1);
    }

    public bool IsCapital()
    {
        return capitale;
    }

    public int GetMapPosition()
    {
        return mapPosition;
    }

    #region Stats Method
    public int Get(ResourceType type)
    {
        switch (type)
        {
            default:
                return 0;
            case ResourceType.armyPower:
                return armyPower;
            case ResourceType.armyCost:
                return armyCost;
            case ResourceType.goldProd:
                return goldProd;
            case ResourceType.materialProd:
                return materialProd;
            case ResourceType.scienceProd:
                return scienceProd;
            case ResourceType.food:
                return food;
        }
    }

    public void Set(ResourceType type, int value)
    {
        switch (type)
        {
            default:
                return;
            case ResourceType.armyPower:
                armyPower.Set(value);
                return;
            case ResourceType.armyCost:
                armyCost.Set(value);
                return;
            case ResourceType.goldProd:
                goldProd.Set(value);
                return;
            case ResourceType.materialProd:
                materialProd.Set(value);
                return;
            case ResourceType.scienceProd:
                scienceProd.Set(value);
                return;
            case ResourceType.food:
                food.Set(value);
                return;
        }
    }

    public void Add(ResourceType type, int value)
    {
        switch (type)
        {
            default:
                return;
            case ResourceType.armyPower:
                armyPower.Set(armyPower + value);
                return;
            case ResourceType.armyCost:
                armyCost.Set(armyCost + value);
                return;
            case ResourceType.goldProd:
                goldProd.Set(goldProd + value);
                return;
            case ResourceType.materialProd:
                materialProd.Set(materialProd + value);
                return;
            case ResourceType.scienceProd:
                scienceProd.Set(scienceProd + value);
                return;
            case ResourceType.food:
                food.Set(food + value);
                return;
        }
    }

    public virtual Stat<int>.StatEvent GetOnSet(ResourceType type)
    {
        switch (type)
        {
            default:
                return null;
            case ResourceType.armyPower:
                return armyPower.onSet;
            case ResourceType.armyCost:
                return armyCost.onSet;
            case ResourceType.goldProd:
                return goldProd.onSet;
            case ResourceType.materialProd:
                return materialProd.onSet;
            case ResourceType.scienceProd:
                return scienceProd.onSet;
            case ResourceType.food:
                return food.onSet;
        }
    }
    #endregion
}

/*

    ANCIEN SYSTEME!!! (a utiliser comme reference)

public struct Ligne
{
    public int total;
    public int profit;
    public int taxe;
}

public class Village : INewDay
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
        //int valS = empire.valeurSoldat;  //J'ai mis cette valeur en commentaire parce qu'elle n'était pas utilisé. Veut-on l'utiliser ?

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

    public virtual void NewDay()
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

        lord.NewDay();


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
        int realamount = 0;

        realamount = army + amount;

        // Si la quantite de soldat est sous 0 apres la modification
        // La ville doit avoir une penalite!
        if (realamount < 0)
        {
            BuyArmy(realamount);
            //AddFoodProd(-amount * armyFoodCost); // Pourquoi????
            // Autres effets negatif/positif ICI
        }
        else
        {
            army.Set(realamount);
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
        if (coupTotal < 0)
        {
            AddGold(coupTotal);
            return true;
        }

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
*/
