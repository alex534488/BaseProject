using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DevPanelEmpire : MonoBehaviour
{
    Color highlightColor = new Color(197.0f / 255.0f, 255.0f / 255.0f, 187.0f / 255.0f);
    Empire empire;
    Village village = null;
    int buildingPage = 0;


    public Button mainButton;

    [Header("CartManager")]
    public Text cartsLeft;

    [Header("Villages")]
    public VerticalLayoutGroup villagesVerticalLayout;

    [Header("Close-up")]
    public Text closeup_infoText;
    public VerticalLayoutGroup buildingVerticalLayout;
    public InputField closeup_buildBuildingText;
    [Range(1, 20)]
    public int closeup_buildingPerPage = 10;
    public Button previousBuildingPage;
    public Button nextBuildingPage;
    //private Architect architect;

    void Awake()
    {
        //Set les listeners sur la listes des villages
        for (int i = 0; i < villagesVerticalLayout.transform.childCount; i++)
        {
            Transform child = villagesVerticalLayout.transform.GetChild(i);
            int index = i;
            child.GetComponent<Button>().onClick.AddListener(
                delegate
                {
                    SetCity(index);
                });
        }
        previousBuildingPage.onClick.AddListener(PreviousBuildingPage);
        nextBuildingPage.onClick.AddListener(NextBuildingPage);
    }

    public void Show(World world)
    {
        this.empire = world.empire;
        mainButton.image.color = highlightColor;
        gameObject.SetActive(true);
        UpdateAll();
    }

    public void Hide()
    {
        mainButton.image.color = Color.white;
        empire = null;
        village = null;
        buildingPage = 0;

        gameObject.SetActive(false);
    }

    public void Build()
    {
        if (village != null && village.Architect != null)
        {
            village.Architect.Build(closeup_buildBuildingText.text);
            UpdateCloseup();
        }
    }

    void SetCity(int index)
    {
        village = empire.VillageList[index];
        buildingPage = 0;
        villagesVerticalLayout.transform.GetChild(index).GetComponent<Button>().image.color = highlightColor;
        UpdateCloseup();
    }

    void UpdateAll()
    {
        //Carts
        UpdateCarts();

        //Village list
        UpdateVillageList();

        //Close-up
        UpdateCloseup();
    }

    void UpdateCloseup()
    {
        ClearBuildingItems();
        if (village != null)
        {
            closeup_infoText.text = "Name: " + village.Name + "\n\n"
                + "capital: " + village.IsCapital() + "\n\n"
                + "map position: " + village.GetMapPosition() + "\n\n"
                + "army power: " + village.Get(Village_ResourceType.armyPower) + "\n"
                + "army cost: " + village.Get(Village_ResourceType.armyCost) + "\n\n"
                + "gold prod.: " + village.Get(Village_ResourceType.goldProd) + "\n"
                + "material prod.: " + village.Get(Village_ResourceType.materialProd) + "\n"
                + "science prod.: " + village.Get(Village_ResourceType.scienceProd) + "\n"
                + "food: " + village.Get(Village_ResourceType.food);

            if (village.Architect != null)
            {
                List<string> buildings = new List<string>(village.Architect.BuildingsName);

                if (closeup_buildingPerPage <= 0)
                    return;

                int startIndex = buildingPage * closeup_buildingPerPage;
                int endIndex = Mathf.Min(startIndex + closeup_buildingPerPage, buildings.Count);
                for (int i = startIndex; i < endIndex; i++)
                {
                    AddBuildingItem(buildings[i]);
                }
            }
        }
        else
        {
            closeup_infoText.text = "";
        }
        UpdateBuildingArrows();
    }

    void NextBuildingPage()
    {
        buildingPage++;
        UpdateCloseup();
    }

    void PreviousBuildingPage()
    {
        buildingPage--;
        UpdateCloseup();
    }

    void UpdateBuildingArrows()
    {
        if (village == null || village.Architect == null || village.Architect.BuildingsName.Count <= closeup_buildingPerPage)
        {
            previousBuildingPage.interactable = false;
            nextBuildingPage.interactable = false;
            return;
        }
        int total = village.Architect.BuildingsName.Count;
        int startIndex = buildingPage * closeup_buildingPerPage;
        int endIndex = Mathf.Min(startIndex + closeup_buildingPerPage, total);

        previousBuildingPage.interactable = startIndex > 0;
        nextBuildingPage.interactable = endIndex < total;
    }

    void UpdateVillageList()
    {
        int i = 0;
        foreach (Village village in empire.VillageList)
        {
            Transform child = villagesVerticalLayout.transform.GetChild(i);
            child.GetComponent<Button>().interactable = true;
            child.GetComponent<Button>().image.color = Color.white;
            child.GetComponentInChildren<Text>().text = village.Name;
            i++;
        }
        //On ferme tout les autre boutons de trop
        while (i < 6)
        {
            Transform child = villagesVerticalLayout.transform.GetChild(i);
            child.GetComponent<Button>().interactable = false;
            child.GetComponentInChildren<Text>().text = "";
            i++;
        }
    }

    void UpdateCarts()
    {
        cartsLeft.text = "Carts left: " + empire.CartsManager.AvailableCarts + " / " + empire.CartsManager.TotalCarts;
    }

    void AddBuildingItem(string name)
    {
        Text prefab = buildingVerticalLayout.transform.GetChild(0).GetComponent<Text>();
        Text obj = Instantiate(prefab.gameObject).GetComponent<Text>();
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(buildingVerticalLayout.transform);
        obj.transform.localScale = Vector3.one;
        obj.text = name;
        obj.GetComponentInChildren<Button>().onClick.AddListener(
            delegate ()
            {
                DestroyBuilding(obj.transform.GetSiblingIndex() -1);
            });
    }

    void ClearBuildingItems()
    {
        for(int i=1; i< buildingVerticalLayout.transform.childCount; i++)
        { 
            Destroy(buildingVerticalLayout.transform.GetChild(i).gameObject);
        }
    }

    void DestroyBuilding(int position)
    {
        if (village == null || village.Architect == null)
            return;
        village.Architect.DestroyBuildingAt(position);
        UpdateCloseup();
    }
}
