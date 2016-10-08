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

    public void Awake()
    {
        if (requestManager == null) requestManager = this;
        CreateRandomRequest();
        onWaitingForRequest.AddListener(GetRequests);
    }

    void GetRequests()
    {
        //if(listRequest.Count <= nbRequest){ GetRandomRequest(nbRequest - listRequest.Count); } nombre de requete limite

        GetRandomRequest(10);

        foreach (Request request in listRequest)
        {
            request.DoRequest();
            request.choosen = false;
            listRequest.Remove(request);
        }
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

    void CreateRandomRequest()
    {
        // LISTE DES REQUETES ALEATOIRES

        // SAMPLE - CREATION D'UNE REQUETE
        List<string> listMessage1 = new List<string>();
        listMessage1.Add("Illius : Ave ! Je viens devant votre jugement car cet homme vient de voler mon cochon ! J’exige qu'il me le rende et qu'il soit punit !");
        listMessage1.Add("Maximus : Mensonge monseigneur, ce cochon est le mien depuis des lunes !");
        List<Dialog.Choix> listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Coupez-moi ce cochon en deux et partagé les morceaux avec la communauté.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Puisque ce cochon vous pose problème, je vais vous le retirer.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Tant de vigueur pour un cochon, vous ferez de bon soldat !", delegate () { }));
        Request request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////

        // SAMPLE - CREATION D'UNE REQUETE
        listMessage1 = new List<string>();
        listMessage1.Add("Maxima : Bien le bonjour mon seigneur. Je viens humblement devant vous pour obtenir vos faveurs.");
        listMessage1.Add("Mon mari vient d'être enrôlé dans l’armée, malheureusement, il était essentiel à la survit de notre pauvre famille.");
        listMessage1.Add("Si vous pouvez simplement lui permettre de revenir auprès des siens, je vous en serai éternellement reconnaissante.");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("J’entends votre requête et je vais faire le nécessaire pour qu'il retourne auprès de sa famille.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Chaque soldat à un rôle crucial dans l'armée, mais j'ai espoir que cette compensation en or puisse couvrir vos besoins pendant son absence.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Malheureusement, en ces temps difficiles, chaque famille porte la même histoire, je ne peux accéder à votre requête.", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);

        ///////////////////////////////////////////////////////////////////

        listMessage1 = new List<string>();
        listMessage1.Add("Faustus : '' Seigneur, un énorme incendie est en train de ravager la cité !");
        listMessage1.Add("Tous les citoyens son au pied d’œuvre pour contenir l'incendie.");
        listMessage1.Add("Nous demandons exceptionnellement l'intervention de l'armée nous aider.");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Sauvez les femmes et les enfants en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Sauvez les réserve de nourritures en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Que les armées franchisent le Rubicon et aillent aider Rome !", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////
        listMessage1 = new List<string>();
        listMessage1.Add("Proculus : ''Ave ! Je viens d'apercevoir douze vautours dans le ciel ! Les augures sont avec nous !");
        listMessage1.Add("D'après vous, quelles bonnes nouvelles nous annonce ces augures ?");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("D'excellente récolte évidement !", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Cela annonce des réussites militaires !", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Cela annonce un retour prochain à la Pax Romana !", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////
        listMessage1 = new List<string>();
        listMessage1.Add("Secundus : Seigneur, une maladie étrange décimes nos troupeaux !");
        listMessage1.Add(" Nous avons trouvé un sage qui prétend pouvoir stopper la maladie, mais il demande une compensation financière !");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Abattez les troupeaux pour contenir la maladie !", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Payer ce sage, et faite lui comprendre qu'il a tous intérêts à stopper la maladie s’il veut conserver sa tête !", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Faite pendre cet escroc et réquisitionnez ses affaires personnelles !", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////
        listMessage1 = new List<string>();
        listMessage1.Add("Statius :  Seigneur, nous avons intercepté une cargaison de contrebande de nourriture. ");
        listMessage1.Add("Que devons nous faire de la cargaison ?");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Faite venir la cargaison dans les greniers de la ville.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Organiser un banquet pour tous avec la cargaison.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Vendez la cargaison et faite moi parvenir les recettes.", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////
        listMessage1 = new List<string>();
        listMessage1.Add("Octavius : Jupiter est en colère monseigneur ! Ses foudres scient le ciel en ce moment même !");
        listMessage1.Add("Que devons nous faire pour apaiser le dieu des dieux ?");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Sacrifiez du bétails !", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Organisez une fête populaire où le bétail sera sacrifié et la population rassasié.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Jupiter n'est qu'une représentation de Dieu ! Que la population se repentisse et le ciel se calmera !", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////
        listMessage1 = new List<string>();
        listMessage1.Add("Hostus : Empereur, le sénat est inquiet de votre politique actuel et demande des précisions sur les futures évolutions de l'empire.");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Dites-leur que l'empire va se concentrer sur la production de nourriture dans les prochains jours.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Dites-leur que l'armée et la sécurité de toutes les villes romaines sont ma principale préoccupation.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Dites-leur que le bonheur de chaque citoyen de capital est la priorité de l’Empereur.", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////

        listMessage1 = new List<string>();
        listMessage1.Add("Valentinus : Les relations entre païen et chrétien sont de plus en plus tendus monseigneur !");
        listMessage1.Add(" Mais prendre une position ouverte ne fera qu'attiser les tensions.");
        listMessage1.Add("Je vous propose d'autres options moins conventionnelle mais sans doutes plus adaptés.");
        listeChoix1 = new List<Dialog.Choix>();
        listeChoix1.Add(new Dialog.Choix("Discréditez les représentant des chrétiens avec des rumeurs de péchés capitaux.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Créez de faux mauvais augures pour apeurer les Païens.", delegate () { }));
        listeChoix1.Add(new Dialog.Choix("Créez l'étincelle qui enflammera le débat, et envoyez l'armée pour nettoyer les rue de ces croyants imbéciles !", delegate () { }));
        request1 = new Request(listMessage1, listeChoix1);
        listRequest.Add(request1);


        ///////////////////////////////////////////////////////////////////


    }
}
