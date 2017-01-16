using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barbare : INewDay
{
    public void NewDay()
    {

    }
    /*
    public float facteur = 1.2f;
    public int delay = 3;

    private Village nextTarget = null;
    public Village actualTarget = null;

    private bool batailleEnCours = false;
    private int waitForAttack = 0;

    #region Nombre de units
    public int nbBarbares = 0;

    private int nbUnites = 0;

    private int totalBarbare = 0;
    private int totalSoldats = 0;

    private int barbareRestant = 0;
    private int soldatRestant = 0;

    public int spawnRate = 2;
    #endregion

    #region Probabilites
    private int probabiliteSoldat = 50;
    private int probabiliteBarbare = 50;
    #endregion

    public Barbare()
    {

    }

    void Start()
    {
        
    }

    public void NewDay() 
    {
        if (actualTarget != null)
        {
           
            waitForAttack = waitForAttack - 1;

            if (waitForAttack <= 0)
                TakeDecision();
        }

        else
        {

            if (nextTarget == null)
            {
                AskTarget();
            }
            SpawnEnnemy(spawnRate);
            AmIStrongEnough();
           
        }

        if (nextTarget != null && nextTarget.isAttacked && World.main.GiveTarget()[0].isAttacked == false)
        {
            nextTarget = null;
        }
    }

    #region Gestion

    void AskTarget() // Retourne le village frontiere avec le moins de soldats disponibles
    {
        List<Village> listNextTarget = World.main.GiveTarget();
        nextTarget = listNextTarget[0];
        AmIStrongEnough();
    }

    void AmIStrongEnough()
    {
        if (nbBarbares >= nextTarget.GetArmy() * facteur)
        {
            actualTarget = nextTarget;
            actualTarget.BeingAttack(this);
            WaitForAttack(delay);
        }
    }

    void SpawnEnnemy(int nbUnites) // Ajoute X barbares au force disponible 
    {
        nbBarbares = nbBarbares + nbUnites;
    } 

    void WaitForAttack(int nbTours) // Initialise le nombre de tour avant que les barbares passent a lattaque 
    {
        waitForAttack = nbTours;
    } 

    void ResetValues()
    {
        nbBarbares = barbareRestant;       // Met a jour le nombre de barbares restant dans le clan
        actualTarget.SetArmy(soldatRestant); // Met a jour la valeur de soldats dans Village

        totalBarbare = 0;
        totalSoldats = 0;

        nbUnites = 0;

        barbareRestant = 0;
        soldatRestant = 0;

        nextTarget = null;
        actualTarget = null;

        batailleEnCours = false;
    }
    #endregion

    #region Combat

    void TakeDecision()
    {
        soldatRestant = actualTarget.GetArmy();
        barbareRestant = nbBarbares;

        nbUnites = soldatRestant + barbareRestant;

        totalBarbare = nbBarbares;
        totalSoldats = soldatRestant;

        if (actualTarget.isDestroyed == true)
        {
            ResetValues();
            return;
        }

        if (barbareRestant / nbUnites >= 1 / 3)
        {
            CalculProbabilite();
            Bataille();
        }

        else
            Retraite();

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


    } // Prend la decision de attaquer le village ou non selon les effectifs

    void Retraite()
    {
        // Definit un nouveau village cible. Il peut s'agir du même village sans aucun problème.
        actualTarget=null;
        AskTarget();
    } // Lorsque les barbares decident de ne pas attaquer le village en question

    void AttaqueBarbare()
    {
        float randomNumber;

        randomNumber = Random.Range(0, 101);

        if (probabiliteBarbare >= randomNumber)
            soldatRestant = soldatRestant- 1;

    } // Lorsque une unite barbare attaque

    void AttaqueSoldat() // Lorsque une unite soldat attaque 
    {
        float randomNumber;

        randomNumber = Random.Range(0f, 100f);

        if (probabiliteSoldat >= randomNumber)
            barbareRestant = barbareRestant - 1;
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
    } // Determine la probabilite des barbares et des soldats concernant leur capacite a tuer une unite ennemie

    void VictoireBarbare() 
    {
        actualTarget.isAttacked = false;
        actualTarget.isDestroyed = true;

        Dialog.Message message = new Dialog.Message();
        List<Choice> listeChoix = null;

        message.text = " Mon seigneur, je vous apporte le rapport de combat du village " + actualTarget.nom + "\n" + " * Le messager vous remet le papyrus sur lequel se lit : * "
            + "% Barbares éliminées: " + (totalBarbare - barbareRestant) + "\n" + " Soldats éliminés: " + (totalSoldats - soldatRestant) + "\n" + " Soldat restant: " + (soldatRestant) + "\n" + " Le village " + actualTarget.nom + " a succombé à l'invasion de barbares";
        message.forceSeparation = '%';

        ResetValues();

        Request rapportCombat= new Request(message, listeChoix);

       
        RequestManager.SendRequest(rapportCombat);
    }

    void VictoireSoldat()
    {
        actualTarget.isAttacked = false;

        Dialog.Message message = new Dialog.Message();
        List<Choice> listeChoix = null;

        message.text = " Mon seigneur, je vous apporte le rapport de combat du village " + actualTarget.nom + "\n" + " * Le messager vous remet le papyrus sur lequel se lit : * "
            + "% Barbares éliminées: " + (totalBarbare - barbareRestant) + "\n" + " Soldats éliminés: " + (totalSoldats - soldatRestant) + "\n" + " Soldat restant: " + (soldatRestant) + "\n" + " Le village " + actualTarget.nom + " a repoussé avec succès l'invasion de barbares";
        message.forceSeparation = '%';

        ResetValues();

        Request rapportCombat = new Request(message, listeChoix);

        RequestManager.SendRequest(rapportCombat);
    }

    void Bataille()
    {
        if(actualTarget.isDestroyed)
        {
            ResetValues();
            return;
        }
        batailleEnCours = true;
        AttaqueBarbare();

        if (soldatRestant <= 0)
        {
            VictoireBarbare();
            return;
        }
            

        AttaqueSoldat();

        if (barbareRestant <= 0)
        {
            VictoireSoldat();
            return;
        }
           
        if (batailleEnCours == true)
            Bataille();
    } // Boucle qui se termine lorsque un des deux clans ne possedent plus de unites

    #endregion
    */
}