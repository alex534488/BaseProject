using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;
using System.Runtime.Serialization;

[System.Serializable]
public class Village : INewDay
{
    private bool capitale = false;
    private string name = "UnNamed";
    private bool isDestroyed = false;
    public bool IsDestroyed { get { return isDestroyed; } }

    private Empire empire;
    public Empire Empire { get { return empire; } }
    private Architect architect;

    private int mapPosition;

    #region Stats
    //Les valeur inséré sont temporaire
    private Stat<int> armyPower = new Stat<int>(10);
    private Stat<int> goldProd = new Stat<int>(1);
    private Stat<int> materialProd = new Stat<int>(2);
    private Stat<int> scienceProd = new Stat<int>(3);
    private Stat<int> food = new Stat<int>(4);
    #endregion

    public Village(Empire empire, string name, int position, bool capitale = false)
    {
        this.empire = empire;
        this.name = name;
        mapPosition = position;
        this.capitale = capitale;
        architect = new Architect(this);
    }

    //TODO
    public Village(Village clone)
    {

    }

    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {

    }

    public void NewDay()
    {
        if (isDestroyed)
        {
            empire.DestroyCity(mapPosition);
        }
    }

    /// <summary>
    /// Met à jour les ressources de l'empire en fonction de la production du village
    /// </summary>
    public void UpdateResource()
    {
        // + science
        empire.Add(Empire_ResourceType.science, Get(Village_ResourceType.scienceProd));

        // + matériaux
        empire.Add(Empire_ResourceType.material, Get(Village_ResourceType.materialProd)); // ajoute la production de materiaux a l'empire

        // + citoyen
        empire.Add(Empire_ResourceType.citizenProgress, Get(Village_ResourceType.food));

        // + gold
        empire.Add(Empire_ResourceType.gold, Get(Village_ResourceType.goldProd));
    }

    public void SetAsCapital()
    {
        capitale = true;
        scienceProd.Set(1);
    }

    public bool IsCapital()
    {
        return capitale;
    }

    public int GetMapPosition()
    {
        return mapPosition;
    }

    public string Name
    {
        get { return name; }
    }

    public Architect Architect
    {
        get { return architect; }
    }

    #region Stats Method
    public Stat<int> GetStat(Village_ResourceType type)
    {
        switch (type)
        {
            default:
                return null;
            case Village_ResourceType.armyPower:
                return armyPower;
            case Village_ResourceType.goldProd:
                return goldProd;
            case Village_ResourceType.materialProd:
                return materialProd;
            case Village_ResourceType.scienceProd:
                return scienceProd;
            case Village_ResourceType.food:
                return food;
        }
    }

    public int Get(Village_ResourceType type)
    {
        Stat<int> stat = GetStat(type);
        return stat != null ? stat : 0;
    }

    public void Set(Village_ResourceType type, int value)
    {
        Stat<int> stat = GetStat(type);
        if (stat != null)
            stat.Set(value);
    }

    public void Add(Village_ResourceType type, int value)
    {
        Set(type, Get(type) + value);
    }

    public void Remove(Village_ResourceType type, int value)
    {
        Set(type, Get(type) - value);
    }

    public virtual Stat<int>.StatEvent GetOnSetEvent(Village_ResourceType type)
    {
        Stat<int> stat = GetStat(type);
        return stat != null ? stat.onSet : null;
    }
    #endregion


    public static void Transfer(Village source, Village destination, Village_ResourceType resource, int amount)
    {
        if (amount < 0) //Swap les deux village si le montant est negatif
        {
            Village temp = source;
            source = destination;
            destination = temp;
        }

        if (source != null) source.Add(resource, -amount);
        if (destination != null) destination.Add(resource, amount);
    }

    public void ApplyBattleResult(BattleResult result)
    {
        if (result.barbareAttack)
        {
            Set(Village_ResourceType.armyPower, result.defenderLeft);
        }
        else
        {
            Set(Village_ResourceType.armyPower, result.invaderLeft);
        }

        if (Get(Village_ResourceType.armyPower) < 1)
            empire.DestroyCity(mapPosition);
    }

    public void Destroy()
    {
        isDestroyed = true;
    }

    public void Attack(int position)
    {
        List<BarbarianClan> clansOnPosition = Universe.Barbares.GetClans(position);
        foreach (BarbarianClan clan in clansOnPosition)
        {
            BattleLauncher.LaunchBattle(clan, this);
            if (armyPower < 1) break; // seuil de retraite possible?
        }
        if (armyPower > 0 && Universe.Barbares.GetClans(position).Count <= 0)
        {
            empire.BuildCity(position);
        }
    }
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
