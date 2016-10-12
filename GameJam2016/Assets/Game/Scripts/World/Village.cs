using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public enum Ressource_Type
{
    gold, food, army, happiness, reputation
}

public struct Ligne
{
    public int total;
    public int profit;
    public int taxe;
}

public class Village : IUpdate {

    public class StatEvent : UnityEvent<int> { }

    #region Identifiant
    public int id = 0;
    public string nom;
    #endregion

    #region Valeurs Initiales
    public int or = 5;
    public int nourriture = 10;
    public int army = 5;
    public int reputation = 100;
    #endregion

    #region Production 
    public int productionOr = 0;
    public int productionNourriture = 3;
    public int productionArmy = 0;
    #endregion

    #region Conditions
    public bool isAttacked = false;
    public bool isDestroyed = false;
    public bool isFrontier = false;
    #endregion

    #region Classes
    public Empire empire;
    public Seigneur lord;
    public Barbare barbares;
    #endregion

    public int random = 0;

    public int coutNourriture;
    public int costArmy;
    public int armyFoodCost = 1;

    //Events
    public StatEvent onGoldChange = new StatEvent();
    public StatEvent onGoldProdChange = new StatEvent();
    public StatEvent onArmyChange = new StatEvent();
    public StatEvent onFoodChange = new StatEvent();
    public StatEvent onFoodProdChange = new StatEvent();
    public StatEvent onReputationChange = new StatEvent();


    public Village(Empire empire, int id, string nomvillage, string nomseigneur)
    {
        this.empire = empire;
        this.id = id;
        this.nom = nomvillage;

        int valN = empire.valeurNouriture;
        int valO = empire.valeurOr;
        int valS = empire.valeurSoldat;

        coutNourriture = empire.valeurNouriture;
        costArmy = empire.valeurSoldat;

        int nbPointProduction = 10;

        while(nbPointProduction >0)
        {
            float choixRng = Random.value;

            if (nbPointProduction >= valN && choixRng < 0.3)
            {
                ModifyFoodProd(1);
                nbPointProduction -= valN;
            }
            else if (nbPointProduction >= valO)
            {
                ModifyGoldProd(1);
                nbPointProduction -= valO;
            }
            
        }


        lord = new Seigneur(this);
        this.lord.nom = nomseigneur;
    }
	
	public virtual void Update ()
    {
        random = (int)(Random.value * 100);

        if (isDestroyed)
        {
            DestructionVillage();
        }
        else if (nourriture < 0)
        {
            lord.Death();
            DestructionVillage();
        }

        UpdateResources();

        UpdateCost();

        lord.Update();


    }

    #region Attack
    public virtual void DestructionVillage()
    {
        this.isDestroyed = true;
        this.army = 0;
        this.nourriture = 0;
        this.or = 0;
        empire.DeleteVillage(this);
    }

    public void BeingAttack(Barbare attaquant) { isAttacked = true; barbares = attaquant; }

    public void OnBecomesFrontier() { isFrontier = true; }
    #endregion

    #region Fonctions modifiant les attributs
    public void DecreaseGold(int amount){ or -= amount; onGoldChange.Invoke(-amount); }

    public void AddGold(int amount){ or += amount; onGoldChange.Invoke(amount); }

    public void DecreaseFood(int amount){ nourriture -= amount; onFoodChange.Invoke(-amount); }

    public void AddFood(int amount){ nourriture += amount; onFoodChange.Invoke(amount); }

    public void DecreaseArmy(int amount)
    {
        army -= amount; 

        if (army < 0)
        {
            int nbTotal = -1 * army;
            int coutTotal = empire.valeurSoldat * nbTotal;
            // Dans les trois lignes suivantes quelques chose marche pas (le gold s'enleve pas tjrs au complet
            if (Empire.instance.capitale == this) Empire.instance.capitale.DecreaseBonheur(nbTotal);
            DecreaseGold(coutTotal);
            army = 0;
            onArmyChange.Invoke(-amount);
        } else
        {
            onArmyChange.Invoke(-amount);
        }
    }

    public void SetArmy(int amount) { army = amount; onArmyChange.Invoke(amount); }

    public void AddArmy(int amount){ army += amount; onArmyChange.Invoke(amount); }

    public void DecreaseReputation(int amount) { reputation -= amount; onReputationChange.Invoke(-amount); }

    public void AddReputation(int amount)
    {
        amount = Mathf.Min(100 - reputation, amount);
        reputation += amount; onReputationChange.Invoke(amount);
    }

    public void ModifyFoodProd(int amount) { productionNourriture += amount; onFoodProdChange.Invoke(amount); }

    public void ModifyGoldProd(int amount) { productionOr += amount; onGoldProdChange.Invoke(amount); }

    public void ModifyResource(Ressource_Type type, int amount)
    {
        switch (type)
        {
            default:
                return;
            case Ressource_Type.army:
                AddArmy(amount);
                break;
            case Ressource_Type.food:
                AddFood(amount);
                break;
            case Ressource_Type.gold:
                AddGold(amount);
                break;
        }
    }

    public int AmountOfResources(Ressource_Type type)
    {
        switch (type)
        {
            case Ressource_Type.army:
                return army;
            case Ressource_Type.food:
                return nourriture;
            case Ressource_Type.gold:
                return or;
            case Ressource_Type.reputation:
                return reputation;
            default:
                return 0;
        }
    }

    public virtual StatEvent GetStatEvent(Ressource_Type type, bool isAlternative = false)
    {
        switch (type)
        {
            case Ressource_Type.army:
                return onArmyChange;
            case Ressource_Type.food:
                return isAlternative? onFoodProdChange: onFoodChange;
            case Ressource_Type.gold:
                return isAlternative? onGoldProdChange: onGoldChange;
            case Ressource_Type.reputation:
                return onReputationChange;
        }
        return null;
    }

    public bool BuyArmy(int amount)
    {
        int coupTotal = costArmy * amount;
        if (coupTotal > or) return false;

        AddArmy(amount);
        DecreaseGold(coupTotal);
        return true;
    }
    #endregion

    // To do : Change or remove random
    #region Updates 
    protected void UpdateResources()
    {
        AddGold(productionOr);
        AddFood(productionNourriture - (army * armyFoodCost));
        // AddArmy(productionArmy);
       
    } // To do : Change or remove random

    protected void UpdateCost() // To do : Change or remove random
    {
        // Remove
    }
    
    #endregion 

    #region Interaction avec UI
    public static void Transfer(Village source, Village destinataire, Ressource_Type ressource, int amount)
    {
        switch (ressource)
        {
            case Ressource_Type.gold:
                {
                    if (source.or >= amount)
                    {
                        source.DecreaseGold(amount);
                        destinataire.AddGold(amount);
                    }

                    else
                    {
                        // Le village source ne possede pas assez de ressources pour effectuer la transaction
                    }
                        break;
                }

            case Ressource_Type.food:
                {
                    if (source.nourriture >= amount)
                    {
                        source.DecreaseFood(amount);
                        destinataire.AddFood(amount);
                    }

                    else
                    {
                        // Le village source ne possede pas assez de ressources pour effectuer la transaction
                    }
                    break;
                }

            case Ressource_Type.army:
                {
                    if (source.army >= amount)
                    {
                        source.DecreaseArmy(amount);
                        destinataire.AddArmy(amount);
                    }

                    else
                    {
                        // Le village source ne possede pas assez de ressources pour effectuer la transaction
                    }
                    break;
                }

            case Ressource_Type.happiness:
                {
                    break;
                }
        }
    }

    public int GetBilan(Ressource_Type type)
    {
        switch (type)
        {
            default:
                return 0;
            case Ressource_Type.army:
                return productionArmy;
            case Ressource_Type.food:
                return productionNourriture - (army * armyFoodCost);
            case Ressource_Type.gold:
                return productionOr;
        }
    }
    public int GetTotal(Ressource_Type type)
    {
        switch (type)
        {
            default:
                return 0;
            case Ressource_Type.army:
                return army;
            case Ressource_Type.food:
                return nourriture;
            case Ressource_Type.gold:
                return or;
        }
    }

    #endregion
}
