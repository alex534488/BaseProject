using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class CanvasMain : MonoBehaviour {

    public static Canvas main;

    void Awake()
    {
        main = GetComponent<Canvas>();
    }
}
