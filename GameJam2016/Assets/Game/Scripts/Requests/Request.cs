using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


[System.Serializable]
public class Transaction
{
    [System.Serializable]
    public enum Id { source, destination, Null }
    [System.Serializable]
    public enum ValueType { flat, sourcePercent, destPercent }

    public Village source = null;
    public Village destination = null;
    public Resource_Type type = Resource_Type.gold;
    public int value = 0;
    public ValueType valueType = ValueType.flat;
    public Condition condition = null;
    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public Id fromId = Id.Null;
    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public Id toId = Id.Null;
    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public string fillValue = "0";

    public Transaction() { }
    public Transaction(Village source, Village destination, Resource_Type type, int value, ValueType valueType = ValueType.flat, Condition condition = null)
    {
        this.source = source;
        this.destination = destination;
        this.type = type;
        this.value = value;
        this.valueType = valueType;
        this.condition = condition;
    }
    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public Transaction(Id fromId, Id toId, Resource_Type type, int value, ValueType valueType = ValueType.flat, Condition condition = null)
    {
        this.fromId = fromId;
        this.toId = toId;
        this.type = type;
        this.value = value;
        this.valueType = valueType;
        this.condition = condition;
    }

    public void Execute()
    {
        if (condition != null && !condition) return;

        int amount = value;

        switch (valueType)
        {
            case ValueType.flat:
                amount = value;
                break;
            case ValueType.destPercent:
                if (destination == null) { Debug.LogError("Error on transfer type 'destPercent'. Destination is null."); amount = 0; }
                else
                    amount = Mathf.RoundToInt((float)value * destination.GetResource(type) / 100f);
                break;
            case ValueType.sourcePercent:
                if (source == null) { Debug.LogError("Error on transfer type 'sourcePercent'. Source is null."); amount = 0; }
                else
                    amount = Mathf.RoundToInt((float)value * source.GetResource(type) / 100f);
                break;
        }

        Village.Transfer(source, destination, type, amount);
    }

    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public void Fill(Village source, Village destination)
    {
        if (fromId == Id.source)
            this.source = source;
        else if (fromId == Id.destination)
            this.source = destination;
        else
            this.source = null;

        if (toId == Id.source)
            this.destination = source;
        else if (toId == Id.destination)
            this.destination = destination;
        else
            this.destination = null;
    }
    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public void Fill(Village source, Village destination, int value)
    {
        this.value = value;
        Fill(source, destination);
    }
}

[System.Serializable]
public class Condition
{
    System.Func<bool> condition = null;
    public Condition(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public static implicit operator bool (Condition condition)
    {
        if (condition.condition == null) return true;
        return condition.condition();
    }
}
[System.Serializable]
public class Choice
{
    public string text = "";
    public UnityAction customCallBack = null;
    public List<Transaction> transactions;

    public Choice() { }
    public Choice(string text, UnityAction callback = null, List<Transaction> transactions = null)
    {
        this.text = text;
        this.transactions = transactions;
        customCallBack = callback;
    }

    public void Choose()
    {
        if(transactions != null)
            foreach (Transaction transaction in transactions) transaction.Execute();

        if (customCallBack != null) customCallBack();
    }
}

public class Request
{
    public Dialog.Message message;
    List<Choice> choix = new List<Choice>();

    public Condition condition = null;
    public int delay;


    // REQUETE MORT
    public Request(Seigneur messager)
    {
        message = new Dialog.Message("Bonjour Empereur. J'ai de mauvaises nouvelles. Le village " + messager.village.nom + " a malheureusement été détruit par une invasion barbare.");
        choix.Add(new Choice("C'est malheureux en effet, mais l'Empire survivra!", delegate () { }));
    }

    // REQUETE D'AIDE
    public Request(Seigneur messager, Resource_Type resource, int amount)
    {
        switch (resource)
        {
            case Resource_Type.gold:
                message = new Dialog.Message("Le village " + messager.village.nom + " a besoin de " + amount + " pièces d'or pour combler ses manquements économiques");
                choix.Add(new Choice("Chaque village de l'Empire compte! (" + amount + " Or, + Réputation)", delegate () { messager.village.AddGold(amount); Empire.instance.capitale.AddGold(-amount); messager.village.AddReputation(20); }));
                choix.Add(new Choice("L'or est précieux, faites bon usage de ces quelques pièces \n(" + (amount + 1) / 2 + " Or)", delegate () { messager.village.AddGold((amount + 1) / 2); Empire.instance.capitale.AddGold(-(amount + 1) / 2); }));
                choix.Add(new Choice("Les caisses sont vides pour vous! (- Réputation)", delegate () { messager.village.AddReputation(-20); }));
                return;
            case Resource_Type.food:
                message = new Dialog.Message("Le village " + messager.village.nom + " a besoin de " + amount + " Nourritures pour nourrir les soldats stationnés dans notre village.");
                choix.Add(new Choice("Chaque village de l'empire compte! (-" + amount + " Nourritures, + Réputation)", delegate () { messager.village.AddFood(amount); Empire.instance.capitale.AddFood(-amount); messager.village.AddReputation(20); }));
                choix.Add(new Choice("Je peux vous fournir quelques rations de Nourritures mon cher. \n(-" + (amount + 1) / 2 + " Nourriture)", delegate () { messager.village.AddFood((amount + 1) / 2); Empire.instance.capitale.AddFood(-(amount + 1) / 2); }));
                choix.Add(new Choice("Rome est au bord de la famine également. (- Réputation)", delegate () { messager.village.AddReputation(-20); }));
                return;
            case Resource_Type.army:
                message = new Dialog.Message("Le village " + messager.village.nom + " a besoin de " + amount + " Soldats pour se défendre contre une invasion imminente de barbares.");
                choix.Add(new Choice("Voici davantage de soldats que nécessaire ! \n(" + Mathf.CeilToInt((amount) * 1.5f) + " Soldats)", delegate () { messager.village.AddArmy(Mathf.CeilToInt((amount) * 1.5f)); Empire.instance.capitale.AddArmy(-Mathf.CeilToInt((amount) * 1.5f)); messager.village.AddReputation(20); }));
                choix.Add(new Choice("Voici le nombre minimum de soldats nécessaire! (" + amount + " Soldats)", delegate () { messager.village.AddArmy(amount); Empire.instance.capitale.AddArmy(-amount); }));
                choix.Add(new Choice("Les barbares sont à nos portes également.", delegate () { messager.village.AddReputation(-20); }));
                return;
            default:
                return;
        }
    }

    // INVESTISSEMENT

    public Request(Seigneur messager, Resource_Type resource)
    {
        int random = Mathf.CeilToInt(Random.Range(0, 3));

        switch (resource)
        {
            case Resource_Type.gold:
                switch (random)
                {
                    case 1:
                        message = new Dialog.Message("Je représente le village " + messager.village.nom + "\n\nNotre récolte a été incroyablement abondante cette saison." + "\n\n" +
                                    "Nous voudrions semer davantage pour améliorer nos réserves de nourritures. \n\nPar contre, cela necessiterait de nouveaux investissements majeurs.");
                        choix.Add(new Choice("Payez entièrement les frais des nouvelles semances \n(-40 Or, +2 Production Nourriture, + Réputation)", delegate () { Empire.instance.capitale.AddGold(-40); messager.village.AddReputation(20); messager.village.AddFoodProd(2); }));
                        choix.Add(new Choice("Aidez les villagois à construire la mine \n(-20 Or Capitale, -20 Or Village, +2 Production Nourriture)", delegate () { Empire.instance.capitale.AddGold(-20); messager.village.AddGold(-20); messager.village.AddFoodProd(2); }));
                        choix.Add(new Choice("Refusez la demande du villageois (- Réputation)", delegate () { messager.village.AddReputation(-20); }));
                        return;
                    case 2:
                        message = new Dialog.Message("Salutation votre majesté. Je viens du village " + messager.village.nom + " qui pourrait bénéficier de votre support." + "\n\n"
                                   + "En effet, bien que nous ne manquons de rien, nos infrastructures commencent à veillir. \n\nCertains batiments risquent de s'éffondrer ou ne sont carrément plus utilisables"
                                   + "Nous aimerions reconstruire quelques batiments de votre choix afin de pour pouvoir poursuivre nos activités" + "\n\n"
                                   + "Êtes-vous en mesure de nous apporter votre aide mon seigneur?");
                        choix.Add(new Choice("Réparez toutes les fermes de votre village \n(-40 Or Capitale, -20 Or Village, +4 Production Or, + Réputation)", delegate () { Empire.instance.capitale.AddGold(-40); messager.village.AddGold(-20); messager.village.AddReputation(20); messager.village.AddGoldProd(4); }));
                        choix.Add(new Choice("Réparez les exploitations minières de votre village \n(-20 Or Capitale, -20 Or Village, +2 Production Nourriture, + Réputation)", delegate () { Empire.instance.capitale.AddGold(-20); messager.village.AddGold(-20); messager.village.AddFoodProd(2); messager.village.AddReputation(20); }));
                        choix.Add(new Choice("Refusez la demande de l'architecte (- Réputation)", delegate () { messager.village.AddReputation(-20); }));
                        return;
                    default:
                        message = new Dialog.Message("Je suis du village " + messager.village.nom + " et vous serez heureux d'apprendre que notre économie se porte à merveille!" + "\n\n" +
                                    "Je viens en tant que messager pour vous informer que nous voudrions une aide financière pour investir dans une nouvelle mine d'or.");
                        choix.Add(new Choice(" Payez entièrement les frais de constructions de la mine \n(-40 Or , +4 Production Or, + Réputation)", delegate () { Empire.instance.capitale.AddGold(-40); messager.village.AddReputation(20); messager.village.AddGoldProd(4); }));
                        choix.Add(new Choice(" Aidez les villagois à construire la mine \n(-20 Or Capitale, -20 Or Village, +4 Production Or)", delegate () { Empire.instance.capitale.AddGold(-20); messager.village.AddGold(-20); messager.village.AddGoldProd(4); }));
                        choix.Add(new Choice(" Refusez la demande du villagois (- Réputation)", delegate () { messager.village.AddReputation(-20); }));
                        return;
                }
            case Resource_Type.food:
                switch (random)
                {
                    default:
                        return;
                }
            case Resource_Type.army:
                switch (random)
                {
                    default:
                        return;
                }
            default:
                return;
        }
    }

    // REQUETE CHARIOT
    public Request(Carriage carriage, int amount)
    {
        if (amount == -1)
        {
            message = new Dialog.Message(" Notre empereur, la carravan qui était parti pour " + carriage.destination.nom + " est revenu vide car le village avait été détruit.");
            choix.Add(new Choice("C'est vraiment dommage, l'Empire avait besoin de ces resources.", delegate () { }));
        }
        else if (amount == 0)
        {
            message = new Dialog.Message(" Notre empereur, la carravan qui était parti pour " + carriage.destination.nom + " est revenu vide car le seigneur du village a refusé de nous donner de ses ressources.");
            choix.Add(new Choice("C'est vraiment dommage, l'Empire avait besoin de ces resources.", delegate () { }));
        }
        else
        {
            message = new Dialog.Message(" Notre empereur, nous sommes de retour de " + carriage.destination.nom + " et nous avons pris les ressources demandées au village soit " + amount + " de " + carriage.resource + ".");
            choix.Add(new Choice("Parfait! Merci beaucoup.", delegate () {  carriage.provenance.AddResource(carriage.resource, (-1 * carriage.amount)); }));
        }
    }

    // REQUETE CLASSIQUE
    public Request(Dialog.Message message, List<Choice> choix)
    {
        this.message = message;
        this.choix = choix;
    }

    public Request()
    {
        message = new Dialog.Message(" Bonjour Empereur, votre récente nomination inquiète beaucoup de citoyens au sein de la capitale. En tant que conseiller laissez moi vous donnez quelques indications quant a la facon de gouverner un Empire."
            + " Chaque jour, des personnes provenant de tout votre empire vont venir vous voir pour faire des requêtes à l'État, libre à vous de les accepter ou non. Dépendemment de vos choix vous allez pouvoir constater les impacts."
            + " En effet, à votre gauche vous avez un indication de resources de votre Capitale. La quantité de soldat, le bonheur de la cité, la quantité de nourriture et d'or sont représenté.\n\n"
            + "À chaque tour vous avez une production de nourriture et d'or, mais les autres attributs peuvent diminuer aussi en fonction de votre gouvernance."
            + " Afin de bien gérer vos décissions et leur potentiel impacte sur l'Empire, vous avez a votre droite, vos trois conseillers. Ils servent tous à avoir de l'information sur les villages compris au sein de votre empire et leur situation. " + "\n\n"
            + " Il vous est possible de demander ou de fournir des ressources à ces villages à l'aide de vos caravanes. À noter que les villes vont vous faire des demandes à l'occasion dépendemment de leur besoin."
            + " Également, il ne faut pas oublier les barbares peuvent attaquer à tout moment votre empire. Des villages pourraient se faire détruire s'il n'y a pas une armé assez suffisante.\n\n"
            + "Pour avoir une idée globale des chose vous pouvez envoyer un éclaireur qui récoltera de l'information sur les déplacements des barbares ce qui permettera aux villes de vous avertir s'elles sont en difficultées."
            + " Finalement, il ne faut pas oublier que lorsque vous avez terminer de gérer votre empire pour la journée en cours, il faut appuyer sur le bouton Prochain Jour afin de passer à la prochaine journée." + "\n\n"
            + "Un icone en haut au centre permet de signaler votre gouvernance a débuter depuis combien de temps."
            + "% Je crois que j'ai bien compris, je suis prêt à Gouverner!");

    }

    public void DoRequest()
    {
        if (condition != null && !condition) Complete(); //Si la condition n'est pas remplie, ne fait pas la request
        else CharacterEnter.Enter(OnCharacterEnter);
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
        Complete();
    }
    void Complete()
    {
        RequestManager.DoNextRequest();
    }
}
