using UnityEngine;
using System.Collections;

public class Barbare : MonoBehaviour
{
    public World theWorld;
    private Village actualTarget;
    private int nbBarbares;

    void Start(){}

    void Update() {}

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

    void SpawnEnnemy()
    {
        // Rajoute 1 au nombre de barbares disponibles
    }

    void TakeDecision()
    {
        // Recupere le nombre de soldats disponible dans le village
        // Commence à 50% de chance de réussite, si 10% plus de unites cela implique 10% plus de chance de gagner la bataille ?
        
    }

}
