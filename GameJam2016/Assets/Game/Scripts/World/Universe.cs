using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// Classes de haut niveau statique accessible de n'importe ou
public class Universe : INewDay {
      
    public static Universe instance;

    public World world;
    public History history;
    public UnityEvent onSetNewWorld = new UnityEvent();
    static public UnityEvent OnSetNewWorld { get { return instance.onSetNewWorld; } }

    public Universe(World loadedWorld = null, History loadedHistory = null)
    {
        instance = this;

        //World
        if (loadedWorld != null)
        {
            world = loadedWorld;
        }
        else
        {
            world = new World();
            world.Init();
        }

        //History
        if (loadedHistory != null)
        {
            history = loadedHistory;
        }
        else
        {
            history = new History();
        }
	}

    public void ArrivalDay()
    {
    }

    public void SetWorldTo(World world)
    {
        this.world = world;
        onSetNewWorld.Invoke();
    }

    public void NewDay()
    {
        world.NewDay();
    }

    static public Empire Empire
    {
        get
        { return instance.world.empire; }
    }

    static public Village Capitale
    {
        get
        {
            return instance.world.empire.Capitale();
        }
    }

    static public BarbareManager Barbares
    {
        get
        {
            return instance.world.barbareManager;
        }
    }

    static public Map Map
    {
        get
        {
            return instance.world.map;
        }
    }

    static public CartsManager CartsManager
    {
        get
        {
            return instance.world.empire.CartsManager;
        }
    }

    static public History History
    {
        get
        {
            return instance.history;
        }
    }

    static public World World
    {
        get { return instance.world; }
    }
}
