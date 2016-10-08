using UnityEngine;
using System.Collections;

public class EscapeKill : MonoBehaviour {
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
}
