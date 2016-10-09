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

    public Request (List<string> message, List<Dialog.Choix> choix)
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
