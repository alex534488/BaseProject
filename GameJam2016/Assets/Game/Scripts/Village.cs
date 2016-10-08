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
    public int or = 50;
    public int nourriture = 50;
    public int army = 10;
    public int reputation = 10;
    #endregion

    // Modifier le cout de la nourriture et des armees
    #region Cout Des Ressources En Or 
    public int coutNourriture = 25; // pour 15 (peut varier) - correspond au montant exact pour nourrir la population
    public int costArmy = 15; // pour 1 (peut varier)
    #endregion

    #region Depense de Nourriture
    public int nourrirArmy = 0;
    public int nourrirPopulation = 15;
    #endregion

    #region Production 
    public int productionOr = 2;
    public int productionNourriture = 1;
    public int productionArmy = 0;
    #endregion

    #region Conditions
    public bool isAttacked = false;
    public bool isDestroyed = false;
    public bool isFrontier = false;
    #endregion

    #region Taxes
    public int taxeOr = 20;
    public int taxeNourriture = 10;
    public int taxeArmy = 0;
    #endregion

    #region Classes
    public Empire empire;
    public Seigneur lord;
    public Barbare barbares;
    #endregion

    public int random = 0;

    public Village(Empire empire, int id, string nomvillage, string nomseigneur)
    {
        this.empire = empire;
        this.id = id;
        this.nom = nomvillage;

        // Ressource de depart aleatoire
        AddGold((int)(Random.value * 100));
        AddFood((int)(Random.value * 100));
        AddArmy((int)(Random.value * 10));
        AddReputation((int)(Random.value * 10));

        // Nouveau random constant pour l'update des ressources
        random = (int)(Random.value * 100);

        lord = new Seigneur(this);
        this.lord.nom = nomseigneur;
    }
	
	public virtual void Update ()
    {
        random = (int)(Random.value * 100);

        nourrirArmy = 2 * army;

        UpdateResources();

        lord.Update();

        UpdateCost();

        UpdateTaxes();
	}

    #region Attack
    public void DestructionVillage(){ empire.DeleteVillage(this); }

    public void BeingAttack(Barbare attaquant) { isAttacked = true; barbares = attaquant; }

    public void OnBecomesFrontier() { isFrontier = true; }
    #endregion

    #region Fonctions modifiant les attributs
    void DecreaseGold(int amount){ or -= amount; }

    void AddGold(int amount){ or += amount; }

    void DecreaseFood(int amount){ nourriture -= amount; }

    void AddFood(int amount){ nourriture += amount; }

    void DecreaseArmy(int amount){ army -= amount; }

    void AddArmy(int amount){ army += amount; }

    void DecreaseReputation(int amount) { army -= amount; }

    void AddReputation(int amount) { army += amount; }
    #endregion

    // To do : Change or remove random
    #region Updates 
    void UpdateResources()
    {
        // Ajout de resources aleatoire
        if (productionOr > 0) AddGold(productionOr * random);
        if (productionOr < 0) DecreaseGold(productionOr * random);

        if (productionNourriture > 0) AddFood(productionNourriture * random);
        if (productionNourriture < 0) DecreaseFood(productionNourriture * random);
    } // To do : Change or remove random

    void UpdateCost() // To do : Change or remove random
    {
        if (nourrirPopulation < 0) AddFood(nourrirPopulation * random);
        if (nourrirPopulation > 0) DecreaseFood(nourrirArmy * random + nourrirPopulation * random);
    }

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
