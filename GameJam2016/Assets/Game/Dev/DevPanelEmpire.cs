using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DevPanelEmpire : MonoBehaviour
{
    public Button mainButton;

    Color highlightColor = new Color(197.0f / 255.0f, 255.0f / 255.0f, 187.0f / 255.0f);
    Empire empire;

    [Header("CartManager")]
    public Text cartsLeft;

    [Header("Villages")]
    public VerticalLayoutGroup villagesVerticalLayout;

    [Header("Close-up")]
    public Button gna;

    public void Show(World world)
    {
        this.empire = world.empire;
        mainButton.image.color = highlightColor;
        gameObject.SetActive(true);
        UpdateContent();
    }
    public void Hide()
    {
        mainButton.image.color = Color.white;
        gameObject.SetActive(false);
    }

    void UpdateContent()
    {
        //Carts

        cartsLeft.text = "Carts left: " + empire.CartsManager.AvailableCarts + " / " + empire.CartsManager.TotalCarts;


        //Village list

        int i = 0;
        foreach (Village village in empire.VillageList)
        {
            Transform child = villagesVerticalLayout.transform.GetChild(i);
            child.GetComponent<Button>().interactable = true;
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
}
