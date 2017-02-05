using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using CCC.Manager;

public class DevPanelBarbareMenu : MonoBehaviour {

    public InputField inputField;
    public BarbarianClan currentClan;
    public DevPanelBarbarian devPanelBarbarian;
    public World world;

    public void SetCurrentClan(BarbarianClan currentClan, World world)
    {
        this.currentClan = currentClan;
        this.world = world;
    }

    public void OnClickMoveTo()
    {
        if(inputField.text != null)
        {
            currentClan.Move(int.Parse(inputField.text));
            devPanelBarbarian.UpdateDisplay(world);
        }
    }

    public void OnClickSpawn()
    {
        if (inputField.text != null)
        {
            currentClan.AddPower(int.Parse(inputField.text));
            devPanelBarbarian.UpdateDisplay(world);
        }
    }

    public void OnClickNewDay()
    {
        if (inputField.text != null)
        {
            for (int i = 0; i < int.Parse(inputField.text); i++)
            {
                currentClan.NewDay();
            }
            devPanelBarbarian.UpdateDisplay(world);
        }
    }

    public void OnDelete()
    {
        Universe.Barbares.Delete(currentClan);
        devPanelBarbarian.UpdateDisplay(world);
        Scenes.UnloadAsync("InteractBarbare");
    }

    public void Leave()
    {
        Scenes.UnloadAsync("InteractBarbare");
    }
}
