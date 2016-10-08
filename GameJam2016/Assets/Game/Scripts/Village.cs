using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public enum Ressource_Type
{
    gold, food, army, happiness
}

public struct Ligne
{
    public int total;
    public int production;
    public int taxe;
}

public class Village : IUpdate {

    #region Identifiant
    public int id = 0;
    public string nom;
    #endregion

    #region Valeurs Initiales
    public int or = 10;
    public int nourriture = 40;
    public int army = 5;
    public int reputation = 100;
    #endregion

    #region Production 
    public int productionOr = 1;
    public int productionNourriture = 5;
    public int productionArmy = 0;
    #endregion

    #region Conditions
    public bool isAttacked = false;
    public bool isDestroyed = false;
    public bool isFrontier = false;
    #endregion

    #region Taxes
    public int taxeOr = 1;
    public int taxeNourriture = 5;
    public int taxeArmy = 0;
    #endregion

    #region Classes
    public Empire empire;
    public Seigneur lord;
    public Barbare barbares;
    #endregion

    public int random = 0;

    public int coutNourriture;
    public int costArmy;
    public int nourrirArmy;


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

        int nbPointProduction = 30;

        while(nbPointProduction >0)
        {
            float choixRng = Random.value;
            if(nbPointProduction>= valS && choixRng<0.1)
            {
                productionArmy += 1;
                nbPointProduction -= valS;
            }
            else if (nbPointProduction >= valN && choixRng < 0.3)
            {
                productionNourriture += 1;
                nbPointProduction -= valN;
            }
            else if (nbPointProduction >= valO)
            {
                productionOr += 1;
                nbPointProduction -= valO;
            }
            
        }

        AddGold(productionOr*4);
        AddFood(productionNourriture*4);
        AddArmy(productionArmy*4);

        taxeArmy = productionArmy;

        lord = new Seigneur(this);
        this.lord.nom = nomseigneur;
    }
	
	public virtual void Update ()
    {
        random = (int)(Random.value * 100);

        nourrirArmy = army;

        UpdateResources();

        UpdateCost();

        UpdateTaxes();

        lord.Update();

        if(nourriture<0 || isDestroyed)
        {
            DestructionVillage();
        }    
	}

    #region Attack
    public void DestructionVillage(){ empire.DeleteVillage(this); }

    public void BeingAttack(Barbare attaquant) { isAttacked = true; barbares = attaquant; }

    public void OnBecomesFrontier() { isFrontier = true; }
    #endregion

    #region Fonctions modifiant les attributs
    public void DecreaseGold(int amount){ or -= amount; }

    public void AddGold(int amount){ or += amount; }

    public void DecreaseFood(int amount){ nourriture -= amount; }

    public void AddFood(int amount){ nourriture += amount; }

    public void DecreaseArmy(int amount){ army -= amount; }

    public void AddArmy(int amount){ army += amount; }

    public void DecreaseReputation(int amount) { army -= amount; }

    public void AddReputation(int amount) { army += amount; }
    #endregion

    // To do : Change or remove random
    #region Updates 
    void UpdateResources()
    {
        or += productionOr;
        nourriture += productionNourriture;
        army += productionArmy;
       
    } // To do : Change or remove random

    void UpdateCost() // To do : Change or remove random
    {
        nourriture -= army;
    }

    //Paid Taxes
    void UpdateTaxes()
    {
        Transfer(this, empire.capitale, Ressource_Type.gold, taxeOr);
        Transfer(this, empire.capitale, Ressource_Type.food, taxeNourriture);
        Transfer(this, empire.capitale, Ressource_Type.army, taxeArmy);
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

    public Ligne GetInfos(Ressource_Type ressource)
    {
        switch (ressource)
        {
            case Ressource_Type.gold:
                {
                    Ligne uneLigne = new Ligne();

                    uneLigne.total = or;
                    uneLigne.production = productionOr;
                    uneLigne.taxe = taxeOr;

                    return uneLigne;
                }

            case Ressource_Type.food:
                {
                    Ligne uneLigne = new Ligne();

                    uneLigne.total = nourriture;
                    uneLigne.production = productionNourriture;
                    uneLigne.taxe = taxeNourriture;

                    return uneLigne;
                }

            default:
            case Ressource_Type.army:
                {
                    Ligne uneLigne = new Ligne();

                    uneLigne.total = army;
                    uneLigne.production = productionArmy;
                    uneLigne.taxe = taxeArmy;

                    return uneLigne;
                }  
        }   

    }

    public int ModifyTaxe(Ressource_Type ressource, int amount)
    {
        switch(ressource)
        {
            case Ressource_Type.gold:
                {
                    // if (taxeOr > 0)
                        taxeOr = taxeOr + amount;
                    return taxeOr;
                }

            case Ressource_Type.food:
                {
                    // if (taxeNourriture > 0)
                        taxeNourriture = taxeNourriture + amount;
                    return taxeNourriture;
                }

            default:
            case Ressource_Type.army:
                {
                    // if (taxeArmy > 0)
                        taxeArmy= taxeArmy + amount;
                    return taxeArmy;
                }
        }
           
    }

    #endregion
}
