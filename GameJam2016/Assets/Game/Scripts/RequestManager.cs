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

        List<string> listMessage = new List<string>();
        listMessage.Add("Illius : Ave ! Je viens devant votre jugement car cet homme vient de voler mon cochon ! J’exige qu'il me le rende et qu'il soit punit !");
        listMessage.Add("Maximus : Mensonge monseigneur, ce cochon est le mien depuis des lunes !");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Coupez-moi ce cochon en deux et partagez les morceaux avec la communauté.", delegate () { Empire.instance.capitale.AddFood(8 / Empire.instance.valeurNouriture); }));
        listeChoix.Add(new Dialog.Choix("Puisque ce cochon vous pose problème, je vais vous le retirer.", delegate () { Empire.instance.capitale.AddGold(8); }));
        listeChoix.Add(new Dialog.Choix("Tant de vigueur pour un cochon, vous ferez de bon soldat !", delegate () { Empire.instance.capitale.AddArmy(2); }));
        Request request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Maxima : Bien le bonjour mon seigneur. Je viens humblement devant vous pour obtenir vos faveurs.");
        listMessage.Add("Mon mari vient d'être enrôlé dans l’armée, malheureusement, il était essentiel à la survit de notre pauvre famille.");
        listMessage.Add("Si vous pouvez simplement lui permettre de revenir auprès des siens, je vous en serai éternellement reconnaissante.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("J’entends votre requête et je vais faire le nécessaire pour qu'il retourne auprès de sa famille.", delegate () { Empire.instance.capitale.DecreaseArmy(1); Empire.instance.capitale.bonheur += 1; }));
        listeChoix.Add(new Dialog.Choix("Chaque soldat à un rôle crucial dans l'armée, mais j'ai espoir que cette compensation en or puisse couvrir vos besoins pendant son absence.", delegate () { Empire.instance.capitale.DecreaseGold(4); }));
        listeChoix.Add(new Dialog.Choix("Malheureusement, en ces temps difficiles, chaque famille porte la même histoire, je ne peux accéder à votre requête.", delegate () { Empire.instance.capitale.bonheur -= 2; }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);

        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Faustus : '' Seigneur, un énorme incendie est en train de ravager la cité !");
        listMessage.Add("Tous les citoyens sont au pied d’œuvre pour contenir l'incendie.");
        listMessage.Add("Je sais que les soldats, par tradition, n'ont pas le droit de franchir le Rubicon, mais nous demandons exceptionnellement l'intervention de l'armée nous aider.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sauvez les femmes et les enfants en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon.", delegate () { Empire.instance.capitale.DecreaseFood(12); }));
        listeChoix.Add(new Dialog.Choix("Sauvez les réserve de nourritures en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon.", delegate () { Empire.instance.capitale.bonheur -= 4; }));
        listeChoix.Add(new Dialog.Choix("Que les armées franchisent le Rubicon et aillent aider Rome !", delegate () { Empire.instance.capitale.DecreaseArmy(4); }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Proculus : ''Ave ! Je viens d'apercevoir douze vautours dans le ciel ! Les augures sont avec nous !");
        listMessage.Add("Tous comme Romulus à la fondation de Rome!");
        listMessage.Add("D'après vous, quelles bonnes nouvelles nous annonce ces augures ?");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("D'excellente récolte évidement !", delegate () { Empire.instance.capitale.productionNourriture += 1; }));
        listeChoix.Add(new Dialog.Choix("Cela annonce des réussites militaires !", delegate () { Empire.instance.capitale.AddArmy(5); }));
        listeChoix.Add(new Dialog.Choix("Cela annonce un retour prochain à la Pax Romana !", delegate () { Empire.instance.capitale.bonheur += 5; }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Secundus : Seigneur, une maladie étrange décimes nos troupeaux !");
        listMessage.Add(" Nous avons trouvé un sage qui prétend pouvoir stopper la maladie, mais il demande une compensation financière !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Abattez les troupeaux pour contenir la maladie !", delegate () { Empire.instance.capitale.DecreaseFood(6); }));
        listeChoix.Add(new Dialog.Choix("Payer ce sage, et faite lui comprendre qu'il a tous intérêts à stopper la maladie s’il veut conserver sa tête !", delegate () { Empire.instance.capitale.DecreaseGold(12); }));
        listeChoix.Add(new Dialog.Choix("Faite pendre cet escroc et réquisitionnez ses affaires personnelles !", delegate () { Empire.instance.capitale.bonheur -= 4; Empire.instance.capitale.DecreaseFood(4); Empire.instance.capitale.AddGold(4); }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Statius :  Seigneur, nous avons intercepté une cargaison de contrebande de nourriture. ");
        listMessage.Add("Que devons nous faire de la cargaison ?");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Faite venir la cargaison dans les greniers de la ville.", delegate () { Empire.instance.capitale.AddFood(6); }));
        listeChoix.Add(new Dialog.Choix("Organiser un banquet pour tous avec la cargaison.", delegate () { Empire.instance.capitale.bonheur += 6; }));
        listeChoix.Add(new Dialog.Choix("Vendez la cargaison et faite moi parvenir les recettes.", delegate () { Empire.instance.capitale.AddGold(12); }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Octavius : Jupiter est en colère monseigneur ! Ses foudres scient le ciel en ce moment même !");
        listMessage.Add("Que devons nous faire pour apaiser le dieu des dieux ?");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Sacrifiez du bétails !", delegate () { Empire.instance.capitale.DecreaseFood(4); }));
        listeChoix.Add(new Dialog.Choix("Organisez une fête populaire où le bétail sera sacrifié et la population rassasié.", delegate () { Empire.instance.capitale.DecreaseFood(8); Empire.instance.capitale.bonheur += 4; }));
        listeChoix.Add(new Dialog.Choix("Jupiter n'est qu'une représentation du seul et unique Dieu ! Je ne craint pas ses foudres!", delegate () { Empire.instance.capitale.bonheur -= 4; }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        listMessage = new List<string>();
        listMessage.Add("Hostus : Empereur, le sénat est inquiet de votre politique actuel et demande des précisions sur les futures évolutions de l'empire.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Dites-leur que l'empire va se concentrer sur la production de nourriture dans les prochains jours.", delegate () { Empire.instance.capitale.AddFood(4); }));
        listeChoix.Add(new Dialog.Choix("Dites-leur que l'armée et la sécurité de toutes les villes romaines sont ma principale préoccupation.", delegate () { Empire.instance.capitale.AddArmy(2); }));
        listeChoix.Add(new Dialog.Choix("Dites-leur que le bonheur de chaque citoyen de capital est la priorité de l’Empereur.", delegate () { Empire.instance.capitale.bonheur += 4; }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Valentinus : Les relations entre païen et chrétien sont de plus en plus tendus monseigneur !");
        listMessage.Add(" Mais prendre une position ouverte ne fera qu'attiser les tensions.");
        listMessage.Add("Je vous propose d'autres options moins conventionnelle mais sans doutes plus adaptés.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Discréditez les représentant des chrétiens avec des rumeurs de péchés capitaux.", delegate () { Empire.instance.capitale.DecreaseGold(8); }));
        listeChoix.Add(new Dialog.Choix("Créez de faux mauvais augures pour apeurer les Païens.", delegate () { Empire.instance.capitale.bonheur -= 4; }));
        listeChoix.Add(new Dialog.Choix("Créez l'étincelle qui enflammera le débat, et envoyez l'armée pour nettoyer les rue de ces croyants imbéciles !", delegate () { Empire.instance.capitale.DecreaseArmy(2); }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        //TODO delegate
        Request expeEvent = null;
        listMessage = new List<string>();
        listMessage.Add("Amenophis : Empereur, je suis marchand en quête de financement pour ma prochaine expéditions.");
        listMessage.Add("J'aurai besoin de vivre ou de marins pour pouvoir lancer un expédition commerciale.");
        listMessage.Add("Bien sûr, à mon retour, je partagerai avec vous les revenus de cette expédition.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Je vous offre le ravitaillement nécessaire à votre expédition !", delegate () { Empire.instance.capitale.DecreaseFood(8); listRandomRequest.Remove(expeEvent);listRandomRequest.Add(ExpeditionEvent2()); }));
        listeChoix.Add(new Dialog.Choix("Je vous offre les marins nécessaires à votre expédition !", delegate () { Empire.instance.capitale.DecreaseArmy(4); listRandomRequest.Remove(expeEvent); listRandomRequest.Add(ExpeditionEvent2()); }));
        listeChoix.Add(new Dialog.Choix("Malheureusement je ne peux fiancer votre expédition pour le moment.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        expeEvent = request;
        listRandomRequest.Add(request);



        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Decima : Bien le bonjour Empereur de Rome. Mon nom est Decima, et je viens vous parler d'une opportunité exceptionnelle !");
        listMessage.Add("je représente un groupe de barbare qui souhaite vendre leurs services aux plus offrants.");
        listMessage.Add("Contre une belle somme d'argent, ils accepteraient de rejoindre les rangs des soldats romains.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Intéressante proposition. Prévenez vos ''amis'', j'accepte leur offre.", delegate () { Empire.instance.capitale.DecreaseGold(20); Empire.instance.capitale.AddArmy(5); }));
        listeChoix.Add(new Dialog.Choix("Jamais l'armée romaine n'acceptera des mercenaires sans loyauté en son sein. ", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////
        Request potionEvent1 = null;
        listMessage = new List<string>();
        listMessage.Add("Lagarefix : Ave Empereur. J'ai eu vent d'une potion magique qui donnerai des pouvoirs surhumains à celui qui la boit.");
        listMessage.Add("Une tel potion nous permettrait d'assurer la sécurité de l'empire romain !");
        listMessage.Add("Malheureusement, j'ai besoins d'investissement pour poursuive mes recherches.");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Bien sur, voici des financements qui devrait vous aider dans cette quête.", delegate () { Empire.instance.capitale.DecreaseGold(20);listRandomRequest.Remove(potionEvent1); listRandomRequest.Add(PotionEvent2()); }));
        listeChoix.Add(new Dialog.Choix("Cette potion magique ne peut être ! Abandonnez vos recherches !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        potionEvent1 = request;
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Consilius : Empereur, un navire commercial vient d’accoster à Ostie avec des propositions intéressantes.");
        listMessage.Add("Il nous propose de lui acheter de la nourriture à des prix très intéressants !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Achetez-lui de la nourriture.", delegate () { Empire.instance.capitale.AddFood(5); Empire.instance.capitale.DecreaseGold(5); }));
        listeChoix.Add(new Dialog.Choix("Achetez-lui des mets exotiques pour la population.", delegate () { Empire.instance.capitale.bonheur += 5; Empire.instance.capitale.DecreaseGold(5); }));
        listeChoix.Add(new Dialog.Choix("Nous n'avons rien à lui acheter.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Consilius : Empereur, un navire commercial vient d’accoster à Ostie avec des propositions intéressantes.");
        listMessage.Add(" Il souhaite acheter une partie de notre nourriture à des prix très intéressants !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Vendez-lui un quart de nos resserve !", delegate () { int n = (int)(Empire.instance.capitale.nourriture * 0.25); Empire.instance.capitale.DecreaseFood(n); Empire.instance.capitale.AddGold(n * 4); }));
        listeChoix.Add(new Dialog.Choix("Vendez-lui la moitié de nos resserve !", delegate () {int n =(int)( Empire.instance.capitale.nourriture * 0.5); Empire.instance.capitale.DecreaseFood(n); Empire.instance.capitale.AddGold(n * 4); }));
        listeChoix.Add(new Dialog.Choix("Dite lui que Rome à besoins de cette nourriture.", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

        listMessage = new List<string>();
        listMessage.Add("Minorinus : Empereur, je viens de trouver un filon d'or exceptionnel sous la ville de Rome.");
        listMessage.Add("Cela nécessiterait de l'investissement colossal pour l’exploiter, mais le jeu en vaut la bougie !");
        listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Bien, voici de l'or pour commencer l’exploitation.", delegate () { Empire.instance.capitale.DecreaseGold(40); Empire.instance.capitale.productionOr += 4; }));
        listeChoix.Add(new Dialog.Choix("Je suis certain que la population sera ravie de participer à des travaux forcés dans une nouvelle mine !", delegate () { Empire.instance.capitale.bonheur -= 5; }));
        listeChoix.Add(new Dialog.Choix("Les barbares sont aux portes de l'empire et vous vous voulez creuser des trous ? Non !", delegate () { }));
        request = new Request(listMessage, listeChoix);
        listRandomRequest.Add(request);


        ///////////////////////////////////////////////////////////////////

    }

    private Request ExpeditionEvent2()
    {

        List<string> listMessage = new List<string>();
        listMessage.Add("Amenophis : Empereur, je viens de revenir de mon expédition et elle fut une réussite.");
        listMessage.Add("Je vous apporte donc votre pars !");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Des mets exotiques venant des quatre coins du globe ?", delegate () { Empire.instance.capitale.AddFood(8); }));
        listeChoix.Add(new Dialog.Choix("De l'or en grandes quantité ?", delegate () { Empire.instance.capitale.AddGold(16); }));
        listeChoix.Add(new Dialog.Choix("Des vêtements de soies pour le peuple ?", delegate () { Empire.instance.capitale.bonheur += 8; }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }

    private Request PotionEvent2()
    {
        Request potionEvent2 = null;
        List<string> listMessage = new List<string>();
        listMessage.Add("Lagarefix : Empereur, je viens de revenir d'un petit village gaulois qui résiste encore et toujours à l'envahisseur grâce à la potion magique.");
        listMessage.Add("ls n'ont pas voulu me partager le secret la potion, mais ils mon offert des graines de blé de qualité exceptionnel !");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Parfait ! Faite pousser ce blé !", delegate () { Empire.instance.capitale.capitaleNourriture += 1; }));
        listeChoix.Add(new Dialog.Choix("C'est intolérable, préparer une armée pour détruire ce village ! Et n'oubliez pas de faire pousser ce blé !"
            , delegate () {
                 Empire.instance.capitale.DecreaseArmy(6); Empire.instance.capitale.capitaleNourriture += 1;
                 listRandomRequest.Remove(potionEvent2); listRandomRequest.Add(PotionEvent3());
            }));
        Request request = new Request(listMessage, listeChoix);
        potionEvent2 = request;
        return request;
    }

    private Request PotionEvent3()
    {

        List<string> listMessage = new List<string>();
        listMessage.Add("Leopardus : Empereur, l'armée que nous avions envoyée pour détruire ce village à été détruite par deux gaulois.");
        listMessage.Add(" Mais bonne nouvelle ! Je suis encore entier !");
        List<Dialog.Choix> listeChoix = new List<Dialog.Choix>();
        listeChoix.Add(new Dialog.Choix("Retournez dans les rangs de l'armée !", delegate () { Empire.instance.capitale.AddArmy(1); }));
        listeChoix.Add(new Dialog.Choix("Faite moi disparaitre cet incompétent !", delegate () { Empire.instance.capitale.AddGold(4); }));
        Request request = new Request(listMessage, listeChoix);
        return request;
    }
}
