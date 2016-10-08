﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Village : IUpdate {
    // Identifiant du village
    public int id = 0;

    // Valeur initiale des attributs
    public int or = 50;
    public int nourriture = 50;
    public int army = 10;

    // Cout des ressources en or
    public int coutNourriture = 25; // pour 15 (peut varier) - correspond au montant exact pour nourrir la population
    public int coutArmy = 15; // pour 1 (peut varier)

    // Par tour
    public int productionOr = 2;
    public int productionNourriture = 1;
    public int nourrirPopulation = 15; 

    public int random = 0;
    public bool isAttacked = false;
    public bool isDestroyed = false;

    public Empire empire;
    public Seigneur lord;
    public Barbare barbares;

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

        lord = new Seigneur(this);
    }
	
	public virtual void Update ()
    {
        random = (int)(Random.value * 100);

        UpdateResources();

        lord.Update();

        UpdateCost();
	}

    public void DestructionVillage(){ empire.DeleteVillage(this); }

    public void BeingAttack(Barbare attaquant) { isAttacked = true; barbares = attaquant; }

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

    void UpdateCost()
    {
        if (nourrirPopulation < 0) AddFood(nourrirPopulation * random);
        if (nourrirPopulation > 0) DecreaseFood(nourrirPopulation * random);
    }
}
