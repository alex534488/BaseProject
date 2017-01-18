using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BarbareManager : INewDay {

    private List<BarbarianClan> listClans = new List<BarbarianClan>();

    public void Init()
    {
        // A faire en debut de partie pour initialiser les barbres 
        // Utilise Spawn
    }

    public void NewDay()
    {
        //TODO: IA qui prend des decisions sur le positionnement et l'appartition des barbares
        // Utilise Spawn et Send
    }

    public List<BarbarianClan> GetClans(int position)
    {
        List<BarbarianClan> result = new List<BarbarianClan>();
        for (int i = 0; i < listClans.Count; i++)
        {
            if(listClans[i].GetPosition() == position)
            {
                result.Add(listClans[i]);
            }
        }
        return result;
    }

    private void Spawn(int amount, List<int> positions)
    {
        // TODO: Repartitions de la quantite sur tous les territoires de facon le plus equitable et aleatoire
    }

    // Envoie une certaine quantite de troupe sur une position
    private void Send(int amount, int position)
    {
        // verifie les positions adjacentes
        // trouve les barbares se trouvant sur ceux-ci
        // Envoie la quantite requise sur la position en
        // les deplacant, si pas assez de troupe, on envoie le
        // maximum possible
    }

    public void IsBeingAttacked(int position)
    {
        // A enlever possiblement, peut etre utile s'il y a des animations relies au barbare lorsqu'un village attaque des barbares
    }
}
