using UnityEngine;
using System.Collections;

// Classes de haut niveau statique accessible de n'importe ou
public class Universe : INewDay {
      
    public static Universe main;

    private World world;
    private History history;

	public Universe() {
        main = this;
        world = new World();
	}

    public void NewDay()
    {
        world.NewDay();
    }

    public Empire GetEmpire()
    {
        return world.empire;
    }

    public Village GetCapitale()
    {
        return world.empire.GetCapitale();
    }

    public BarbareManager GetBarbares()
    {
        return world.barbareManager;
    }

    public Map GetMap()
    {
        return world.map;
    }

    public CartsManager GetCartsManager()
    {
        return world.empire.GetCartsManager();
    }
}
