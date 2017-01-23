using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DevPanel : MonoBehaviour
{
    [System.NonSerialized]
    World currentWorld = null;
    int currentDay = 0; // 0 étant ajd, -1, -2, etc. le passé

    public GameObject nullWorldError;
    public Button nextDay;
    public Button previousDay;
    [Header("Categories")]
    public DevPanelEmpire empireDisplay;
    public DevPanelBarbarian barbarianDisplay;
    public DevPanelMap mapDisplay;

    public enum Category
    {
        none,
        empire,
        barbarian,
        map
    }
    Category currentCategory = Category.none;

    void Start()
    {
        if (currentWorld == null)
            DisplayWorld(Universe.World);
    }

    void DisplayWorld(World world)
    {
        if (world == null)
            return;
        currentWorld = world;

        CheckDayButtons();
        SetCategory(currentCategory);
    }

    public void NextDay()
    {
        World world = Universe.History.ViewPast(-(currentDay + 1));
        if (world != null)
        {
            currentDay++;
            DisplayWorld(world);
        }
    }

    public void PreviousDay()
    {
        World world = Universe.History.ViewPast(-(currentDay - 1));
        if (world != null)
        {
            currentDay--;
            DisplayWorld(world);
        }
    }

    public void SetEmpire()
    {
        SetCategory(Category.empire);
    }

    public void SetBarbare()
    {
        SetCategory(Category.barbarian);
    }

    public void SetMap()
    {
        SetCategory(Category.map);
    }

    public void SetCategory(Category category)
    {
        currentCategory = category;
        switch (currentCategory)
        {
            default:
            case Category.none:
                empireDisplay.Hide();
                barbarianDisplay.Hide();
                mapDisplay.Hide();
                break;
            case Category.empire:
                empireDisplay.Show(currentWorld);
                barbarianDisplay.Hide();
                mapDisplay.Hide();
                break;
            case Category.barbarian:
                empireDisplay.Hide();
                barbarianDisplay.Show();
                mapDisplay.Hide();
                break;
            case Category.map:
                empireDisplay.Hide();
                barbarianDisplay.Hide();
                mapDisplay.Show();
                break;
        }
    }

    void CheckDayButtons()
    {
        previousDay.interactable = Universe.History.ViewPast(-(currentDay - 1)) != null;
        nextDay.interactable = Universe.History.ViewPast(-(currentDay + 1)) != null;
    }

    void ErrorNullWorld()
    {
        if (nullWorldError != null)
            nullWorldError.SetActive(true);
    }
}
