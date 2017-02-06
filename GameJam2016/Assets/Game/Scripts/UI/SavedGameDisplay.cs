using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class SavedGameDisplay : MonoBehaviour {

    private List<GameSave> listGameSaves = new List<GameSave>();
    public GameSave currentGameSave;
    public GameObject container;
    public GameObject buttonPrefab;
    public ExitLoadMenu menu;
    public UnityEvent otherButtonClick;

    void Start () {
        menu.OnSave.AddListener(Refresh);
        Refresh();
    }

    public void OnGameSaveClick(GameObject gameSave)
    {
        otherButtonClick.Invoke();
        currentGameSave = listGameSaves[int.Parse(gameSave.name)];
        menu.SetCurrentGameSave(currentGameSave);
    }

    private void Refresh()
    {
        Clear();
        Display();
    }

    private void Display()
    {
        // Allons chercher toutes les gamesaves
        List<string> listGameSavesName = GameManager.GetGameSaves();
        foreach (string str in listGameSavesName)
        {
            listGameSaves.Add(GameSave.Load(str));
        }
        // Ensuite on les display
        for (int i = 0; i < listGameSaves.Count; i++)
        {
            GameObject newGameSaveButton = Instantiate(buttonPrefab);
            newGameSaveButton.name = "" + i;
            newGameSaveButton.transform.GetComponentInChildren<Text>().text = "Gamesave : " + i + " | Day " + listGameSaves[i].currentWorld.CurrentDay;
            newGameSaveButton.transform.SetParent(container.transform);
        }
    }

    private void Clear()
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
