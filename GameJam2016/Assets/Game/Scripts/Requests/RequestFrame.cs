using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using Game.Characters;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Request/Request Frame")]
public class RequestFrame : ScriptableObject
{
    public int customFrame = -1;
    public string tag = "exemple_village_need_gold";
    public string text = "Entrez une message...";
    public char forceSeparation = '%';
    public Resource_Type type = Resource_Type.custom;
    public List<Choice> choices = new List<Choice>(3);
    public Condition condition = null;
    public ScriptableObject characterKit = null;

    Village source = null; //Demandant
    Village destination = null; //Demandé
    //  ex: village demande de la nourriture à la capitale.
    //      source = village
    //      destination = capitale
    //  ex: évenement random où une inondation ravage la capitale.
    //      source = null
    //      destination = capitale

    public Request Build(Village source, Village destination, int value = 1, Resource_Type type = Resource_Type.custom, UnityAction[] callbacks = null)
    {
        this.source = source;
        this.destination = destination;
        this.type = type;

        string textTemp = Filter(text, source, destination, value);  //Filtre le text

        //Liste identique a la liste original, mais avec les textes filtré.
        //On doit faire une copie, sinon le text filtré se retrouve directement dans la request frame.
        List<Choice> choicesTempo = null;

        if (choices != null && choices.Count > 0)
        {
            //Creer la liste
            choicesTempo = new List<Choice>(choices.Count);
            int currentChoice = 0;

            foreach (Choice choice in choices)
            {
                //Fill les transactions
                if (choice.transactions != null)
                    foreach (Transaction transaction in choice.transactions)
                    {
                        int newValue = Mathf.RoundToInt(float.Parse(Filter(transaction.fillValue, source, destination, value)));
                        transaction.Fill(source, destination, newValue, type);
                    }

                //Determine s'il y a un callback a faire
                UnityAction callback = null;
                if (callbacks != null && callbacks.Length > currentChoice)
                    callback = callbacks[currentChoice];

                //Construit le choix
                Choice choiceTempo = new Choice(Filter(choice.text, source, destination, value),
                delegate ()
                {
                    if (callback != null) callback();
                    if (choice.customCallBack != null) choice.customCallBack();
                }, choice.transactions);

                //Ajoute la copie de choix a la liste temporaire
                choicesTempo.Add(choiceTempo);
                currentChoice++;
            }
        }

        //Build le message et la request
        Dialog.Message messageTempo = new Dialog.Message(textTemp, forceSeparation);
        Request request = new Request(messageTempo, choicesTempo);

        if (characterKit != null && characterKit is IKitMaker)
            request.SetCharacterKit((characterKit as IKitMaker).MakeKit());

        request.condition = condition;

        return request;
    }

    string Filter(string text, Village source, Village destination, int value)
    {
        text = FilterWords(text, source, destination, value);
        text = FilterOperations(text);
        text = RemoveBrackets(text);
        return text;
    }

    string FilterWords(string text, Village source, Village destination, int value)
    {
        int lastIndex = 0;
        try
        {
            while (true)
            {
                if (lastIndex >= text.Length - 3) break;

                int index = text.IndexOf('[', lastIndex);

                if (index < 0) break;

                int ending = text.IndexOf(']', index);
                string sub = text.Substring(index, ending - index + 1); // [blabbla]
                string result = ReplaceTerms(sub, source, destination, value);
                text = text.Remove(index, ending - index + 1);
                text = text.Insert(index, result);

                ending = text.IndexOf(']', index);

                lastIndex = ending + 1;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in " + name + " text formating. " + e.Message);
        }
        return text;
    }
    string FilterOperations(string text)
    {
        char[] operators = new char[] { '+', '-', '*', '/', '^', '%' };

        int lastIndex = 0;
        try
        {
            while (true)
            {
                if (lastIndex >= text.Length - 3) break;

                int index = text.IndexOf('[', lastIndex);
                if (index < 0) break;

                int ending = text.IndexOf(']', index);

                int start = index + 1;
                int end = ending - 1;

                int operatorIndex = text.IndexOfAny(operators, start, (end - start) + 1);
                if (operatorIndex == -1) //Pas d'operation dans cette section
                {
                    lastIndex = ending + 1;
                    continue;
                }

                string numberAString = text.Substring(start, operatorIndex - start);

                int nextOperatorIndex = text.IndexOfAny(operators, operatorIndex + 1, end - operatorIndex);
                if (nextOperatorIndex == -1) nextOperatorIndex = end + 1;

                string numberBString = text.Substring(operatorIndex + 1, nextOperatorIndex - (operatorIndex + 1));

                float numberA = float.Parse(numberAString);
                float numberB = float.Parse(numberBString);
                char op = text[operatorIndex];

                float result = (float)((int)(Operate(numberA, numberB, op) * 100)) / 100f;

                text = text.Remove(start, nextOperatorIndex - start);
                text = text.Insert(start, result.ToString());
            }
            lastIndex = 0;
            while (true)
            {
                if (lastIndex >= text.Length - 3) break;

                int index = text.IndexOf('[', lastIndex);
                if (index < 0) break;

                int ending = text.IndexOf(']', index);

                int start = index + 1;
                int end = ending - 1;

                string numberAString = text.Substring(start, (end - start) + 1);

                float numberA;
                if (!float.TryParse(numberAString, out numberA))
                {
                    lastIndex = ending + 1;
                    continue;
                }

                int result = Mathf.RoundToInt(numberA);

                text = text.Remove(start, (end - start) + 1);
                text = text.Insert(start, result.ToString());
                lastIndex = ending + 1;
            }
        }
        catch
        {
            Debug.LogError("Error in " + name + " text formating.");
        }
        return text;
    }
    string RemoveBrackets(string text)
    {
        while (true)
        {
            int index = text.IndexOf('[');
            if (index == -1) break;
            text = text.Remove(index, 1);
        }
        while (true)
        {
            int index = text.IndexOf(']');
            if (index == -1) break;
            text = text.Remove(index, 1);
        }
        return text;
    }

    string ReplaceTerms(string text, Village source, Village destination, int value)
    {
        text = text.Replace("source.name", source == null ? "" : source.nom);
        text = text.Replace("source.food", source == null ? "" : source.GetFood().ToString());
        text = text.Replace("source.foodProd", source == null ? "" : source.GetFoodProd().ToString());
        text = text.Replace("source.army", source == null ? "" : source.GetArmy().ToString());
        text = text.Replace("source.armyProd", source == null ? "" : source.GetArmyProd().ToString());
        text = text.Replace("source.gold", source == null ? "" : source.GetGold().ToString());
        text = text.Replace("source.goldProd", source == null ? "" : source.GetGoldProd().ToString());

        text = text.Replace("destination.name", destination == null ? "" : destination.nom);
        text = text.Replace("destination.food", destination == null ? "" : destination.GetFood().ToString());
        text = text.Replace("destination.foodProd", destination == null ? "" : destination.GetFoodProd().ToString());
        text = text.Replace("destination.army", destination == null ? "" : destination.GetArmy().ToString());
        text = text.Replace("destination.armyProd", destination == null ? "" : destination.GetArmyProd().ToString());
        text = text.Replace("destination.gold", destination == null ? "" : destination.GetGold().ToString());
        text = text.Replace("destination.goldProd", destination == null ? "" : destination.GetGoldProd().ToString());
        text = text.Replace("type", GameResources.ToString(type));

        text = text.Replace("value", value.ToString());

        return text;
    }
    float Operate(float a, float b, char op)
    {
        switch (op)
        {
            default:
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                return a / b;
            case '^':
                return Mathf.Pow(a, b);
            case '%':
                return a % b;
        }
    }

    public void CustomFrames(int index)
    {
        bool indexExists = true;
        switch (index)
        {
            default:
                indexExists = false;
                break;
            case -1:
                condition = null;
                if (choices != null && choices.Count > 0)
                {
                    foreach (Choice choice in choices)
                    {
                        if (choice.transactions != null && choice.transactions.Count > 0)
                            foreach (Transaction transac in choice.transactions)
                                transac.condition = null;
                    }
                }
                break;
            case 0:
                {
                    //La requete ne se fait que si la destination a au moins 11 soldat
                    condition = new Condition(delegate
                    {
                        return destination.GetArmy() > 10;
                    });

                    tag = "exemple_village_need_food";
                    text = "Je suis un messager venant du village de [source.name].\n\nNos citoyen sont amateur de PFK. Nous désirons donc acheter votre poule.";

                    List<Transaction> choixUnTrans = new List<Transaction>();                           //Transaction du choix 1
                    choixUnTrans.Add(new Transaction(Transaction.Id.source, Transaction.Id.destination, Resource_Type.gold, 10));       //Le village donne de l'or a la capital
                    choixUnTrans.Add(new Transaction(Transaction.Id.Null, Transaction.Id.source, Resource_Type.food, 1));                //Le village gagne 1 de food

                    List<Transaction> choixDeuxTrans = new List<Transaction>();                      //Transaction du choix 2
                    choixDeuxTrans.Add(new Transaction(Transaction.Id.Null, Transaction.Id.destination, Resource_Type.happiness, 2));    //La capital gagne 2 de bonheur

                    List<Transaction> choixTroisTrans = null;                                           //Transaction du choix 3 (aucune)

                    choices = new List<Choice>(3);
                    choices.Add(                                                                                                          //Premier choix
                        new Choice(
                            "Premier choix: Vendre la poule",                                                                                   //Message
                            delegate { Debug.Log("Ceci est un custom callback, utile quand on veut faire des actions unique custom. "); },       //Custom callback
                            choixUnTrans                                                                                                        //Transactions
                            )
                        );
                    choices.Add(                                                                                                          //Deuxieme choix
                        new Choice(
                            "Deuxieme choix: Garder la poule et l'engager dans un cirque.",                                                     //Message
                            null,                                                                                                               //Custom callback
                            choixDeuxTrans                                                                                                      //Transactions
                            )
                        );
                    choices.Add(                                                                                                          //Troisieme choix
                        new Choice(
                            "Troisieme choix: Regarder le mur.",                                                                                //Message
                            null,                                                                                                               //Custom callback
                            choixTroisTrans                                                                                                     //Transactions
                            )
                        );
                    break;
                }
        }
        if (indexExists)
            customFrame = index;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RequestFrame))]
public class RequestFrameEditor : CCC.EditorUtil.AdvEditor
{
    int customFrameIndex = -2; // -2 = non-initialisé
    bool textTips = false;
    bool characterKitFold = false;
    RequestFrame frame;

    GUIStyle wrapTextArea = new GUIStyle(EditorStyles.textField);

    bool CanEdit() { return frame.customFrame == -1; }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnInspectorGUI()
    {
        wrapTextArea = new GUIStyle(EditorStyles.textField);
        wrapTextArea.wordWrap = true;

        frame = target as RequestFrame;

        //DrawHeader();

        DrawTextTips();

        DrawCustomFrameIndex(frame);

        GUI.enabled = CanEdit();

        DrawTag(frame);

        DrawCharacterKit();

        DrawText(frame);

        DrawChoices(frame);

        GUI.enabled = true;

        EditorUtility.SetDirty(target);
    }

    #region Header

    void DrawCustomFrameIndex(RequestFrame frame)
    {
        if (customFrameIndex == -2) customFrameIndex = frame.customFrame;

        EditorGUILayout.LabelField("Custom Frame", bold);
        GUILayout.BeginHorizontal();

        customFrameIndex = EditorGUILayout.IntField("(none: -1)", customFrameIndex);

        //si il n'y a aucune changement, on disable le bouton 'apply'
        if (customFrameIndex == frame.customFrame) GUI.enabled = false;

        if (GUILayout.Button("Apply")) frame.CustomFrames(customFrameIndex);
        if (GUILayout.Button("Revert")) customFrameIndex = frame.customFrame;

        GUI.enabled = true;

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (!CanEdit())
        {
            EditorGUILayout.LabelField("** AUCUNE MODIFICATIONS PERMISES **");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

    }

    void DrawTag(RequestFrame frame)
    {
        EditorGUILayout.LabelField("Tag", bold);

        frame.tag = EditorGUILayout.TextField(frame.tag);

        EditorGUILayout.Space();
    }

    void DrawTextTips()
    {
        textTips = EditorGUILayout.Foldout(textTips, "Text tips");

        if (textTips)
        {
            EditorGUILayout.LabelField("- Utilisez '%' pour forcer le splitting du texte sur une autre page.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Les 3 variables de la requète sont 'source', 'destination' et 'value'.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("   La 'source' de la request représente la ville qui demande la question.");
            EditorGUILayout.LabelField("   La 'destination' est l'autre ville.");
            EditorGUILayout.LabelField("   Une des deux peut être null dans le cas où une seul ville est concernée.");
            EditorGUILayout.LabelField("   La 'value' est la valeur passée. Elle n'est pas oubligé d'être utilisée.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Utilisez '[source.property]' ou '[destination.property]' pour utiliser les");
            EditorGUILayout.LabelField("   propriétées des ville dans le texte.");
            EditorGUILayout.LabelField("     ex: Le village de [source.name] aimerais avoir quelques de vos ");
            EditorGUILayout.LabelField("          [destination.army] soldats");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Utilisez '[value]' ou '[value OPERATOR number]'  pour référer à la valeur");
            EditorGUILayout.LabelField("   passée.");
            EditorGUILayout.LabelField("     ex: - Donner [value*1.15] soldats au village.");
            EditorGUILayout.LabelField("          - Donner [value] soldats au village.");
            EditorGUILayout.LabelField("          - Donner [value-10] soldats au village.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Utilisez '[type]' pour référer au Resource_type de la requête.");
            EditorGUILayout.LabelField("   passée.");
            EditorGUILayout.LabelField("     ex: - Notre ville a besoin de [type].");
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    void DrawText(RequestFrame frame)
    {
        EditorGUILayout.LabelField("Request Text", bold);

        //frame.text = GUILayout.TextArea(frame.text);
        frame.text = EditorGUILayout.TextArea(frame.text, wrapTextArea);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    void DrawCharacterKit()
    {
        EditorGUILayout.LabelField("Character Kit", bold);

        if (frame.characterKit != null)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(18)))
                frame.characterKit = null;
            else
                EditorGUILayout.LabelField("     " + frame.characterKit.name);
            EditorGUILayout.EndHorizontal();
        }
        characterKitFold = EditorGUILayout.Foldout(characterKitFold, "Set");
        if (characterKitFold)
        {
            ScriptableObject temp = null;
            temp = EditorGUILayout.ObjectField(null, typeof(CharacterGenerator), false) as ScriptableObject;
            if (temp == null)
                temp = EditorGUILayout.ObjectField(null, typeof(CustomKit), false) as ScriptableObject;
            if (temp != null)
                frame.characterKit = temp;
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    #endregion

    #region Choices
    void DrawChoices(RequestFrame frame)
    {
        GUILayout.Label("Choix", bold);

        if (frame.choices == null || frame.choices.Count <= 0)
        {
            GUILayout.Label("Aucun choix pour l'instant", centered);
        }
        //
        int index = 0;
        if (frame.choices != null)
            foreach (Choice choix in frame.choices)
            {
                if (!DrawChoice(choix, index)) break;
                EditorGUILayout.Space();
                index++;
            }


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
        if (GUILayout.Button("Ajouter un choix"))
        {
            if (frame.choices == null) frame.choices = new List<Choice>();
            frame.choices.Add(new Choice());
        }
        EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }

    bool DrawChoice(Choice choix, int index)
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label((index + 1) + "------------------------------", bold);

        if (GUILayout.Button("^", GUILayout.Width(30)))
        {
            if (index > 0)
            {
                Choice temp = frame.choices[index - 1];
                frame.choices[index - 1] = choix;
                frame.choices[index] = temp;
            }
            return false;
        }
        if (GUILayout.Button("v", GUILayout.Width(30)))
        {
            if (index < frame.choices.Count - 1)
            {
                Choice temp = frame.choices[index + 1];
                frame.choices[index + 1] = choix;
                frame.choices[index] = temp;
            }
            return false;
        }
        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            frame.choices.Remove(choix);
            return false;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Text du choix");

        choix.text = EditorGUILayout.TextArea(choix.text, wrapTextArea);


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Transactions");
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            if (choix.transactions == null) choix.transactions = new List<Transaction>();
            choix.transactions.Add(new Transaction());
        }
        EditorGUILayout.EndHorizontal();

        if (choix.transactions != null)
            foreach (Transaction transaction in choix.transactions)
            {
                if (!DrawTransaction(transaction))
                {
                    choix.transactions.Remove(transaction);
                    break;
                }
            }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        return true;
    }

    bool DrawTransaction(Transaction transaction)
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("x", GUILayout.Width(20))) return false;

        EditorGUILayout.Space();
        transaction.fillValue = EditorGUILayout.TextField(transaction.fillValue);
        EditorGUILayout.Space();
        transaction.type = (Resource_Type)EditorGUILayout.EnumPopup(transaction.type);
        transaction.valueType = (Transaction.ValueType)EditorGUILayout.EnumPopup(transaction.valueType);
        EditorGUILayout.Space();
        GUILayout.Label("de");
        EditorGUILayout.Space();
        transaction.fromId = (Transaction.Id)EditorGUILayout.EnumPopup(transaction.fromId);
        EditorGUILayout.Space();
        GUILayout.Label("à");
        EditorGUILayout.Space();
        transaction.toId = (Transaction.Id)EditorGUILayout.EnumPopup(transaction.toId);
        EditorGUILayout.EndHorizontal();
        return true;
    }

    #endregion
}
#endif