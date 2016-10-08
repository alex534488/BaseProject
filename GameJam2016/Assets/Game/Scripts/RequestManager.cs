using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RequestManager : MonoBehaviour {

    static RequestManager requestManager; 

    // public int nbRequest = 5;
    List<Request> listRequest = new List<Request>();
    List<Request> listRandomRequest = new List<Request>();
    public UnityEvent onWaitingForRequest = new UnityEvent();

    void Start()
    {
        CreateRandomRequest();
        onWaitingForRequest.AddListener(GetRequests);
    }

    void GetRequests()
    {
        //if(listRequest.Count <= nbRequest){ GetRandomRequest(nbRequest - listRequest.Count); } nombre de requete limite

        GetRandomRequest(10);

        request.DoRequest();
        request.choosen = false;
        listRequest.Remove(request);
    }

    public static void SendRequest(Request request)
    {
        requestManager.listRequest.Add(request);

        /* Ancien systeme pour un nombre maximum de requetes
        if (listRequest.Count <= nbRequest)
        {
            request.choosen = true;
            listRequest.Add(request);
        }
        // On prend les requetes restante et on...
        */
    }

    void GetRandomRequest(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Request choosenRequest = listRandomRequest[Random.Range(0, listRandomRequest.Count)];
            if (choosenRequest.choosen) continue;
            listRequest.Add(choosenRequest);
        }
    }

    void DoNextRequest()
    {
        Request current = listRequest[0];
        current.DoRequest();
        current.choosen = false;
        listRequest.Remove(current);
    }

    void CreateRandomRequest()
    {
        // LISTE DES REQUETES ALEATOIRES

        // SAMPLE - CREATION D'UNE REQUETE
        List<string> listMessage1 = new List<string>();
        listMessage1.Add("...");
        List<Dialog.Choix> listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Choix 1:...", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Choix 2:...", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Choix 3:...", delegate () { }));
        Request request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);

        ///////////////////////////////////////////////////////////////////



    }
}
