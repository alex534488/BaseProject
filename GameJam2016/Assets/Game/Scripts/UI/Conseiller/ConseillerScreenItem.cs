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
    public Text bilanText;
    public Text carriageText;


    Village village;
    Ressource_Type type;

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
        totalText.text = "" + village.GetResource(type);
        slider.UpdateSlider((float)(village.GetReputation() / 100.0));
        int bilan = village.GetResource(GameResources.GetAlternate(type));
        bilanText.text = "" + bilan;
        bg.color = (bilan >= 0) ? positiveColor : negativeColor;
        carriageText.text = "" + CarriageManager.GetCarriageCountAt(village);
    }

    void OnSendClick()
    {
        int sendAmount = System.Convert.ToInt32(sendField.text);
        Empire.instance.capitale.SendCartToVillage(village, type, sendAmount);
        
        UpdateDisplay();
        sendPanel.ToggleActive();
    }

    void OnRequestClick()
    {

        Empire.instance.capitale.SendCartToVillage(village, type, -1);
        UpdateDisplay();
    }
}
