using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using CCC.UI;

public class MapCityPanel : MonoBehaviour
{

    [Header("Settings have to be set in the button gameObjects")]
    public WindowAnimation mainPanel;
    public Button hitbox;
    public WindowAnimation extension;
    public Text carriageAmount;
    public Button requestButton;
    public Button sendButton;

    public class IntEvent : UnityEvent<int> { }
    public IntEvent onSend = new IntEvent();
    public UnityEvent onRequest = new UnityEvent();
    public UnityEvent onClose = new UnityEvent();

    bool closing = false;

    #region Open/close
    public void Open(Vector2 position, bool buttonsEnabled = true)
    {
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
    }

    public void RequestClick()
    {
        onRequest.Invoke();
        Close();
    }

    void SetCarriageText(int amount)
    {
        if (carriageAmount != null)
            carriageAmount.text = "Remaining          : " + amount;
    }

}
