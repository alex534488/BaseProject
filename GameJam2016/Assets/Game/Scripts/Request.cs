using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Request {

    public Seigneur messager;

    List<string> message = new List<string>();
    List<Dialog.Choix> choix = new List<Dialog.Choix>();
    
    public bool choosen = false;
    public int delay;

    // REQUETE D'AIDE

    public Request(Seigneur messager,Ressource_Type resource, int amount)
    {
        this.messager = messager;

        switch (resource)
        {
            case Ressource_Type.gold:
                message.Add("Le village "+messager.village.nom+" à bessoin de "+amount+ "or pour combler ses manquement économiques");
                choix.Add(new Dialog.Choix("Chaque village de l'empire compte! (donner "+amount+" or)", delegate () { if (Empire.instance.capitale.or >= amount) { messager.village.AddGold(amount); Empire.instance.capitale.DecreaseGold(amount); messager.village.AddReputation(20); } }));
                choix.Add(new Dialog.Choix("L'or est précieux, faite bon usage de ces quelques pièce (donner " + (amount+1)/2 + " or)", delegate () { if (Empire.instance.capitale.or >= (amount+1)/2) { messager.village.AddGold((amount+1)/2); Empire.instance.capitale.DecreaseGold((amount+1)/2); } } ));
                choix.Add(new Dialog.Choix("Les caisse sont vides pour vous!", delegate () { messager.village.DecreaseReputation(20); }));
                return;
            case Ressource_Type.food:
                message.Add("Le village " + messager.village.nom + " à bessoin de " + amount + " nourriture pour nourri les soldat stationnés dans le village.");
                choix.Add(new Dialog.Choix("Chaque village de l'empire compte! (donner " + amount + " nourritures)", delegate () { if (Empire.instance.capitale.nourriture >= amount) { messager.village.AddFood(amount); Empire.instance.capitale.DecreaseFood(amount); messager.village.AddReputation(20); } }));
                choix.Add(new Dialog.Choix("Les récoltes sont médiocre en ce moment. (donner " + (amount + 1) / 2 + " nourriture)", delegate () { if (Empire.instance.capitale.nourriture >= (amount + 1) / 2) { messager.village.AddFood((amount + 1 )/ 2); Empire.instance.capitale.DecreaseFood((amount + 1) / 2); } }));
                choix.Add(new Dialog.Choix("Rome est au bord de la famine également.", delegate () { messager.village.DecreaseReputation(20); }));
                return;
            case Ressource_Type.army:
                message.Add("Le village " + messager.village.nom + " à bessoin de " + amount + " soldats pour proteger contre l'arrivé récentes de barbares.");
                choix.Add(new Dialog.Choix("Chaque village de l'empire compte! (donner " + amount + " Soldats)", delegate () { if (Empire.instance.capitale.army >= amount) { messager.village.AddArmy(amount); Empire.instance.capitale.DecreaseArmy(amount); messager.village.AddReputation(20); } }));
                choix.Add(new Dialog.Choix("L'or est précieux, faite bon usage de ces quelques pièce (donner " + (amount + 1 )/ 2 + " Soldats)", delegate () { if (Empire.instance.capitale.army >= (amount + 1 )/ 2) { messager.village.AddArmy((amount + 1 )/ 2); Empire.instance.capitale.DecreaseArmy((amount + 1) / 2); } }));
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
                        message.Add("Bien le bonjour votre excellence! Avez-vous vu le beau temps qu'il y a eu dernierement? Notre recolte a été incroyablement abondante cette saison." + "\n\n" +
                                    "Nous voudrions éventuellement semer d'avantages de graines pour continuer d'avoir autant de réserves de nourritures. Par contre, cela necessiterait de nouveaux investissements majeurs.");
                        choix = new List<Dialog.Choix>();
                        choix.Add(new Dialog.Choix(" Payez entièrement les frais des nouvelles semances (-40 Or , +3 Production D'Or) ()", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.AddReputation(20); messager.village.ModifyGoldProd(3); }));
                        choix.Add(new Dialog.Choix(" Aidez les villagois à construire la mine (-20 Or, +3 Production D'Or)", delegate () { Empire.instance.capitale.DecreaseGold(20); messager.village.DecreaseGold(20); messager.village.ModifyGoldProd(3); }));
                        choix.Add(new Dialog.Choix("Refusez la demande du villagois", delegate () { messager.village.DecreaseReputation(20); }));
                        return;
                    case 2:
                        message.Add("Salutation votre majesté. Je viens du village lointin " + messager.village.nom + " qui aurait grandement besoin de votre support." + "\n\n" +
                                    "En effet, bien que nous ne manquons de rien, nos infrastructures commence à veillir. Certains batiments risque de s'effondrer ou ne sont carément plus déja utilisable");
                        message.Add("Ce que nous aimerions ce serait de pouvoir reconstruire plusieurs batiments pour pouvoir pooursuivre nos activité et ne pas nous ralentir dans notre si bonne lancé" + "\n\n" +
                                     "Êtes-vous en mesure de nous apporter votre aide mon seigneur?");
                        choix = new List<Dialog.Choix>();
                        choix.Add(new Dialog.Choix(" Payez entièrement les frais des nouvelles infrastructures (-40 Or , +3 Production D'Or) ()", delegate () { Empire.instance.capitale.DecreaseGold(40); messager.village.AddReputation(20); messager.village.ModifyGoldProd(3); }));
                        choix.Add(new Dialog.Choix(" Aidez les villagois à construire les infrastructures (-20 Or, +3 Production D'Or)", delegate () { Empire.instance.capitale.DecreaseGold(20); messager.village.DecreaseGold(20); messager.village.ModifyGoldProd(3); }));
                        choix.Add(new Dialog.Choix("Refusez la demande de l'architecte", delegate () { messager.village.DecreaseReputation(20); }));
                        return;
                    default:
                        message.Add("Bonjour notre digne empereur! Je suis du village " + messager.village.nom + " et vous serez heureux d'apprendre que notre économie se porte à merveille!" + "\n\n" +
                                    "Je viens en tant que messager pour vous informer que nous voudrions une aide financière pour investir dans une nouvelle mine d'or.");
                        choix.Add(new Dialog.Choix(" Payez entièrement les frais de constructions de la mine (-40 Or , +3 Production D'Or)", delegate () { messager.village.DecreaseGold(40); messager.village.AddReputation(20); messager.village.ModifyGoldProd(3); }));
                        choix.Add(new Dialog.Choix(" Aidez les villagois à construire la mine (-20 Or, +3 Production D'Or)", delegate () { messager.village.DecreaseGold(20); messager.village.ModifyGoldProd(3); }));
                        choix.Add(new Dialog.Choix(" Refusez la demande du villagois", delegate () { messager.village.DecreaseReputation(20); }));
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
