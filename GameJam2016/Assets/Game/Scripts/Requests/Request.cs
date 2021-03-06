﻿using UnityEngine;
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

    [System.NonSerialized] // TEMPORAIRE
    public Village source = null;
    [System.NonSerialized] // TEMPORAIRE
    public Village destination = null;
    public ResourceType type = ResourceType.gold;//
    public int value = 0;
    public ValueType valueType = ValueType.flat;
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

    /// <summary>
    /// NOTE: pour transféré des ressources de l'empire, mettre un village quelquonque  de l'empire dans la 'source' afin de dépenser les ressources. Sinon, c'est un gain.
    /// </summary>
    public Transaction(Village source, Village destination, ResourceType type, int value, ValueType valueType = ValueType.flat)
    {
        this.source = source;
        this.destination = destination;
        this.type = type;
        this.value = value;
        this.valueType = valueType;
        //this.condition = condition;
    }
    /// <summary>
    /// Used in the editor (request creation)
    /// </summary>
    public Transaction(Id fromId, Id toId, ResourceType type, int value, ValueType valueType = ValueType.flat)
    {
        this.fromId = fromId;
        this.toId = toId;
        this.type = type;
        this.value = value;
        this.valueType = valueType;
        //this.condition = condition;
    }

    public void Execute()
    {
        int amount = GetFlatAmount();

        //Si la resource ciblé est une resource de l'empire, on fait un gain / une perte de resource ET NON un transfert
        if (GameResources.IsTypeEmpire(type))
        {
            //Si le village 'source' n'est pas nul et qu'il appartient à l'empire, alors on inverse le montant à ajouter
            if (source != null && Universe.Empire.Has(source))
                amount = -amount;
            Universe.Empire.Add(GameResources.Convert_ToEmpire(type), amount);
        }

        //Sinon, on fait un transfert traditionel entre 2 village
        else
            Village.Transfer(source, destination, GameResources.Convert_ToVillage(type), amount);
    }

    private int GetFlatAmount()
    {
        switch (valueType)
        {
            case ValueType.destPercent:
                if (destination == null) { Debug.LogError("Error on transfer type 'destPercent'. Destination is null."); return 0; }
                else
                {
                    if (GameResources.IsTypeEmpire(type))
                        return Mathf.RoundToInt((float)value * Universe.Empire.Get(GameResources.Convert_ToEmpire(type)) / 100f);
                    else
                        return Mathf.RoundToInt((float)value * destination.Get(GameResources.Convert_ToVillage(type)) / 100f);
                }
            case ValueType.sourcePercent:
                if (source == null) { Debug.LogError("Error on transfer type 'sourcePercent'. Source is null."); return 0; }
                else
                {
                    if (GameResources.IsTypeEmpire(type))
                        return Mathf.RoundToInt((float)value * Universe.Empire.Get(GameResources.Convert_ToEmpire(type)) / 100f);
                    else
                        return Mathf.RoundToInt((float)value * source.Get(GameResources.Convert_ToVillage(type)) / 100f);
                }
            default:
                return value;
        }
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
    public void Fill(Village source, Village destination, int value, ResourceType type = ResourceType.custom)
    {
        this.value = value;
        if (type != ResourceType.custom && this.type == ResourceType.custom) this.type = type;
        Fill(source, destination);
    }

    public void Split(out Transaction sourceTransaction, out Transaction destinationTransaction)
    {
        int flatValue = GetFlatAmount();

        sourceTransaction = new Transaction(source, null, type, flatValue, ValueType.flat);
        destinationTransaction = new Transaction(null, destination, type, flatValue, ValueType.flat);
    }
}

//public class Condition
//{
//    System.Func<bool> condition = null;
//    public Condition(System.Func<bool> condition)
//    {
//        this.condition = condition;
//    }

//    public static implicit operator bool (Condition condition)
//    {
//        if (condition.condition == null) return true;
//        return condition.condition();
//    }
//}
[System.Serializable]
public class Choice
{
    public string text = "";
    public Command command = null;
    public List<Transaction> transactions;
    public Condition condition;

    public Choice() { }
    public Choice(string text, Condition condition, Command command = null, List<Transaction> transactions = null)
    {
        this.text = text;
        this.transactions = transactions;
        this.command = command;
        this.condition = condition;
    }

    public bool IsAvailable()
    {
        return condition != null ? condition.Execute() : true;
    }

    public void Choose()
    {
        if (transactions != null)
            foreach (Transaction transaction in transactions) transaction.Execute();

        if (command != null) command.Execute();
    }
}

[System.Serializable]
public class Request
{
    public Dialog.Message message;
    public List<Choice> choix = new List<Choice>();

    public int delay;
    IKit characterKit = null;

    public void SetCharacterKit(IKit kit)
    {
        characterKit = kit;
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
        SetCharacterKit(CharacterBank.GetKit(CharacterBank.StandardTags.Philosopher));
    }

    public void DoRequest()
    {
        CharacterEnter.Enter(OnCharacterEnter, characterKit);
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
        RequestManager.OnRequestComplete();
    }
}
