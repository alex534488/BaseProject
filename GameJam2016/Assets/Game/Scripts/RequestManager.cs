using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RequestManager : MonoBehaviour
{

    static RequestManager requestManager;

    List<Request> listRequest = new List<Request>();
    List<Request> listRandomRequest = new List<Request>();

    public UnityEvent OnCompletionOfRequests = new UnityEvent();

    void Awake()
    {
        if (requestManager == null) requestManager = this;
        GenerateRandomRequests();
    }

    public void NewDay()
    {
        GetRandomRequest(1);

        listRequest[0].DoRequest();
    }

    public static void SendRequest(Request request)
    {
        requestManager.listRequest.Add(request);
    }

    void GetRandomRequest(int amount)
    {
        listRequest.Add(listRandomRequest[0]);
        listRandomRequest.Remove(listRandomRequest[0]);
        if(listRandomRequest.Count <=0) { GenerateRandomRequests(); }
    }

    public static void DoNextRequest()
    {
        requestManager.listRequest.Remove(requestManager.listRequest[0]);
        if(requestManager.listRequest.Count <= 0)
        {
            //requestManager.listRequest.Clear();
            requestManager.OnCompletionOfRequests.Invoke();
            return;
        }
        requestManager.listRequest[0].DoRequest();
    }

    void GenerateRandomRequests()
    {
        // LISTE DES REQUETES ALEATOIRES

        // SAMPLE - CREATION D'UNE REQUETE
        List<string> listMessage = new List<string>();
        listMessage.Add("Illius : Ave ! Je viens devant votre jugement car cet homme vient de voler mon cochon ! J’exige qu'il me le rende et qu'il soit punit !");
        listMessage.Add("Maximus : Mensonge monseigneur, ce cochon est le mien depuis des lunes !");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Coupez-moi ce cochon en deux et partagé les morceaux avec la communauté.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Puisque ce cochon vous pose problème, je vais vous le retirer.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Tant de vigueur pour un cochon, vous ferez de bon soldat !", delegate () { }));
        Request request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        
        listMessage = new List<string>();
        listMessage.Add("Maxima : Bien le bonjour mon seigneur. Je viens humblement devant vous pour obtenir vos faveurs.");
        listMessage.Add("Mon mari vient d'être enrôlé dans l’armée, malheureusement, il était essentiel à la survit de notre pauvre famille.");
        listMessage.Add("Si vous pouvez simplement lui permettre de revenir auprès des siens, je vous en serai éternellement reconnaissante.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("J’entends votre requête et je vais faire le nécessaire pour qu'il retourne auprès de sa famille.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Chaque soldat à un rôle crucial dans l'armée, mais j'ai espoir que cette compensation en or puisse couvrir vos besoins pendant son absence.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Malheureusement, en ces temps difficiles, chaque famille porte la même histoire, je ne peux accéder à votre requête.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);

        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Faustus : '' Seigneur, un énorme incendie est en train de ravager la cité !");
        listMessage.Add("Tous les citoyens sont au pied d’œuvre pour contenir l'incendie.");
        listMessage.Add("Je sais que les soldats, par tradition, n'ont pas le droit de franchir le Rubicon, mais nous demandons exceptionnellement l'intervention de l'armée nous aider.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sauvez les femmes et les enfants en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Sauvez les réserve de nourritures en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Que les armées franchisent le Rubicon et aillent aider Rome !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Proculus : ''Ave ! Je viens d'apercevoir douze vautours dans le ciel ! Les augures sont avec nous !");
        listMessage.Add("Tous comme Romulus à la fondation de Rome!");
        listMessage.Add("D'après vous, quelles bonnes nouvelles nous annonce ces augures ?");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("D'excellente récolte évidement !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Cela annonce des réussites militaires !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Cela annonce un retour prochain à la Pax Romana !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Secundus : Seigneur, une maladie étrange décimes nos troupeaux !");
        listMessage.Add(" Nous avons trouvé un sage qui prétend pouvoir stopper la maladie, mais il demande une compensation financière !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Abattez les troupeaux pour contenir la maladie !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Payer ce sage, et faite lui comprendre qu'il a tous intérêts à stopper la maladie s’il veut conserver sa tête !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Faite pendre cet escroc et réquisitionnez ses affaires personnelles !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Statius :  Seigneur, nous avons intercepté une cargaison de contrebande de nourriture. ");
        listMessage.Add("Que devons nous faire de la cargaison ?");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Faite venir la cargaison dans les greniers de la ville.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Organiser un banquet pour tous avec la cargaison.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Vendez la cargaison et faite moi parvenir les recettes.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Octavius : Jupiter est en colère monseigneur ! Ses foudres scient le ciel en ce moment même !");
        listMessage.Add("Que devons nous faire pour apaiser le dieu des dieux ?");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sacrifiez du bétails !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Organisez une fête populaire où le bétail sera sacrifié et la population rassasié.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Jupiter n'est qu'une représentation du seul et unique Dieu ! Je ne craint pas ses foudres!", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Hostus : Empereur, le sénat est inquiet de votre politique actuel et demande des précisions sur les futures évolutions de l'empire.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Dites-leur que l'empire va se concentrer sur la production de nourriture dans les prochains jours.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Dites-leur que l'armée et la sécurité de toutes les villes romaines sont ma principale préoccupation.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Dites-leur que le bonheur de chaque citoyen de capital est la priorité de l’Empereur.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Valentinus : Les relations entre païen et chrétien sont de plus en plus tendus monseigneur !");
        listMessage.Add(" Mais prendre une position ouverte ne fera qu'attiser les tensions.");
        listMessage.Add("Je vous propose d'autres options moins conventionnelle mais sans doutes plus adaptés.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Discréditez les représentant des chrétiens avec des rumeurs de péchés capitaux.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Créez de faux mauvais augures pour apeurer les Païens.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Créez l'étincelle qui enflammera le débat, et envoyez l'armée pour nettoyer les rue de ces croyants imbéciles !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

<<<<<<< HEAD
        listRandomRequest.Sort((a, b) => 1 - 2 * Random.Range(0, 1)); // Shuffle
=======
        listMessage = new List<string>();
        listMessage.Add("Amenophis : Empereur, je suis marchand en quête de financement pour ma prochaine expéditions.");
        listMessage.Add("J'aurai besoin de vivre ou de marins pour pouvoir lancer un expédition commerciale.");
        listMessage.Add("Bien sûr, à mon retour, je partagerai avec vous les revenus de cette expédition.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Je vous offre le ravitaillement nécessaire à votre expédition !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Je vous offre les marins nécessaires à votre expédition !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Malheureusement je ne peux fiancer votre expédition pour le moment.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRequest.Add(request);



        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Decima : Bien le bonjour Empereur de Rome. Mon nom est Decima, et je viens vous parler d'une opportunité exceptionnelle !");
        listMessage.Add("e représente un groupe de barbare qui souhaite vendre leurs services aux plus offrants.");
        listMessage.Add("Contre une belle somme d'argent, ils accepteraient de rejoindre les rangs des soldats romains.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Intéressante proposition. Prévenez vos ''amis'', j'accepte leur offre.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Jamais l'armée romaine n'acceptera des mercenaires sans loyauté en son sein. ", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Lagarefix : Ave Empereur. J'ai eu vent d'une potion magique qui donnerai des pouvoirs surhumains à celui qui la boit.");
        listMessage.Add("Une tel potion nous permettrait d'assurer la sécurité de l'empire romain !");
        listMessage.Add("Malheureusement, j'ai besoins d'investissement pour poursuive mes recherches.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Bien sur, voici des financements qui devrait vous aider dans cette quête.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Cette potion magique ne peut être ! Abandonnez vos recherches !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Consilius : Empereur, un navire commercial vient d’accoster à Ostie avec des propositions intéressantes.");
        listMessage.Add("Il nous propose de lui acheter de la nourriture à des prix très intéressants !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Achetez-lui de la nourriture.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Achetez-lui des mets exotiques pour la population.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Nous n'avons rien à lui acheter.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Consilius : Empereur, un navire commercial vient d’accoster à Ostie avec des propositions intéressantes.");
        listMessage.Add(" Il souhaite acheter une partie de notre nourriture à des prix très intéressants !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Vendez-lui un quart de nos resserve !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Vendez-lui la moitié de nos resserve !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Dite lui que Rome à besoins de cette nourriture.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Minorinus : Empereur, je viens de trouver un filon d'or exceptionnel sous la ville de Rome.");
        listMessage.Add("Cela nécessiterait de l'investissement colossal pour l’exploiter, mais le jeu en vaut la bougie !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Bien, voici de l'or pour commencer l’exploitation.", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Je suis certain que la population sera ravie de participer à des travaux forcés dans une nouvelle mine !", delegate () { }));
        listeChoix.Add(new Dialog.Choix("Les barbares sont aux portes de l'empire et vous vous voulez creuser des trous ? Non !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRequest.Add(request);


        ///////////////////////////////////////////////////////////////////


>>>>>>> f7e1c92acf44e081fd8353e0cf3b328040227dfe
    }
}
