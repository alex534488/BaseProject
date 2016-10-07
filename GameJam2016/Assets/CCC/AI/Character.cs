using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Character : Personnage
{
    // Attribut
    // ex: public float aurasize;

    // Evennements
    // ex: public UnityEvent onEnemyNearby;

    void Start()
    {
        // Set Initial Behaviors
        ChangeState<StatesIdle>();

        // Setup Entity detector
        /* 2D Detector Version
        if (detector != null)
        {
            System.Type[] enemies =
            {
                typeof(Zombie)
            };
            System.Type[] allies =
            {
                //On ne met aucun allié parce que les humans (policier / civile) n'intéragisse pas ensemble pour l'instant
            };
            detector.Init(enemies, true, allies, false);
            detector.onEnemyEnter.AddListener(OnEnemyNearby);
        }
        */

        // Setup Events
    }

    protected override void Update()
    {
        base.Update();
    }

    ///////////////////////

    // Ajouter fonctions specifique aux attributs ou aux character
}
