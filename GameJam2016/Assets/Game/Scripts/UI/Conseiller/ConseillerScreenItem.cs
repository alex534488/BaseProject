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
    public SliderColor slider; 
    [Header("Buttons")]
    public ToggleGameObject sendPanel;
    public Button sendButton;
    public InputField sendField;
    public Button requestButton;
    [Header("Texts")]
    public Text totalText;
    public Text prodText;


    Village village;
    Ressource_Type type;

    Ligne ligne;

    void Awake()
    {
        sendButton.onClick.AddListener(OnSendClick);
        requestButton.onClick.AddListener(OnRequestClick);
    }

    public void Display(Village village, Ressource_Type type)
    {
        this.type = type;
        this.village = village;

        cityName.text = village.nom;

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        ligne = village.GetInfos(type);

        
        totalText.text = "" + ligne.total;
        print("change prod amount ! + slider color");
        //slider.UpdateSlider(reputation);
        prodText.text = "" + ligne.production;
        bg.color = (ligne.production - ligne.taxe > 0) ? positiveColor : negativeColor;
    }

    void OnSendClick()
    {
        print("send cariage");
        //SendCarriage
        int sendAmount = System.Convert.ToInt32(sendField.text);
        UpdateDisplay();
        sendPanel.ToggleActive();
    }

    void OnRequestClick()
    {
        print("request cariage");
        //SendCarriage
        UpdateDisplay();
    }
}
