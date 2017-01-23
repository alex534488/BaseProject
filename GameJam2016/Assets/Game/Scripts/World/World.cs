using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : INewDay {

    public Empire empire;
    public BarbareManager barbareManager;
    public Map map;

    // Creation du monde du jeu
    public void Init()
    {
        // Creation de la map
        map = new Map();

        // Creation des barbares
        barbareManager = new BarbareManager();

        // Creation de l'empire
        empire = new Empire();
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
