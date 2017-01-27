using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class World : INewDay {

    private int currentDay = 1;
    public int CurrentDay { get { return currentDay; } }
    public Empire empire;
    [System.NonSerialized] //Temporaire
    public BarbareManager barbareManager;
    public Map map;

    // Creation du monde du jeu, appelé slmt la première fois (et non)
    public void Init()
    {
        // Creation de la map
        map = new Map();

        // Creation des barbares
        barbareManager = new BarbareManager();

        // Creation de l'empire
        empire = new Empire();
    }

    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {
        barbareManager = new BarbareManager(); //Temporaire
    }

    public void NewDay()
    {
        currentDay++;
        barbareManager.NewDay();
        empire.NewDay();
    }

    // TODO: Faire la fonctione qui copie world pour en faire une sauvegarde
    public World CloneState()
    {
        return this;
    }
}
