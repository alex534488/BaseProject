using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class DialogIcon
{
    public string gold = "[g]";
    public string soldier = "[s]";
    public string food = "[f]";
    public string happiness = "[h]";
}

public class Dialog : MonoBehaviour
{
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
    public static void DisplayText(List<string> messageComplet, List<Choix> listeChoix = null, UnityAction dialogComplete = null)
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
        master.MasterDisplayText(messageComplet, listeChoix);
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

    public void Test()
    {
        List<string> message = new List<string>();
        message.Add("Hello, je suis une délicieuse poutine !");
        message.Add("NAK NAK");
        message.Add("this is da end!");

        List<Choix> choix = new List<Choix>();
        choix.Add(new Choix("-Mourir", delegate () { print("choix 1"); }));
        choix.Add(new Choix("-Vivre", delegate () { print("choix 2"); }));
        choix.Add(new Choix("-Flatter un chat", delegate () { print("choix 3"); }));

        DisplayText(message, choix);
    }

    void Awake()
    {
        if (master == null) master = this;
    }

    public void MasterDisplayText(List<string> messageComplet, List<Choix> listeChoix = null, UnityAction dialogComplete = null)
    {
        if (isInDialog) return;

        isInDialog = true;

        queue = new List<object>();
        currentDialogTexts = new List<DialogText>();

        currentDialogBox = Instantiate(dialogBoxPrefab.gameObject).GetComponent<DialogBox>();
        currentDialogBox.transform.SetParent(transform, false);
        currentDialogBox.button.onClick.AddListener(OnBoxClick);
        //currentDialogBox.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0) + screenBottomOffset;

        if (messageComplet != null) foreach (string message in messageComplet) queue.Add(message);
        if (listeChoix != null) foreach (Choix choix in listeChoix) queue.Add(choix);

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
        else //Display tous les choix
        {
            foreach(object choixObj in queue)
            {
                Choix choix = choixObj as Choix;
                DialogText text = Instantiate(dialogTextPrefab.gameObject).GetComponent<DialogText>();
                currentDialogTexts.Add(text);

                text.transform.SetParent(currentDialogBox.transform, false);
                text.highlightOnHover = true;
                text.onChoose.AddListener(delegate (DialogText item)
                {
                    if(choix.callback != null) choix.callback.Invoke();
                    Quit();
                });
                text.Init(choix.text, false);
            }
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
