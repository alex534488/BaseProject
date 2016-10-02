using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class School : MonoBehaviour
{
    [Header("Timers")]
    public float minimumSpawnTimer;
    public float maximumSpawnTimer;

    private float activeSpawnTimer;            // Timer utilise pour gerer le spawn

    private List<Civil> listeCivil = new List<Civil>();
    private List<Policier> listePolicier = new List<Policier>();

    // Use this for initialization
    void Start ()
    {
        activeSpawnTimer = Random.Range(minimumSpawnTimer, maximumSpawnTimer);
	}
	
	// Update is called once per frame
	void Update ()
    {
        activeSpawnTimer = activeSpawnTimer - Time.deltaTime;

        if (activeSpawnTimer <= 0)
        {
            SpawnCivil();
            SpawnPolicier();
        }
	}

    void SpawnCivil()
    {
        Civil unCivil = (GetComponentInChildren<SpawnPoint>().SpawnObject("Civil")).GetComponent<Civil>();
        unCivil.GetComponent<Comportement>().ChangeState<StatesMoveTo>();

        // Ajouter fonction aleatoire
        Vector3 awayPosition = new Vector3(unCivil.mapSize.x, 0, unCivil.mapSize.y);

        (unCivil.GetComponent<Comportement>().currentStates as StatesMoveTo).Init(awayPosition);
        listeCivil.Add(unCivil);
    }

    void SpawnPolicier()
    {
        Policier unPolicier = (GetComponentInChildren<SpawnPoint>().SpawnObject("Policier")).GetComponent<Policier>();
    }
}
