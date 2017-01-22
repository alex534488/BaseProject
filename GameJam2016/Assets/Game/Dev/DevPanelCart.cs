using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine.UI;

public class DevPanelCart : MonoBehaviour
{

    public GameObject container;
    public Text fromText;
    public Text toText;
    public Text remainingDaysText;
    public PointerListener button;
    public GameObject transactionPanel;

    private Cart cart;

    void Awake()
    {
        button.onPointerEnter.AddListener(ShowTransactions);
        button.onPointerExit.AddListener(HideTransactions);
        Hide();
    }
    public void Show(Cart cart)
    {
        this.cart = cart;
        Village source = Universe.Empire.GetVillageAtPos(cart.MapSource);
        Village destination = Universe.Empire.GetVillageAtPos(cart.MapDestination);
        fromText.text = source == null ? cart.MapSource.ToString() : TermToText.Term(source);
        toText.text = destination == null ? cart.MapDestination.ToString() : TermToText.Term(destination);

        remainingDaysText.text = "arrive in " + cart.RemainingDays.ToString() + " days";
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
        HideTransactions();
    }

    void ShowTransactions()
    {
        ReadOnlyCollection<Transaction> start = cart.StartTransactions;
        ReadOnlyCollection<Transaction> arrive = cart.ArriveTransactions;

        string completeText = "";
        if (start != null)
            foreach (Transaction transac in start)
            {
                completeText += ": " + TermToText.Term(transac, true) + '\n';
            }
        if (arrive != null)
            foreach (Transaction transac in arrive)
            {
                completeText += ": " + TermToText.Term(transac, true) + '\n';
            }

        transactionPanel.SetActive(true);
        transactionPanel.GetComponentInChildren<Text>().text = completeText;
    }
    void HideTransactions()
    {
        transactionPanel.SetActive(false);
    }
}
