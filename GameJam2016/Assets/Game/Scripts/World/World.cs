using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : INewDay {

    public Empire empire;
    public BarbareManager barbareManager;
    public Map map;

    // Creation du monde du jeu
    public World()
    {
        // Creation des barbares
        barbareManager = new BarbareManager();

        // Creation de l'empire
        empire = new Empire();

        // Creation de la map
        map = new Map();
    }

    public void NewDay()
    {
        barbareManager.NewDay();
        empire.NewDay();
    }

    // TODO: Faire la fonctione qui copie world pour en faire une sauvegarde
    public World CloneState()
    {
        return this;
    }
}
