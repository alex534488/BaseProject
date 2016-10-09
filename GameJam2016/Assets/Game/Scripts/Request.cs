﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Request {

    public Seigneur messager;

    List<string> message = new List<string>();
    List<Dialog.Choix> choix = new List<Dialog.Choix>();
    
    public bool choosen = false;
    public int delay;


    // REQUETE MORT
    public Request(Seigneur messager)
    {
        message.Add("Bonjour Empereur. J'ai de mauvaises nouvelles. Le village " + messager.village.nom + " a malheureusement été détruit par une invasion barbare.");
        choix.Add(new Dialog.Choix("C'est malheureux en effet, mais l'Empire survivra!", delegate () { }));
    }

    // REQUETE D'AIDE
    public Request(Seigneur messager,Ressource_Type resource, int amount)
    {
        this.messager = messager;

        switch (resource)
        {
            case Ressource_Type.gold:
                message.Add("Le village "+messager.village.nom+" a besoin de "+amount+ " pièces d'or pour combler ses manquements économiques");
                choix.Add(new Dialog.Choix("Chaque village de l'Empire compte! ("+amount+ " Or, + Réputation)", delegate () { if (Empire.instance.capitale.or >= amount) { messager.village.AddGold(amount); Empire.instance.capitale.DecreaseGold(amount); messager.village.AddReputation(20); } }));
                choix.Add(new Dialog.Choix("L'or est précieux, faites bon usage de ces quelques pièces \n("+ (amount+1)/2 + " Or)", delegate () { if (Empire.instance.capitale.or >= (amount+1)/2) { messager.village.AddGold((amount+1)/2); Empire.instance.capitale.DecreaseGold((amount+1)/2); } } ));
                choix.Add(new Dialog.Choix("Les caisses sont vides pour vous! (- Réputation)", delegate () { messager.village.DecreaseReputation(20); }));
                return;
            case Ressource_Type.food:
                message.Add("Le village " + messager.village.nom + " a besoin de " + amount + " Nourritures pour nourrir les soldats stationnés dans notre village.");
                choix.Add(new Dialog.Choix("Chaque village de l'empire compte! ("+ amount + " Nourritures, + Réputation)", delegate () { if (Empire.instance.capitale.nourriture >= amount) { messager.village.AddFood(amount); Empire.instance.capitale.DecreaseFood(amount); messager.village.AddReputation(20); } }));
                choix.Add(new Dialog.Choix("Je peux vous fournir quelques rations de Nourritures mon cher. \n("+ (amount + 1) / 2 + " Nourriture)", delegate () { if (Empire.instance.capitale.nourriture >= (amount + 1) / 2) { messager.village.AddFood((amount + 1 )/ 2); Empire.instance.capitale.DecreaseFood((amount + 1) / 2); } }));
                choix.Add(new Dialog.Choix("Rome est au bord de la famine également. (- Réputation)", delegate () { messager.village.DecreaseReputation(20); }));
                return;
            case Ressource_Type.army:
                message.Add("Le village " + messager.village.nom + " a besoin de " + amount + " Soldats pour se défendre contre une invasion imminente de barbares.");
                choix.Add(new Dialog.Choix("Chaque village de l'empire compte! ("+ amount + " Soldats)", delegate () { if (Empire.instance.capitale.army >= amount) { messager.village.AddArmy(amount); Empire.instance.capitale.DecreaseArmy(amount); messager.village.AddReputation(20); } }));
                choix.Add(new Dialog.Choix("Je peux vous fournir quelques soldats mon cher. \n("+ (amount + 1 )/ 2 + " Soldats)", delegate () { if (Empire.instance.capitale.army >= (amount + 1 )/ 2) { messager.village.AddArmy((amount + 1 )/ 2); Empire.instance.capitale.DecreaseArmy((amount + 1) / 2); } }));
                choix.Add(new Dialog.Choix("Les barbares sont à nos portes également.", delegate () { messager.village.DecreaseReputation(20); }));
                return;
            default:
                return;
        }
    }

    // INVESTISSEMENT

    public Request(Seigneur messager, Ressource_Type resource)
    {
        this.messager = messager;
        int random = Mathf.CeilToInt(Random.Range(0,3));

        switch (resource)
        {
            case Ressource_Type.gold:
                switch (random)
                {
                    case 1:
                        message.Add("Je représente le village "+ messager.village.nom + "\n\nNotre récolte a été incroyablement abondante cette saison." + "\n\n" +
                                    "Nous voudrions éventuellement semer d'avantage de graines pour continuer d'avoir autant de réserves de nourritures. \n\nPar contre, cela necessiterait de nouveaux investissements majeurs.");
                        choix = new List<Dialog.Choix>();
                        choix.Add(new Dialog.Choix("Payez entièrement les frais des nouvelles semances \n(-40 Or, +2 Production Nourriture, + Réputation)", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.AddReputation(20); messager.village.ModifyFoodProd(2); }));
                        choix.Add(new Dialog.Choix("Aidez les villagois à construire la mine \n(-20 Or Capitale, -20 Or Village, +2 Production Nourriture)", delegate () { Empire.instance.capitale.DecreaseGold(20); messager.village.DecreaseGold(20); messager.village.ModifyFoodProd(2); }));
                        choix.Add(new Dialog.Choix("Refusez la demande du villageois (- Réputation)", delegate () { messager.village.DecreaseReputation(20); }));
                        return;
                    case 2:
                        message.Add("Salutation votre majesté. Je viens du village " + messager.village.nom + " qui pourrait bénéficier de votre support." + "\n\n" +
                                    "En effet, bien que nous ne manquons de rien, nos infrastructures commencent à veillir. \n\nCertains batiments risquent de s'éffondrer ou ne sont carrément plus utilisables");
                        message.Add("Nous aimerions reconstruire quelques batiments de votre choix afin de pour pouvoir poursuivre nos activités" + "\n\n" +
                                     "Êtes-vous en mesure de nous apporter votre aide mon seigneur?");
                        choix = new List<Dialog.Choix>();
                        choix.Add(new Dialog.Choix("Réparez toutes les fermes de votre village \n(-40 Or Capitale, -20 Or Village, +4 Production Or, + Réputation)", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.DecreaseGold(20); messager.village.AddReputation(20); messager.village.ModifyGoldProd(4); }));
                        choix.Add(new Dialog.Choix("Réparez les exploitations minières de votre village \n(-40 Or Capitale, -20 Or Village, +2 Production Nourriture, + Réputation)", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.DecreaseGold(20); messager.village.ModifyFoodProd(2); messager.village.AddReputation(20); }));
                        choix.Add(new Dialog.Choix("Refusez la demande de l'architecte (- Réputation)", delegate () { messager.village.DecreaseReputation(20); }));
                        return;
                    default:
                        message.Add("Je suis du village " + messager.village.nom + " et vous serez heureux d'apprendre que notre économie se porte à merveille!" + "\n\n" +
                                    "Je viens en tant que messager pour vous informer que nous voudrions une aide financière pour investir dans une nouvelle mine d'or.");
                        choix.Add(new Dialog.Choix(" Payez entièrement les frais de constructions de la mine \n(-40 Or , +4 Production Or, + Réputation)", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.AddReputation(20); messager.village.ModifyGoldProd(4); }));
                        choix.Add(new Dialog.Choix(" Aidez les villagois à construire la mine \n(-20 Or Capitale, -20 Or Village, +4 Production Or)", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.DecreaseGold(20); messager.village.ModifyGoldProd(4);}));
                        choix.Add(new Dialog.Choix(" Refusez la demande du villagois (- Réputation)", delegate () { messager.village.DecreaseReputation(20); }));
                        return;
                }
            case Ressource_Type.food:
                switch (random)
                {
                    default:
                        return;
                }
            case Ressource_Type.army:
                switch (random)
                {
                    default:
                        return;
                }
            default:
                return;
        }
    }

    // REQUETE CLASSIQUE
    public Request (List < string> message, List<Dialog.Choix> choix)
    {
        this.message = message;
        this.choix = choix;
    }

    public void DoRequest()
    {
        CharacterEnter.Enter(OnCharacterEnter);
    }

    void OnCharacterEnter()
    {
        Dialog.DisplayText(message, choix, OnTextComplete);
    }
    void OnTextComplete()
    {
        CharacterEnter.Exit(OnCharacterExit);
    }
    void OnCharacterExit()
    {
        RequestManager.DoNextRequest();
    }
}
