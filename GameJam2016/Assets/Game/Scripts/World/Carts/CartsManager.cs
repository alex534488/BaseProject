using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using CCC.Utility;

public class CartsManager : INewDay
{
    List<Cart> ongoingCarts = new List<Cart>();

    private Stat<int> availableCarts = new Stat<int>(0, 0, 10, BoundMode.Cap);

    public CartsManager(int startingAmount)
    {
        availableCarts.Set(startingAmount);
    }

    public int AvailableCarts
    {
        get { return availableCarts; }
    }

    public int TotalCarts
    {
        get { return availableCarts + ongoingCarts.Count; }
    }

    public Stat<int>.StatEvent OnAvailableCartChange
    {
        get { return availableCarts.onSet; }
    }

    //TODO: Ce qu'il y a dans cette fonction marchait, mais c'était un peut difficile de s'y retrouver.
    //      Je propose qu'on tente de la subdivisé en plus petite tache/fonction si possible
    public void NewDay()
    {
        // On met a jour les chariots
        UpdateCarts();
    }

    private void UpdateCarts()
    {
        int arrivedCartCount = 0;
        for (int i = 0; i < ongoingCarts.Count; i++)
        {
            if (ongoingCarts[i].Progress())
            {
                ongoingCarts.RemoveAt(i);
                arrivedCartCount++;
                i--;
            }
        }
        //On remet le cart en 'available'
        availableCarts.Set(availableCarts + arrivedCartCount);
    }

    // Envoie le chariot pour envoye une resource specifique a ce village (pour l'instant seulement l'armee)
    public void SendCartVillage()
    {
        // TODO
    }

    // Envoie le chariot faire des echanges avec les autres villages (donne de l'or)
    public void SendCartTrade()
    {
        // TODO
    }

    // Envoie le chariot a l'exterieur de la map pour decouvrir des resources aleatoires
    public void SendCartExplore()
    {
        // TODO
    }
}
