using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetBestScore : MonoBehaviour {

    public Text affichage;
    private string text = "Best Score: ";

	void Start () {
        text += PlayerPrefs.GetInt("highscore", 0);
        affichage.text = text;
	}
}
