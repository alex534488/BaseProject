using UnityEngine;
using System.Collections;

public class ToggleGameObject : MonoBehaviour {

    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
