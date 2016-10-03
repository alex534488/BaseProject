using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Civil : Personnage
{
    public GameObject sprite;
    public Vector2 mapSize;
    private Vector3 fleetarget;

    public UnityEvent onZombieNearby;

    // Use this for initialization
    void Start()
    {
        // Variables
        damage = 0;
        hp = 5;
        movementSpeed = 1;

        // Set Initial Behaviors
        comportement.ChangeState<StatesIdle>();

        //Setup detector
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
            detector.onEnemyEnter.AddListener(OnZombieNearby);
        }
    }

    void Update()
    {
        if(comportement.currentStates.target != null)
        {
            // calculer la position la plus propice a une fuite
            // et l'enregistrer dans fleetarget
        }
    }

    void OnZombieNearby(Personnage personnage)
    {
        comportement.ChangeState<StatesMoveTo>();
        (comportement.currentStates as StatesMoveTo).Init(fleetarget);
    }
}
