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
        barbareManager.Initialize();

        // Creation de l'empire
        empire = new Empire();
        empire.Start();

        // Creation de la map
        map = new Map();
        map.Start();
    }

    public void NewDay()
    {
        barbareManager.Uptade();
        empire.NewDay();
    }

    // TODO: Faire la fonctione qui copie world pour en faire une sauvegarde
    public World CloneState()
    {
        return this;
    }
}
