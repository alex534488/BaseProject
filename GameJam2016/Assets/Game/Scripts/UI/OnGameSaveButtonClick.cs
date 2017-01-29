using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnGameSaveButtonClick : MonoBehaviour {

    Color highlightColor = Color.red;
    Color startingColor;
    public GameObject gameDisplay;
    public bool currentClick;

    void Start()
    {
        currentClick = false;
        gameDisplay = GameObject.Find("LoadGamePanel");
        startingColor = transform.gameObject.GetComponent<Button>().image.color;
        gameDisplay.GetComponent<SavedGameDisplay>().otherButtonClick.AddListener(ChangeSelection);
    }

	public void OnClick()
    {
        currentClick = true;
        transform.gameObject.GetComponent<Button>().image.color = highlightColor;
        gameDisplay.GetComponent<SavedGameDisplay>().OnGameSaveClick(transform.gameObject);
    }

    private void ChangeSelection()
    {
        if(!currentClick) transform.gameObject.GetComponent<Button>().image.color = startingColor;
        currentClick = false;
    }
}
