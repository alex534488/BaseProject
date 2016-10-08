using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConseillerScreenItem : MonoBehaviour
{
    public Image bg;
    public Color negativeColor = Color.red;
    public Color positiveColor = Color.green;
    public Text cityName;
    public Button upButton;
    public Button downButton;
    [Header("Texts")]
    public Text totalText;
    public Text prodText;
    public Text taxeText;
    public Text bilanText;


    Village village;
    Ressource_Type type;

    Ligne ligne;

    void Awake()
    {
        upButton.onClick.AddListener(OnUpClick);
        downButton.onClick.AddListener(OnDownClick);
    }

    public void Display(Village village, Ressource_Type type)
    {
        this.type = type;
        this.village = village;

        cityName.text = village.nom;

        ligne = village.GetInfos(type);

        UpdateTexts();
    }

    void UpdateTexts()
    {
        totalText.text = "" + ligne.total;
        prodText.text = "" + ligne.production;
        taxeText.text = "" + ligne.taxe;
        bilanText.text = "" + (ligne.production - ligne.taxe);
        bg.color = (ligne.production - ligne.taxe > 0) ? positiveColor : negativeColor;
    }

    void OnUpClick()
    {
        ligne.taxe = village.ModifyTaxe(type, 1);
        UpdateTexts();
    }

    void OnDownClick()
    {
        ligne.taxe = village.ModifyTaxe(type, -1);
        UpdateTexts();
    }
}
