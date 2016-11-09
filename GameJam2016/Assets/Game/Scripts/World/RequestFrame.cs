using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TransactionFrame : Transaction
{
    public enum Id { source, destination, Null}
    public Id from = Id.Null;
    public Id to = Id.Null;
    public TransactionFrame()
    {

    }
    public TransactionFrame(Id from, Id to, Ressource_Type type, int value, ValueType valueType = ValueType.flat, Condition condition = null)
        : base(null, null, type, value, valueType, condition)
    {
        this.from = from;
        this.to = to;
    }
    public void Build(Village source, Village destination)
    {
        if (from == Id.source)
            this.source = source;
        else if (from == Id.destination)
            this.source = destination;
        else
            this.source = null;

        if (to == Id.source)
            this.destination = source;
        else if (to == Id.destination)
            this.destination = destination;
        else
            this.destination = null;
    }
}

[CreateAssetMenu(menuName = "Request Frame")]
public class RequestFrame : ScriptableObject
{
    public string tag = "exemple_village_need_gold";
    public string text = "Entrez une message...";
    public char forceSeparation = '%';
    public List<Choice> choices = new List<Choice>(3);
    public Condition condition = null;

    Village source = null; //Demandant
    Village destination = null; //Demandé
    //  ex: village demande de la nourriture à la capitale.
    //      source = village
    //      destination = capitale
    //  ex: évenement random où une inondation ravage la capitale.
    //      source = null
    //      destination = capitale

    public Request Build(Village source, Village destination)
    {
        this.source = source;
        this.destination = destination;

        text = Filter(text, source, destination);  //Filtre le text
        if (choices != null)
            foreach (Choice choice in choices)
            {
                choice.text = Filter(choice.text, source, destination);  //Filtre le text des choix multiple
                if (choice.transactions != null)
                    foreach (Transaction transaction in choice.transactions)
                        if (transaction is TransactionFrame) (transaction as TransactionFrame).Build(source, destination);
            }

        Dialog.Message message = new Dialog.Message(text, forceSeparation);
        Request request = new Request(message, choices);

        request.condition = condition;

        return request;
    }

    string Filter(string text, Village source, Village destination)
    {
        if(source != null)
        {
            text = text.Replace("[source.name]", source.nom);
            text = text.Replace("[source.food]", source.GetFood().ToString());
            text = text.Replace("[source.foodProd]", source.GetFoodProd().ToString());
            text = text.Replace("[source.army]", source.GetArmy().ToString());
            text = text.Replace("[source.armyProd]", source.GetArmyProd().ToString());
            text = text.Replace("[source.gold]", source.GetGold().ToString());
            text = text.Replace("[source.goldProd]", source.GetGoldProd().ToString());
        }
        if(destination != null)
        {
            text = text.Replace("[destination.name]", destination.nom);
            text = text.Replace("[destination.food]", destination.GetFood().ToString());
            text = text.Replace("[destination.foodProd]", destination.GetFoodProd().ToString());
            text = text.Replace("[destination.army]", destination.GetArmy().ToString());
            text = text.Replace("[destination.armyProd]", destination.GetArmyProd().ToString());
            text = text.Replace("[destination.gold]", destination.GetGold().ToString());
            text = text.Replace("[destination.goldProd]", destination.GetGoldProd().ToString());
        }
        return text;
    }

    public void CustomFrames(int index)
    {
        switch (index)
        {
            case 0:
                //La requete ne se fait meme pas si la destination n'a pas 10 soldat
                condition = new Condition(delegate 
                {
                    return destination.GetArmy() > 10;
                });

                tag = "exemple_village_need_food";
                text = "Je suis un messager venant du village de [source.name].\n\nNos citoyen sont amateur de PFK. Nous désirons donc acheter votre poule.";

                List<Transaction> choixUnTrans = new List<Transaction>();                           //Transaction du choix 1
                choixUnTrans.Add(new TransactionFrame(TransactionFrame.Id.source, TransactionFrame.Id.destination, Ressource_Type.gold, 10));       //Le village donne de l'or a la capital
                choixUnTrans.Add(new TransactionFrame(TransactionFrame.Id.Null, TransactionFrame.Id.source, Ressource_Type.food, 1));                //Le village gagne 1 de food

                List<Transaction> choixDeuxTrans = new List<Transaction>();                      //Transaction du choix 2
                choixDeuxTrans.Add(new TransactionFrame(TransactionFrame.Id.Null, TransactionFrame.Id.destination, Ressource_Type.happiness, 2));    //La capital gagne 2 de bonheur

                List<Transaction> choixTroisTrans = null;                                           //Transaction du choix 3 (aucune)

                choices.Add(                                                                                                          //Premier choix
                    new Choice(
                        "Premier choix: Vendre la poule",                                                                                   //Message
                        delegate { Debug.Log("Ceci est un custom callback, utile quand on veut faire des actions unique custom. "); },       //Custom callback
                        choixUnTrans                                                                                                        //Transactions
                        )
                    );
                choices.Add(                                                                                                          //Deuxieme choix
                    new Choice(
                        "Deuxieme choix: Garder la poule et l'engager dans un cirque.",                                                     //Message
                        null,                                                                                                               //Custom callback
                        choixDeuxTrans                                                                                                      //Transactions
                        )
                    );
                choices.Add(                                                                                                          //Troisieme choix
                    new Choice(
                        "Troisieme choix: Regarder le mur.",                                                                                //Message
                        null,                                                                                                               //Custom callback
                        choixTroisTrans                                                                                                     //Transactions
                        )
                    );
                break;
        }
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(RequestFrame))]
//public class RequestFrameEditor : Editor
//{

//}
//#endif