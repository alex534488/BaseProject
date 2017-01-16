using UnityEngine;
using System.Collections;

// Classes de haut niveau statique accessible de n'importe ou
public class Universe : INewDay {
      
    public static Universe main;

    private World world;
    private History history;

	public Universe()
    {
        main = this;
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
        { return main.world.empire; }
    }

    static public Village Capitale
    {
        get
        {
            return main.world.empire.GetCapitale();
        }
    }

    static public BarbareManager Barbares
    {
        get
        {
            return main.world.barbareManager;
        }
    }

    static public Map Map
    {
        get
        {
            return main.world.map;
        }
    }

    static public CartsManager CartsManager
    {
        get
        {
            return main.world.empire.GetCartsManager();
        }
    }

    static public History History
    {
        get
        {
            return main.history;
        }
    }
}
