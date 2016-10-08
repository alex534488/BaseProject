using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Request {

    List<string> message = new List<string>();
    List<Dialog.Choix> choix = new List<Dialog.Choix>();
    public bool choosen = false;

    public Request(Ressource_Type resource, int amount)
    {
        switch (resource)
        {
            case Ressource_Type.gold:
                message.Add("...");
                choix.Add(new Dialog.Choix("Choix 1:...", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 2:...", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 3:...", delegate () { }));
                return;
            case Ressource_Type.food:
                message.Add("...");
                choix.Add(new Dialog.Choix("Choix 1:...", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 2:...", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 3:...", delegate () { }));
                return;
            case Ressource_Type.army:
                message.Add("...");
                choix.Add(new Dialog.Choix("Choix 1:...", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 2:...", delegate () { }));
                choix.Add(new Dialog.Choix("Choix 3:...", delegate () { }));
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
        Dialog.DisplayText(message, choix,RequestManager.DoNextRequest);
    }
}
