using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
    int lastUpdate;
	// Use this for initialization
	void Start () {
        lastUpdate = (int)(Time.time);
    }
	
	// Update is called once per frame
	void Update () {
        
        if((int)(Time.time)%1==0 && (int)(Time.time) != lastUpdate)
        {
            Color col = GameObject.Find("Rouge").GetComponent<Image>().color;
            col.a += 0.002f;
            col.r -= 0.002f;
            GameObject.Find("Rouge").GetComponent<Image>().color = col;





        }
        

    }

    public void playAgain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
