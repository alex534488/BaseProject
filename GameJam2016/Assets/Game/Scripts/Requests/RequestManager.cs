using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RequestManager : MonoBehaviour
{
    public RequestBank bank;
    static RequestManager requestManager;

    List<Request> listRequest = new List<Request>();
    List<Request> listRandomRequest = new List<Request>();

    public UnityEvent onAllRequestsComplete = new UnityEvent();

    void Awake()
    {
        if (requestManager == null) requestManager = this;
        //GenerateRandomRequests();
    }

    public void NewDay()
    {
        //GetRandomRequest(1);

        if (listRequest.Count > 0)
            listRequest[0].DoRequest();
        else
            onAllRequestsComplete.Invoke();
    }

    public static void SendRequest(Request request)
    {
        requestManager.listRequest.Add(request);
    }

    //void GetRandomRequest(int amount)
    //{
    //    listRequest.Add(listRandomRequest[0]);
    //    listRandomRequest.Remove(listRandomRequest[0]);
    //    //if (listRandomRequest.Count <= 0)
    //        //GenerateRandomRequests();
    //}

    public static void DoNextRequest()
    {
        requestManager.listRequest.Remove(requestManager.listRequest[0]);
        if (requestManager.listRequest.Count <= 0)
        {
            requestManager.onAllRequestsComplete.Invoke();
            return;
        }
        requestManager.listRequest[0].DoRequest();
    }

    public static void DeleteRequest(Request request)
    {
        requestManager.listRequest.Remove(request);
    }

    public static void DeleteAllRequests()
    {
        requestManager.listRequest.Clear();
    }

    /*
void GenerateRandomRequests()
{
    // LISTE DES REQUETES ALEATOIRES
    Dialog.Message message = new Dialog.Message("Illius : Ave ! Je viens vous avertir que deux hommes se disputent concernant le titre de propriété d'un cochon." + "\n\n" + "Le premier exige que le cochon lui soit rendu et que le voleur soit punit. Le second soutient que le cochon lui appartient depuis des lunes. " + "\n\n" + "Que devrions-nous faire?");
    List<Choice> listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Coupez-moi ce cochon en deux et partagez les morceaux avec la communauté. (+4 Nourriture)", delegate () { Empire.instance.capitale.AddFood(4); }));
    listeChoix.Add(new Choice("Puisque ce cochon vous pose problème, je vais vous le retirer. (+8 Or)", delegate () { Empire.instance.capitale.AddGold(8); }));
    listeChoix.Add(new Choice("Tant de vigueur pour un cochon, vous ferez de bon soldat ! (+2 Soldat)", delegate () { Empire.instance.capitale.AddArmy(2); }));
    Request request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 1

    message = new Dialog.Message("Maxima : Bien le bonjour mon seigneur. Je viens humblement devant vous pour obtenir vos faveurs." + "\n \n" + "Mon mari vient d'être enrôlé dans l’armée, malheureusement, il était essentiel à la survie de notre pauvre famille." + "\n \n" + "Si vous pouviez simplement lui permettre de revenir auprès des siens, je vous en serais éternellement reconnaissante.");

    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("J’entends votre requête et je vais faire le nécessaire pour qu'il retourne auprès de sa famille. (-1 Soldat +1 Bonheur)", delegate () { Empire.instance.capitale.AddArmy(-1); Empire.instance.capitale.AddHappiness(1); }));
    listeChoix.Add(new Choice("Chaque soldat à un rôle crucial dans l'armée, mais j'ai espoir que cette compensation en or puisse couvrir vos besoins pendant son absence. (-8 Or)", delegate () { Empire.instance.capitale.AddGold(-8); }));
    listeChoix.Add(new Choice("Malheureusement, en ces temps difficiles, chaque famille porte la même histoire, je ne peux accéder à votre requête. (-2 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-2); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 2

    message = new Dialog.Message("Faustus : Seigneur, un énorme incendie est en train de ravager la cité !" + "\n \n" + "Tous les citoyens sont au pied d’œuvre pour contenir l'incendie." + "\n \n" + "Je sais que les soldats, par tradition, n'ont pas le droit de franchir le Rubicon, mais nous demandons exceptionnellement l'intervention de l'armée afin de nous aider.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Sauvez les femmes et les enfants en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon. (-16 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-16); }));
    listeChoix.Add(new Choice("Sauvez les réserve de nourritures en priorité, mais malheureusement, les soldats ne peuvent franchir le Rubicon. (-8 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-8); }));
    listeChoix.Add(new Choice("Que les armées franchisent le Rubicon et aillent aider Rome ! (-6 soldats)", delegate () { Empire.instance.capitale.AddArmy(-6); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 3
    message = new Dialog.Message("Proculus : Ave ! Je viens d'apercevoir douze vautours dans le ciel, j'en déduis que les augures sont de notre coté !" + "\n \n" + "Cela me rappelle Romulus à la fondation de Rome!" + "\n \n" + "D'après vous, quelle bonne nouvelle nous annonce ces augures ?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("D'excellentes récoltes évidemment ! (+1 Production Nourriture)", delegate () { Empire.instance.capitale.AddFoodProd(1); }));
    listeChoix.Add(new Choice("Cela annonce des réussites militaires ! (+5 Soldat)", delegate () { Empire.instance.capitale.AddArmy(5); }));
    listeChoix.Add(new Choice("Cela annonce un retour prochain à la Pax Romana ! (+3 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(3); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 4
    message = new Dialog.Message("Secundus : Seigneur, une maladie étrange décime nos troupeaux !" + "\n \n" + "Nous avons trouvé un sage qui prétend pouvoir arrêter la propagation de la maladie" + "\n \n" + "Mais ce dernier demande une compensation financière !");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Abattez les troupeaux pour contenir la maladie ! "+" \n" + "(-6 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-6); }));
    listeChoix.Add(new Choice("Payer ce sage, et faites lui comprendre qu'il est dans son intérêt de réussir la maladie s’il veut conserver sa tête ! (-12 Or)", delegate () { Empire.instance.capitale.AddGold(-12); }));
    listeChoix.Add(new Choice("Faite pendre cet escroc et réquisitionnez ses affaires personnelles ! (-2 Bonheur, -4 Nourriture, + 10 Or)", delegate () { Empire.instance.capitale.AddHappiness(-2); Empire.instance.capitale.AddFood(-4); Empire.instance.capitale.AddGold(10); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 5
    message = new Dialog.Message("Statius : Seigneur, nous avons intercepté une cargaison de contrebande de nourriture. " + "\n \n" + "Les voyous se sont malheureusement échappé." + "\n \n" + "Que devrions-nous faire de la cargaison ?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Faite venir la cargaison dans les greniers de la ville." + "\n" + " (+3 Nourriture)", delegate () { Empire.instance.capitale.AddFood(3); }));
    listeChoix.Add(new Choice("Organiser un banquet pour tous avec la cargaison. " + "\n" + " (+1 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(1); }));
    listeChoix.Add(new Choice("Vendez la cargaison et faite moi parvenir les recettes." + "\n" + " (+6 Or)", delegate () { Empire.instance.capitale.AddGold(6); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 6
    message = new Dialog.Message("Octavius : Jupiter est en colère monseigneur ! "+ "\n \n" + "Ses foudres déchirent le ciel en ce moment même !" + "\n \n" + "Que devons nous faire pour apaiser le dieu des dieux ?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Sacrifiez du bétails ! (-8 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-8); }));
    listeChoix.Add(new Choice("Organisez une fête populaire où le bétail sera sacrifié et la population rassasiée (-16 Nourriture, +4 Bonheur)", delegate () { Empire.instance.capitale.AddFood(-16); Empire.instance.capitale.AddHappiness(2); }));
    listeChoix.Add(new Choice("Jupiter n'est qu'une représentation du seul et unique Dieu ! Je ne crains pas ses foudres! (-3 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-3); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 7
    message = new Dialog.Message("Hostus : Empereur, le sénat est inquiet de votre politique actuel et demande des précisions sur les futures évolutions de l'empire.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Dites-leur que l'empire va se concentrer sur la production de nourriture dans les prochains jours. \n(+6 Nourriture)", delegate () { Empire.instance.capitale.AddFood(6); }));
    listeChoix.Add(new Choice("Dites-leur que l'armée et la sécurité de toutes les villes romaines sont ma principale préoccupation. (+3 Soldat)", delegate () { Empire.instance.capitale.AddArmy(3); }));
    listeChoix.Add(new Choice("Dites-leur que le bonheur de chaque citoyen de capital est la priorité de l’Empereur. (+2 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(2); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 8

    message = new Dialog.Message("Valentinus : Les relations entre païen et chrétien sont de plus en plus tendus monseigneur ! \n \nMais prendre une position ouverte ne fera qu'attiser les tensions.\n \nJe vous propose d'autres options moins conventionnelle mais sans doutes plus adaptés.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Discréditez les représentant des chrétiens avec des rumeurs de péchés capitaux. (-8 Or)", delegate () { Empire.instance.capitale.AddGold(-8); }));
    listeChoix.Add(new Choice("Créez de faux mauvais augures pour apeurer les Païens. (-2 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-2); }));
    listeChoix.Add(new Choice("Créez l'étincelle qui enflammera le débat, et envoyez l'armée pour nettoyer les rue de ces croyants imbéciles ! (-4 Soldat)", delegate () { Empire.instance.capitale.AddArmy(-4); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 9

    Request expeEvent = null;
    message = new Dialog.Message("Amenophis : Empereur, je suis marchand en quête de financement pour ma prochaine expédition.\n \nJ'aurais besoin de vivres ou de marins pour pouvoir lancer un expédition commerciale.\n \nBien sûr, à mon retour, je partagerais avec vous les revenus de cette expédition.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Je vous offre le ravitaillement nécessaire à votre expédition ! (-8 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-8); listRandomRequest.Remove(expeEvent);listRandomRequest.Add(ExpeditionEvent2()); }));
    listeChoix.Add(new Choice("Je vous offre les marins nécessaires à votre expédition ! (-4 Soldat)", delegate () { Empire.instance.capitale.AddArmy(-4); listRandomRequest.Remove(expeEvent); listRandomRequest.Add(ExpeditionEvent2()); }));
    listeChoix.Add(new Choice("Malheureusement je ne peux financer votre expédition pour le moment.", delegate () { }));
    request = new Request(message, listeChoix);
    expeEvent = request;
    listRandomRequest.Add(request);



    /////////////////////////////////////////////////////////////////// 10

    message = new Dialog.Message("Decima : Bien le bonjour Empereur de Rome. \n\nJe viens vous parler d'une opportunité exceptionnelle ! \n\nJe représente un groupe de barbares qui souhaite vendre leurs services aux plus offrants. \n\nContre une belle somme d'argent, ils accepteraient de rejoindre les rangs des soldats romains.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Intéressante proposition. Prévenez vos amis, j'accepte leur offre. (-20 Or +7 Soldats)", delegate () { Empire.instance.capitale.AddGold(-20); Empire.instance.capitale.AddArmy(7); }));
    listeChoix.Add(new Choice("Jamais l'armée romaine n'acceptera des mercenaires sans loyauté dans ses rangs. ", delegate () { }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 11
    Request potionEvent1 = null;
    message = new Dialog.Message("Lagarefix : Ave Empereur. J'ai eu vent d'une potion magique qui donnerais des pouvoirs surhumains à celui qui la boit. \n\nUne telle potion nous permettrait d'assurer la sécurité de l'Empire romain ! \n\nMalheureusement, j'ai besoin d'investissement pour poursuivre mes recherches.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Bien sur, voici une somme raisonnable qui devrait vous aider dans cette quête. (-20 Or)", delegate () { Empire.instance.capitale.AddGold(-20);listRandomRequest.Remove(potionEvent1); listRandomRequest.Add(PotionEvent2()); }));
    listeChoix.Add(new Choice("Cette potion magique n'est que de vilaines sottises ! Abandonnez vos recherches !", delegate () { }));
    request = new Request(message, listeChoix);
    potionEvent1 = request;
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 12

    message = new Dialog.Message("Consilius : Empereur, un navire commercial vient d’accoster à Ostie avec des propositions intéressantes.\n\nIl nous propose de lui acheter de la nourriture à des prix très intéressants !");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Achetez-lui de la nourriture. (+5 Nourriture, -5 Or)", delegate () { Empire.instance.capitale.AddFood(5); Empire.instance.capitale.AddGold(-5); }));
    listeChoix.Add(new Choice("Achetez-lui des mets exotiques pour la population.\n(+5 Bonheur, -25 Or)", delegate () { Empire.instance.capitale.AddHappiness(5); Empire.instance.capitale.AddGold(-25); }));
    listeChoix.Add(new Choice("Nous n'avons rien à lui acheter.", delegate () { }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 13

    message = new Dialog.Message("Consilius : Empereur, un navire commercial vient d’accoster à Ostie avec des propositions intéressantes. \n\nIl souhaite acheter une partie de notre nourriture à des prix très intéressants !");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Vendez-lui le quart de nos réserves ! (25% de vente à 4 Or par Nourriture vendu)", delegate () { int n = (int)(Empire.instance.capitale.GetFood() * 0.25); Empire.instance.capitale.AddFood(-n); Empire.instance.capitale.AddGold(n * 4); }));
    listeChoix.Add(new Choice("Vendez-lui la moitié de nos réserves ! (50% de vente à 4 Or par Nourriture vendu)", delegate () {int n =(int)( Empire.instance.capitale.GetFood() * 0.5); Empire.instance.capitale.AddFood(-n); Empire.instance.capitale.AddGold(n * 4); }));
    listeChoix.Add(new Choice("Dites lui que Rome à besoin de cette nourriture.", delegate () { }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 14

    message = new Dialog.Message("Minorinus : Empereur, je viens de trouver un filon d'or exceptionnel sous la ville de Rome. \n\nCela nécessiterait un investissement colossal pour l’exploiter, mais le jeu en vaut la chandelle !");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Bien, voici de l'or pour commencer l’exploitation. (+4 Production Or, -40 Or)", delegate () { Empire.instance.capitale.AddGold(-40); Empire.instance.capitale.AddGoldProd(4); }));
    listeChoix.Add(new Choice("Je suis certain que la population sera ravie de participer à des travaux forcés dans cette nouvelle mine ! (+4 Production Or, -5 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-5); Empire.instance.capitale.AddGoldProd(4); }));
    listeChoix.Add(new Choice("Les barbares sont aux portes de l'empire et vous voulez creuser des trous ? Non !", delegate () { }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 15

    message = new Dialog.Message("Maxima : Mon empereur, j’implore votre soutien en ce jour malheureux. \n\nJe viens de recevoir un papyrus annonçant le décès de mon mari, tombé au combat.\n\nAfin d’honorer son service, êtes-vous en mesure de sauver ma famille en ces temps difficiles? ");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("L’état des récoltes ne me permet pas de vous offrir de la nourriture, mais voici une compensation financière \n(-5 Or, +1 Bonheur)", delegate () { Empire.instance.capitale.AddGold(-5); Empire.instance.capitale.AddHappiness(1); }));
    listeChoix.Add(new Choice("J’entends votre requête et vous offre de la nourriture en échange des loyaux services de votre mari tombé au combat. (-3 Nourriture, +1 Bonheur)", delegate () { Empire.instance.capitale.AddFood(-3); Empire.instance.capitale.AddHappiness(1); }));
    listeChoix.Add(new Choice("Sa mémoire sera honorée mais aucune compensation ne vous sera accordée.", delegate () { }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 16

    message = new Dialog.Message("Mamerca : Seigneur, mes fils sont tombés gravement malade et je n’ai personne pour amasser mes récoltes. \n\nJe possède de bonnes réserves, suffisante à ma consommation personnelle, mais il serait dommage de perdre les récoltes.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Mes Citoyens vont les récolter et vous serez compensé financièrement. (+4 Nourriture, -2 Or)", delegate () { Empire.instance.capitale.AddFood(4); Empire.instance.capitale.AddGold(-2); }));
    listeChoix.Add(new Choice("Mes Citoyens iront collecter vos récoltes et elles seront ajoutées à vos réserves. (+1 Bonheur, -4 Or)", delegate () { Empire.instance.capitale.AddHappiness(1); Empire.instance.capitale.AddGold(-4); }));
    listeChoix.Add(new Choice("Mes Citoyens vont les récolter pour l'empire. \n(+8 Nourriture, -1 Bonheur)", delegate () { Empire.instance.capitale.AddFood(8); Empire.instance.capitale.AddHappiness(-1); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 17

    message = new Dialog.Message("Procula : Mon empereur, le climat familial dans notre résidence n’est plus ce qu’il était. \n\nDepuis que mon mari est revenu de la guerre, il est violent avec les enfants.\n\nPouvez-vous m’autoriser à quitter le domicile familial pour notre protection?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Votre mari cherche à préparer vos enfants à la dure réalité de la guerre. (-3 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-3); }));
    listeChoix.Add(new Choice("L’Empire hébergera vos enfants pour leur sécurité, mais votre place est aux cotés de votre mari. \n(-10 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-10); }));
    listeChoix.Add(new Choice("L’Empire hébergera votre famille pendant 1 semaine, le temps que votre mari reprenne ses esprits \n(-15 Nourriture, +3 Bonheur)", delegate () { Empire.instance.capitale.AddFood(-15); Empire.instance.capitale.AddHappiness(3); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 18

    message = new Dialog.Message("Numeria : Seigneur, mon garçon s’est fait jeter dans le Fleuve, hier au soir, par des jeunes voyous.\n\nPouvez-vous m’aider à trouver les malfaiteurs?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Mes soldats partiront à la recherche de ses voyous et ils paieront pour leurs crimes. (-2 Soldat, +1 Bonheur)", delegate () { Empire.instance.capitale.AddArmy(-2); Empire.instance.capitale.AddHappiness(1); }));
    listeChoix.Add(new Choice("Votre fils devrait apprendre à se défendre. À partir d’aujourd’hui, il commencera sa formation militaire. (-1 Bonheur, +1 Soldat)", delegate () { Empire.instance.capitale.AddHappiness(-1); Empire.instance.capitale.AddArmy(1); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 19

    message = new Dialog.Message("Quinta : Mon seigneur, j’implore votre générosité suite à un grave incendie qui a ravagé ma résidence. \n\nMon mari, mes 4 enfants et moi-même ne sommes plus en mesure de subvenir à nos besoins en ces temps difficile.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Je suis en mesure de vous verser quelques pièces d’or à vous et votre famille. (-5 Or)", delegate () { Empire.instance.capitale.AddGold(-5); }));
    listeChoix.Add(new Choice("Je vous offre une partie des réserves alimentaires de l’Empire ma chère dame.(-3 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-3); }));
    listeChoix.Add(new Choice("Les dieux nous soumettent parfois à des tests difficiles, mais votre famille doit surmonter cette épreuve seul ma chère dame. (-1 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-1); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);


    /////////////////////////////////////////////////////////////////// 20

    message = new Dialog.Message("Opiter : Ô grand maître, la cité aurait bien besoin d’offrir à ses citoyens davantage de sources de divertissement.\n\nLa construction d’une arène de gladiateur pourrait remonter le moral de nos troupes !");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Investir et construire l’arène (-12 Or, +3 Bonheur)", delegate () { Empire.instance.capitale.AddGold(-12); Empire.instance.capitale.AddHappiness(3);}));
    listeChoix.Add(new Choice("La cité ne peut pas se permettre de telles dépenses en ces temps difficiles mon cher Opiter", delegate () {  }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 21

    message = new Dialog.Message("Mamercus: Empereur, puis-je vous proposer de construire un refuge qui accueillera les familles en difficulté de notre belle capitale?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Il s’agit d’une excellente proposition, voici les fonds nécessaires pour la construction de ce refuge. \n(-5 Nourriture, -10 Or, +3 Bonheur)", delegate () { Empire.instance.capitale.AddFood(-5); Empire.instance.capitale.AddGold(-10); Empire.instance.capitale.AddHappiness(3); }));
    listeChoix.Add(new Choice("Tu peux bien évidemment le proposer, mais ma réponse sera négative mon cher ami.", delegate () { }));

    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 22

    message = new Dialog.Message("Secundus : Mon seigneur, les soldats ne sont pas satisfaits de la qualité et la quantité de la nourriture qu’ils reçoivent actuellement. \n\nPar le fait même, les jeunes ne désirent plus s’enrôler dans l’armée.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("J’accepte d’entendre leur demande et de leur offrir quelques rations supplémentaire (-20 Nourriture)", delegate () { Empire.instance.capitale.AddFood(-20); }));
    listeChoix.Add(new Choice("Les jeunes doivent faire leurs services militaires, de plein grès ou non. (-5 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(-5); }));
    listeChoix.Add(new Choice("Je considère que cette requête est déraisonnable mon cher (-6 Soldats)", delegate () { Empire.instance.capitale.AddArmy(-8); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 23

    message = new Dialog.Message("Faustus : Mon empereur, les récoltes ne vont pas aussi bien qu’à l’habitude. \n\nPlusieurs animaux ont été victime de la maladie, il faudrait en acheter de nouveaux afin d’optimiser la récolte.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Nous allons investir massivement dans l'agriculture pour corriger le problème et optimiser les récoltes. \n(-40 Or, +4 Production Nourriture)", delegate () { Empire.instance.capitale.AddGold(-40); Empire.instance.capitale.AddFoodProd(4); }));
    listeChoix.Add(new Choice("Je vais immédiatement corriger ce problème en ordonnant l’achat  de nouveaux bœufs. (-10 Or)", delegate () { Empire.instance.capitale.AddGold(-10);  }));
    listeChoix.Add(new Choice("Les coffres de l’Empire ne me permettent pas une telle dépense en ce moment. (-2 Production Nourriture)", delegate () { Empire.instance.capitale.AddFoodProd(-1); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 24

    message = new Dialog.Message("Valentinus : Mon seigneur, j’aimerais vous demander la permission de construire une banque qui permettrait d’augmenter la prospérité économique de la cité. \n\nJ’aurais besoin de 20 pièces d’or.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Toutes les idées qui permettront d’augmenter la richesse de notre Empire seront les bienvenues.(-20 Or, +2 Production Or)", delegate () { Empire.instance.capitale.AddGold(-20); Empire.instance.capitale.AddGoldProd(2);  }));
    listeChoix.Add(new Choice("Malgré votre bonne initiative, il m’est impossible d’accepter la présente demande.", delegate () { }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 25


    message = new Dialog.Message("Vibius : Empereur, compte-tenu de la situation économique actuelle, \n\nDevrions-nous encourager les gens à se convertir en fermiers ou en marchands?");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Incitons les fermiers à devenir marchands. \n(+2 Production Or,-1 Production Nourriture)", delegate () { Empire.instance.capitale.AddGoldProd(2); Empire.instance.capitale.AddFoodProd(-1); }));
    listeChoix.Add(new Choice("Incitons les marchands à devenir fermiers. \n(+1 Production Nourriture -2 Production Or)", delegate () { Empire.instance.capitale.AddGoldProd(-2); Empire.instance.capitale.AddFoodProd(1); }));
    request = new Request(message, listeChoix);
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 26

    Request AakifN = null;
    Request AbdulN = null;
    Request ChakaN = null;

    Request Aakif = null;
    Request Abdul = null;
    Request Chaka = null;

    Request trad = null;

    message = new Dialog.Message("Vopiscus : Bien le bonjour, je suis un grand voyageur \n\nJ’ai parcouru une grande partie de notre monde et visité des centaines de cités. \n\nJe suis prêt à vous offrir mes services de traduction au coût raisonnable de 2 pièces par jour.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Un service de traduction? Êtes-vous tombé sur la tête mon jeune ami, tout le monde parle la même langue ici, dans l’Empire.", delegate () { }));
    listeChoix.Add(new Choice("Vos services seront de grande utilité au sein de l’Empire, ainsi j’accepte votre requête. (-2 Production Or)"
        , delegate () {
            Empire.instance.capitale.AddGoldProd(-2);
            listRandomRequest.Remove(AakifN);
            listRandomRequest.Remove(AbdulN);
            listRandomRequest.Remove(ChakaN);
            listRandomRequest.Remove(trad);
            listRandomRequest.Add(Aakif);
            listRandomRequest.Add(Abdul);
            listRandomRequest.Add(Chaka);
        }));   //Da fuck ??
    request = new Request(message, listeChoix);
    trad = request;
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 27


    message = new Dialog.Message("Abdul (Voyageur étranger) : Nou bezwen manje. Noue vou ede pli tor.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Retournez dans votre contrée et apprenez notre langue avant de venir nous importuner encore.", delegate () { }));
    request = new Request(message, listeChoix);
    AbdulN = request;
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 28


    message = new Dialog.Message("Aakif (Voyageur étranger) : Keiser, ons, Suid-voisons is in ekonomiese probleme en vra vir jou finansiële ondersteuning.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Votre dialecte de civilisation sous-développé et inutile ne m’intéresse guère mon cher. \nApprenez à communiquer dans la langue de l’Empire avant de revenir me voir.", delegate () { }));
    request = new Request(message, listeChoix);
    AakifN = request;
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 29


    message = new Dialog.Message("Chaka(Voyageur étranger) : Singabantu Izıhlabane omuhle abazokwazi ukubungaza izakhamuzi esenana X igolide.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Comment voulez-vous que je vous aide si je ne vous comprends point?", delegate () { }));
    request = new Request(message, listeChoix);
    ChakaN = request;
    listRandomRequest.Add(request);

    /////////////////////////////////////////////////////////////////// 


    message = new Dialog.Message("Aakif (Voyageur Étranger - Traduit): Empereur, nous, voisons du Sud sommes en difficulté économique et demandons votre soutien financier.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Offrir 120 pièces d’or au chef de la tribu afin de développer une alliance économique. \n(+2 Production Or pour TOUS les villages)", delegate () 
    {
        Empire.instance.capitale.AddGold(-120);
         foreach (Village vil in Empire.instance.listVillage)
        {
            vil.AddGoldProd(2);
        }
        listRandomRequest.Remove(Aakif);
    }));
    listeChoix.Add(new Choice("Malheureusement, je ne peux point vous aider actuellement.", delegate () { }));
    request = new Request(message, listeChoix);
    Aakif = request;

    ///////////////////////////////////////////////////////////////////


    message = new Dialog.Message("Abdul (Voyageur étranger) : Les récoltes de ma tribu sont médiocres cette saison. \n\nNous avons grandement besoin de nourriture. \n\nNotre alliance alimentaire saura bénificier à votre Empire également");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Offrir 60 Nourriture au chef de la tribu afin de développer une alliance alimentaire.\n(+1 Production Nourriture pour TOUS les village)"
        , delegate () 
        {
            Empire.instance.capitale.AddFood(-60);
            foreach (Village vil in Empire.instance.listVillage)
            {
                vil.AddFoodProd(1);
            }
            listRandomRequest.Remove(Abdul);
        }));
    listeChoix.Add(new Choice("Malheureusement, je ne peux point vous aider actuellement.", delegate () { }));
    request = new Request(message, listeChoix);
    Abdul = request;

            ///////////////////////////////////////////////////////////////////


    message = new Dialog.Message("Nous sommes d'excellents acteurs qui seront en mesure de divertir vos citoyens en échange de 100 pièces d'or.");
    listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Offrir 100 pièces d’or à la communauté et développer une alliance culturelle. \n(Améliorer sa réputation au sein de TOUS les villages)",
        delegate ()
        {
            Empire.instance.capitale.AddGold(-100);
            foreach (Village vil in Empire.instance.listVillage)
            {
                vil.AddReputation(20);
            }
            listRandomRequest.Remove(Chaka);
        }));
    listeChoix.Add(new Choice("Malheureusement, je ne peux point vous aider actuellement.", delegate () { }));
    request = new Request(message, listeChoix);
    Chaka = request;

    shuffleList<Request>(listRandomRequest);

}

private Request ExpeditionEvent2()
{

    Dialog.Message message = new Dialog.Message("Amenophis : Empereur, je viens de revenir de mon expédition et elle fut une réussite.\n\nJe vous apporte donc votre part du butin !");
    List<Choice> listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Des mets exotiques venant des quatre coins du globe ? (+8 nourriture)", delegate () { Empire.instance.capitale.AddFood(8); }));
    listeChoix.Add(new Choice("De l'or en grandes quantité ? (+16 Or)", delegate () { Empire.instance.capitale.AddGold(16); }));
    listeChoix.Add(new Choice("Des vêtements de soies pour le peuple ? (+3 Bonheur)", delegate () { Empire.instance.capitale.AddHappiness(3); }));
    Request request = new Request(message, listeChoix);
    return request;
}

private Request PotionEvent2()
{
    Request potionEvent2 = null;
    Dialog.Message message = new Dialog.Message("Lagarefix : Empereur, je viens de revenir d'un petit village gaulois qui résiste encore et toujours à l'envahisseur grâce à la potion magique.\n\nIls n'ont pas voulu me partager le secret la potion \n\nMais ils m'ont offert des graines de blé de qualité exceptionnelle ! \n(+1 Production Nourriture");
    List<Choice> listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Parfait ! Faite pousser ce blé !", delegate () { Empire.instance.capitale.AddFoodProd(1); }));
    listeChoix.Add(new Choice("C'est intolérable, préparer une armée pour détruire ce village ! \n\n Et n'oubliez pas de faire pousser ce blé ! (-6 Soldat)"
        , delegate () {
             Empire.instance.capitale.AddArmy(-6); Empire.instance.capitale.AddFoodProd(1);
             listRandomRequest.Remove(potionEvent2); listRandomRequest.Add(PotionEvent3());
        }));
    Request request = new Request(message, listeChoix);
    potionEvent2 = request;
    return request;
}

private Request PotionEvent3()
{

    Dialog.Message message = new Dialog.Message("Leopardus : Empereur, l'armée que nous avions envoyé pour détruire ce village à été détruite par deux gaulois.\n\n Mais bonne nouvelle ! Je suis encore entier !");
    List<Choice> listeChoix = new List<Choice>();
    listeChoix.Add(new Choice("Retournez dans les rangs de l'armée ! (+1 Soldat)", delegate () { Empire.instance.capitale.AddArmy(1); }));
    listeChoix.Add(new Choice("Faites moi disparaitre cet incompétent ! (+4 Or)", delegate () { Empire.instance.capitale.AddGold(4); }));
    Request request = new Request(message, listeChoix);
    return request;
}

    */
    private void shuffleList<T>(List<T> list)
    {
        int nbTri = list.Count * list.Count;
        int y;
        int x;
        T temp;
        for (int i = 0; i < nbTri; i++)
        {
            x = Random.Range(0, list.Count);
            y = Random.Range(0, list.Count);

            temp = list[x];
            list[x] = list[y];
            list[y] = temp;

        }
    }

    //Request bank methods

    public static RequestFrame GetRequestFrame(string tag)
    {
        if (requestManager == null) { Debug.LogError("Error: Request manager instance is null."); return null; }
        if (requestManager.bank == null) { Debug.LogError("Error: Request manager's bank is null."); return null; }

        return requestManager.bank.GetFrame(tag);
    }

    public static bool BuildAndSendRequest(string tag, Village source, Village destination, int value = 1, ResourceType type = ResourceType.custom, UnityAction[] callbacks = null)
    {
        RequestFrame frame = GetRequestFrame(tag);
        if (frame == null) return false;

        SendRequest(frame.Build(source, destination, value, type, callbacks));
        return true;
    }

}

