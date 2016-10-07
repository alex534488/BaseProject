using UnityEngine;
using System.Collections;

public class Barbare : IUpdate
{

    public World theWorld;
    private Village actualTarget;
    private bool batailleEnCours = false;

    private int nbBarbares;
    private int nbSoldats;
    private int nbUnites;

    private int probabiliteSoldat = 50;
    private int probabiliteBarbare = 50;


    void Start(){}

    public void Update() {}

    void AskTarget() // Retourne le village frontiere avec le moins de soldats disponibles
    {
        actualTarget = theWorld.GiveTarget(); 
    }

    void WaitForAttack(int nbTours) // Attend un certain nombre de tours avant de attaquer
    {
        // Ajoute un Listener sur la fonction de fin de tour
        // Chaque fois que elle se declenche, decroit le compteur de 1
        TakeDecision(); // Appelle la fonction TakeDecision qui determinera si les barbares attaqueront le village;
    }

    void SpawnEnnemy(int nbUnites)
    {
        nbBarbares = nbBarbares + nbUnites;
    }

    void TakeDecision()
    {
        // Recupere le nombre de soldats disponible dans le village

        // Si le barbare ne possede pas au moins 1 tiers des unites total disponible, il ne va simplement pas attaquer
        // Exemple 1 : 50 barbares - 100 soldats = 150 unites totales -> 50 sur 150 = 33% -> Attaque
        // Exemple 2 : 30 barbares - 100 soldats = 130 unites totales -> 30 sur 130 = 23% -> Retraite

        // Le clan qui possede le plus de unites benificient de un bonus probabilistique de tuer les unites ennemies
        // Exemple 1 :
        //              Forces de clan : 50 babares et 100 soldats
        //              Barbares : 33% unités et Soldats : 67% unités Bonus de 34% (67-33)
        //              Probabilite de base de tuer une unite est de 50%
        //              Soldats ont 84% de tuer une unite lorsque ils attaquent dans cette situation alors que les barbares ont 50% de base

        // Exemple 2 :
        //              Forces de clan : 100 babares et 75 soldats
        //              Barbares : 57% unités et Soldats : 43% unités => Bonus de 14% (57-43)
        //              Probabilite de base de tuer une unite est de 50%
        //              Barbares ont 64% de tuer une unite lorsque ils attaquent dans cette situation alors que les soldats ont 50% de base

        // Deroulement du combat :
        //                         Si le nombre de barbares est plus grand que 0, Attack()
        //                         Si le barbare tue une unite, il enleve 1 au nombre de soldat ennemie
        //                         Si le nombre de soldat ennemi est plus grand que 0, EstAttaque()
        //                         Boucle jusqua ce que un des deux clans perdent

        // nbSoldats = Village.army;
        nbUnites = nbSoldats + nbBarbares;

        if (nbBarbares / nbUnites >= 1 / 3)
        {
            CalculProbabilite();
            Bataille();
        }

        else
            Retraite();
    }

    void Retraite()
    {
        // Definit un nouveau village cible. Il peut s'agir du même village sans aucun problème.
        AskTarget(); 
    }

    void AttaqueBarbare()
    {
        float randomNumber;

        randomNumber = Random.Range(0, 101);

        if (probabiliteBarbare >= randomNumber)
            nbSoldats = nbSoldats - 1;
    }

    void AttaqueSoldat()
    {
        float randomNumber;

        randomNumber = Random.Range(0f, 100f);

        if (probabiliteSoldat >= randomNumber)
            nbBarbares = nbBarbares - 1;
    }

    void CalculProbabilite()
    {
        int pourcentageBarbare = Mathf.RoundToInt(nbBarbares / nbUnites * 100f);
        int pourcentageSoldat = 100 - pourcentageBarbare;

        if (pourcentageBarbare >= pourcentageSoldat)
        {
            probabiliteBarbare = pourcentageSoldat - pourcentageBarbare + 50;
            probabiliteSoldat = 50;
        }

        else
        {
            probabiliteSoldat= pourcentageSoldat - pourcentageBarbare + 50;
            probabiliteBarbare = 50;
        }
    }

    void VictoireBarbare() // TO DO : Effet de la victoire des barbares
    {
        // Effets de la victoire des Barbares
        batailleEnCours = false;
    }

    void VictoireSoldat() // TO DO : Effet de la victoire des soldats
    {
        // Effets de la victoire des Soldats
        batailleEnCours = false;
    }

    void Bataille()
    {
        AttaqueBarbare();

        if (nbSoldats <= 0)
            VictoireBarbare();

        AttaqueSoldat();

        if (nbBarbares <= 0)
            VictoireSoldat();

        if (batailleEnCours == true)
            Bataille();
    }

}
