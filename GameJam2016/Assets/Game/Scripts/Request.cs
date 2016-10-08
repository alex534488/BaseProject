using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Request {

    public Seigneur messager;

    List<string> message = new List<string>();
    List<Dialog.Choix> choix = new List<Dialog.Choix>();
    public bool choosen = false;

    public Request(Seigneur messager,Ressource_Type resource, int amount, bool asking = false, bool give = false)
    {

        switch (resource)
        {
            case Ressource_Type.gold:
                message.Add("Bla Gold Bla");
                choix.Add(new Dialog.Choix("Choix 1: Donner ben du gold", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 2: Donner un peu de gold", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 3: Fuck you", delegate () { }));
                return;
            case Ressource_Type.food:
                message.Add("Bla Food Bla");
                choix.Add(new Dialog.Choix("Choix 1: Donner ben du bouffe", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 2: Donner un peu de bouffe", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 3: Fuck you", delegate () { }));
                return;
            case Ressource_Type.army:
                message.Add("Bla Army Bla");
                choix.Add(new Dialog.Choix("Choix 1: Donner ben des gars", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 2: Donner un peu de gars", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 3: Fuck you", delegate () { }));
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
