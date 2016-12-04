using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using CCC.UI;

public class MapCityPanel : MonoBehaviour
{

    public WindowAnimation mainPanel;
    public Button hitbox;
    public WindowAnimation extension;
    public Text carriageAmount;
    [Header("Buttons")]
    [Space()]
    public Button requestButton;
    public Button sendButton;
    [Header("High")]
    public int sendHighAmount;
    public Button sendHighButton;
    [Header("Medium")]
    public int sendMedAmount;
    public Button sendMedButton;
    [Header("Low")]
    public int sendLowAmount;
    public Button sendLowButton;

    public class IntEvent : UnityEvent<int> { }
    public IntEvent onSend = new IntEvent();
    public UnityEvent onRequest = new UnityEvent();
    public UnityEvent onClose = new UnityEvent();

    bool closing = false;

    void Start()
    {
        //Add listeners
        sendLowButton.onClick.AddListener(SendLowClick);
        sendMedButton.onClick.AddListener(SendMedClick);
        sendHighButton.onClick.AddListener(SendHighClick);

        //Set texts
        sendLowButton.GetComponentInChildren<Text>().text = "Send " + sendLowAmount;
        sendMedButton.GetComponentInChildren<Text>().text = "Send " + sendMedAmount;
        sendHighButton.GetComponentInChildren<Text>().text = "Send " + sendHighAmount;
    }

    #region Open/close
    public void Open(Vector2 position, bool buttonsEnabled = true)
    {
        UpdateSendButtons();

        //Enable/Disable les boutons (on veut pas que les boutons soit enabled quand on est en mode 'reputation')
        requestButton.interactable = buttonsEnabled;
        sendButton.interactable = buttonsEnabled;

        mainPanel.GetComponent<RectTransform>().position = position;
        mainPanel.Open();
        mainPanel.gameObject.SetActive(true);
        hitbox.gameObject.SetActive(true);
    }

    public void Close()
    {
        if (closing)
            return;
        closing = true;

        mainPanel.Close(OnCloseComplete);
        CloseExtension();
    }

    void OnCloseComplete()
    {
        mainPanel.gameObject.SetActive(false);
        hitbox.gameObject.SetActive(false);
        onClose.Invoke();
        closing = false;
    }
    #endregion


    #region Extension open/close

    public void ToggleExtension()
    {
        if (!extension.IsOpen())
            OpenExtension();
        else
            CloseExtension();
    }

    public void OpenExtension()
    {
        if (extension.IsOpen()) return;

        extension.gameObject.SetActive(true);
        extension.Open();
        SetCarriageText(Empire.instance.capitale.charriot);
    }

    public void CloseExtension()
    {
        if (!extension.IsOpen()) return;

        extension.Close(OnExtensionCloseComplete);
    }

    void OnExtensionCloseComplete()
    {
        extension.gameObject.SetActive(false);
    }
    #endregion

    public void SendClick(int amount)
    {
        onSend.Invoke(amount);
        Close();
        SetCarriageText(Empire.instance.capitale.charriot);
    }

    public void RequestClick()
    {
        onRequest.Invoke();
        Close();
        SetCarriageText(Empire.instance.capitale.charriot);
    }

    void SetCarriageText(int amount)
    {
        if (carriageAmount != null)
            carriageAmount.text = ": " + amount;
    }

    void UpdateSendButtons()
    {
        Village capital = Empire.instance.capitale;
        Resource_Type type = MapLens.CurrentType();
        int amount = capital.GetResource(type);

        sendLowButton.interactable = amount >= sendLowAmount;
        sendMedButton.interactable = amount >= sendMedAmount;
        sendHighButton.interactable = amount >= sendHighAmount;
    }

    #region Send Button Events
    void SendHighClick()
    {
        SendClick(sendHighAmount);
    }
    void SendMedClick()
    {
        SendClick(sendMedAmount);
    }
    void SendLowClick()
    {
        SendClick(sendLowAmount);
    }
    #endregion
}
