using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class DialogText : MonoBehaviour
{

    public class DialogTransactionItem
    {
        public int value = 0;
        public ResourceType type;
    }
    public class DialogTextEvent : UnityEvent<DialogText> { }
    [Header("Component")]
    public Text text;
    public PointerListener button;
    public GameObject transactionContainer;
    public Text sourceName;
    public Text destinationName;
    public RectTransform leftItemSlot;
    public RectTransform rightItemSlot;
    public RectTransform leftItemPrefab;
    public RectTransform rightItemPrefab;
    [Header("Variables")]
    public Color normalColor = Color.black;
    public Color highlightColor = Color.gray;
    public float characterSpeed = 0.1f;
    public bool highlightOnHover = false;
    public DialogTextEvent onChoose = new DialogTextEvent();

    bool hasInit = false;
    bool isAnimating = false;
    Tweener tweener = null;

    string message;

    void Awake()
    {
        if (!hasInit) gameObject.SetActive(false);

        button.onPointerEnter.AddListener(OnPointerEnter);
        button.onPointerExit.AddListener(OnPointerExit);
        button.onClick.AddListener(OnClick);
    }

    public void Init(string message, bool isAnimated)
    {
        this.message = message;

        if (isAnimated)
        {
            text.text = "";
            isAnimating = true;
            int characterCount = message.Length;
            tweener = text.DOText(message, characterCount / characterSpeed, false, ScrambleMode.None).SetEase(Ease.Linear).OnComplete(OnCompletAnim);
        }
        else
        {
            text.text = message;
        }

        //Transaction

        hasInit = true;
        gameObject.SetActive(true);
    }

    public void Init(string message, bool isAnimated, List<Transaction> transactions)
    {
        if (transactions != null && transactions.Count > 0) DisplayTransactions(transactions);
        Init(message, isAnimated);
    }

    public void DisplayTransactions(List<Transaction> transactions)
    {
        List<Village> villageConcernée = new List<Village>();
        Village rightVillage = null;
        Village leftVillage = null;
        bool multipleLeftVillages = false;

        //Ajoutes tous les villages concernées à une liste
        foreach (Transaction transaction in transactions)
        {
            if (transaction.source != null && !villageConcernée.Contains(transaction.source)) villageConcernée.Add(transaction.source);
            if (transaction.destination != null && !villageConcernée.Contains(transaction.destination)) villageConcernée.Add(transaction.destination);
        }

        //Met le village de droite et de gauche. La capitale est a droite en priorité.
        foreach (Village village in villageConcernée)
        {
            if (village == Empire.instance.capitale) rightVillage = village;
            else if (leftVillage == null) leftVillage = village;
            else if (rightVillage == null) rightVillage = village;
        }
        if (villageConcernée.Count > 2) multipleLeftVillages = true;


        List<DialogTransactionItem> leftList = new List<DialogTransactionItem>();
        List<DialogTransactionItem> rightList = new List<DialogTransactionItem>();

        //Construits les liste de valeurs (compile les transaction en version abrégé)
        foreach (Transaction transaction in transactions)
        {
            if(transaction.source != null)
            {
                if(transaction.source == rightVillage)
                    AddToItems(rightList, transaction.type, -transaction.value);
                else
                    AddToItems(leftList, transaction.type, -transaction.value);
            }
            if (transaction.destination != null)
            {
                if (transaction.destination == rightVillage)
                    AddToItems(rightList, transaction.type, transaction.value);
                else
                    AddToItems(leftList, transaction.type, transaction.value);
            }
        }

        //Met les nom des village sur les text
        if (leftVillage != null)
        {
            sourceName.text = multipleLeftVillages? "Villages" : leftVillage.nom; //Si on a eux plus d'1 village a gauche, on va juste ecrire 'Villages'
            sourceName.gameObject.SetActive(true);
        }
        else sourceName.gameObject.SetActive(false);

        if (rightVillage != null)
        {
            destinationName.text = rightVillage.nom;
            destinationName.gameObject.SetActive(true);
        }
        else destinationName.gameObject.SetActive(false);


        SpawnItems(leftList, leftItemSlot, leftItemPrefab);
        SpawnItems(rightList, rightItemSlot, rightItemPrefab);

        transactionContainer.SetActive(true);
    }

    void AddToItems(List<DialogTransactionItem> list, ResourceType type, int value)
    {
        DialogTransactionItem item = GetItem(list, type);
        if (item == null)
        {
            item = new DialogTransactionItem();
            item.type = type;
            list.Add(item);
        }

        item.value += value;
    }

    void SpawnItems(List<DialogTransactionItem> list, RectTransform container, RectTransform prefab)
    {
        foreach (DialogTransactionItem item in list)
        {
            RectTransform tr = ((GameObject)Instantiate(prefab.gameObject, container.transform, false)).GetComponent<RectTransform>();
            tr.anchoredPosition = Vector2.zero;
            tr.localScale = Vector2.one;
            tr.GetComponent<Text>().text = (item.value >= 0 ? "+" : "") + item.value;
            tr.GetComponentInChildren<Image>().sprite = GameResources.GetIcon(item.type);
            container = tr.Find("Slot") as RectTransform;
        }
    }

    DialogTransactionItem GetItem(List<DialogTransactionItem> list, ResourceType type)
    {
        foreach (DialogTransactionItem item in list)
        {
            if (item.type == type) return item;
        }
        return null;
    }

    void Highlight(bool state)
    {
        text.color = state ? highlightColor : normalColor;
    }

    public void SpeedUp()
    {
        if (tweener == null || !isAnimating) return;
        tweener.Kill();
        OnCompletAnim();
        text.text = message;
    }

    public void Exit()
    {
        Destroy(gameObject);
    }

    //Events

    void OnPointerEnter()
    {
        if (highlightOnHover) Highlight(true);
    }

    void OnPointerExit()
    {
        Highlight(false);
    }

    void OnClick()
    {
        if (isAnimating)
        {
            SpeedUp();
        }
        else
        {
            if (onChoose != null)
            {
                onChoose.Invoke(this);
            }
        }
    }

    void OnCompletAnim()
    {
        isAnimating = false;
    }
}
