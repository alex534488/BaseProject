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
        totalText.text = "" + village.GetTotal(type);
        slider.UpdateSlider(village.reputation / 100);
        int bilan = village.GetBilan(type);
        bilanText.text = "" + bilan;
        bg.color = (bilan >= 0) ? positiveColor : negativeColor;
    }

    void OnSendClick()
    {
        int sendAmount = System.Convert.ToInt32(sendField.text);
        Carriage carriage = new Carriage(Carriage.stdDelay, village, Empire.instance.capitale, type, sendAmount);
        CarriageManager.SendCarriage(carriage);
        //SendCarriage
        UpdateDisplay();
        sendPanel.ToggleActive();
    }

    void OnRequestClick()
    {
        int sendAmount = System.Convert.ToInt32(sendField.text);
        Carriage carriage = new Carriage(Carriage.stdDelay, village, Empire.instance.capitale, type, sendAmount);
        CarriageManager.SendCarriage(carriage);
        //SendCarriage
        UpdateDisplay();
    }
}
