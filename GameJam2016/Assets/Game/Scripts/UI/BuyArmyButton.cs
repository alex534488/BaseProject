﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CCC.Manager;

[RequireComponent(typeof(Button))]
public class BuyArmyButton : MonoBehaviour {

    public AudioClip buyClip;
    public AudioClip errorClip;
    public AudioClip sellClip;
    public bool sellButton = false;

	void Awake () {
        GetComponent<Button>().onClick.AddListener(OnClick);
	}

    void OnClick()
    {
        if (sellButton)
        {
            Empire.instance.capitale.DecreaseArmy(1);
            SoundManager.main.Play(sellClip);
        }
        else
        {
            bool result = Empire.instance.capitale.BuyArmy(1);

            if (result)
            {
                SoundManager.main.Play(buyClip);
            }
            else
            {
                SoundManager.main.Play(sellClip);
            }
        }
    }
}
