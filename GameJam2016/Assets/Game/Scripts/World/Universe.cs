using UnityEngine;
using System.Collections;

// Classes de haut niveau statique accessible de n'importe ou
public class Universe : INewDay {
      
    public static Universe instance;

    private World world;
    private History history;

	public Universe()
    {
        instance = this;
        world = new World();
        history = new History();
	}

    public void NewDay()
    {
        history.RecordDay(world);

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
