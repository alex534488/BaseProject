using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using CCC.Utility;

public class DialogIcon
{
    public string gold = "[g]";
    public string soldier = "[s]";
    public string food = "[f]";
    public string happiness = "[h]";
}

public class Dialog : MonoBehaviour
{
    public class Message
    {
        public string text;
        public char forceSeparation;
        public Message(string text, char forceSeparation = '%')
        {
            this.text = text;
            this.forceSeparation = forceSeparation;
        }
        public Message() { }
    }
    public class Choix
    {
        public Choix(string text, UnityAction callback)
        {
            this.text = text;
            this.callback = callback;
        }
        public string text;
        public UnityAction callback;
    }

    /// <summary>
    /// Spawn la boite de text. Affiche le message puis propose les choix s'il n'en a
    /// </summary>
    public static void DisplayText(Message message, List<Choice> listeChoix = null, UnityAction dialogComplete = null)
    {
        if (IsInDialog())
        {
            Debug.LogError("Is already in dialog.");
            return;
        }
        if (master == null)
        {
            Debug.LogError("Master is null.");
            return;
        }
        master.MasterDisplayText(message, listeChoix, dialogComplete);
    }

    public static bool IsInDialog() { return master.isInDialog; }

    static Dialog master;

    public DialogText dialogTextPrefab;
    public DialogBox dialogBoxPrefab;
    public Vector3 screenBottomOffset;

    private List<DialogText> currentDialogTexts = new List<DialogText>();
    private DialogBox currentDialogBox;

    private List<object> queue = new List<object>();
    private bool isInDialog = false;

    private UnityAction dialogComplete;

    void Awake()
    {
        if (master == null) master = this;
    }

    public void MasterDisplayText(Message message, List<Choice> listeChoix = null, UnityAction dialogComplete = null)
    {
        if (isInDialog) return;

        RectOffset padder = dialogBoxPrefab.GetComponent<VerticalLayoutGroup>().padding;
        Vector2 paddingSub = new Vector2(padder.horizontal, padder.vertical);
        List<string> messageSplit = TextSplitter.Split(message.text, dialogTextPrefab.text,
            dialogBoxPrefab.GetComponent<RectTransform>().sizeDelta - paddingSub
            , message.forceSeparation);

        isInDialog = true;
        this.dialogComplete = dialogComplete;

        queue = new List<object>();
        currentDialogTexts = new List<DialogText>();

        currentDialogBox = Instantiate(dialogBoxPrefab.gameObject).GetComponent<DialogBox>();
        currentDialogBox.transform.SetParent(transform, false);
        currentDialogBox.button.onClick.AddListener(OnBoxClick);
        //currentDialogBox.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0) + screenBottomOffset;

        if (messageSplit != null) foreach (string aMessage in messageSplit) queue.Add(aMessage);
        if (listeChoix != null) foreach (Choice choix in listeChoix) queue.Add(choix);

        NextText();
    }

    private void NextText()
    {
        if (queue.Count <= 0)
        {
            Quit();
            return;
        }
        currentDialogTexts.Clear();

        object obj = queue[0];

        if (obj is string) // Display 1 message
        {
            string objString = obj as string;
            DialogText text = Instantiate(dialogTextPrefab.gameObject).GetComponent<DialogText>();
            currentDialogTexts.Add(text);

            text.transform.SetParent(currentDialogBox.transform, false);
            text.highlightOnHover = false;
            text.onChoose.AddListener(delegate (DialogText item)
            {
                item.Exit();
                EndItem();
            });
            text.Init(objString, true);
        }
        else //Display tous les choix (EST SLMT FAIT A LA FIN, sinon y a des bug a fix)
        {
            if (queue.Count > 2) currentDialogBox.SetBig(DisplayAllChoix);
            else DisplayAllChoix();

        }
    }

    void DisplayAllChoix()
    {
        foreach (object choixObj in queue)
        {
            Choice choix = choixObj as Choice;
            DialogText text = Instantiate(dialogTextPrefab.gameObject).GetComponent<DialogText>();
            currentDialogTexts.Add(text);

            text.transform.SetParent(currentDialogBox.transform, false);
            text.highlightOnHover = true;
            text.onChoose.AddListener(delegate (DialogText item)
            {
                choix.Choose();
                Quit();
            });
            text.Init(choix.text, false, choix.transactions);
        }
    }


    private void OnBoxClick()
    {
        if(currentDialogTexts.Count == 1) currentDialogTexts[0].button.OnPointerClick(null);
    }

    private void EndItem()
    {
        queue.RemoveAt(0);
        NextText();
    }

    private void Quit()
    {
        foreach (DialogText text in currentDialogTexts) text.Exit();
        currentDialogTexts.Clear();

        if(currentDialogBox != null) currentDialogBox.Exit();
        currentDialogBox = null;

        isInDialog = false;
        if (dialogComplete != null) dialogComplete.Invoke();
    }

}
