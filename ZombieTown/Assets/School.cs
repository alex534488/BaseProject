using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class School : MonoBehaviour
{
    [Header("Timers")]
    public float minimumSpawnTimer;
    public float maximumSpawnTimer;

    [Header("Spawn")]
    public int nombrePoliciers;
    public int nombreCivils;

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
            SpawnEntities();
        }
	}

    void SpawnEntities()
    {
        Civil unCivil = (GetComponentInChildren<SpawnPoint>().SpawnObject("Civil")).GetComponent<Civil>();
        Vector3 awayPosition = RandomPosition(unCivil);
        listeCivil.Add(unCivil);

        for (int i=0; i < nombreCivils - 1; i++)
        {
            unCivil = (GetComponentInChildren<SpawnPoint>().SpawnObject("Civil")).GetComponent<Civil>();
            listeCivil.Add(unCivil);
        }

        foreach (Civil civil in listeCivil)
        {
            civil.GetComponent<Comportement>().ChangeState<StatesMoveTo>();
            (civil.GetComponent<Comportement>().currentStates as StatesMoveTo).Init(awayPosition);
        }

        for (int i=0; i < nombrePoliciers; i++)
        {
            Policier unPolicier = (GetComponentInChildren<SpawnPoint>().SpawnObject("Policier")).GetComponent<Policier>();
            unPolicier.GetComponent<Comportement>().ChangeState<StatesFollow>();

            int j = i;

            if (j >= nombreCivils)
                j = 0;
      
            (unPolicier.GetComponent<Comportement>().currentStates as StatesFollow).Init(listeCivil[j]);
        }
    }

    Vector3 RandomPosition(Civil unCivil)
    {
        float max_x = unCivil.mapSize.x;
        float max_y = 0;
        float max_z = unCivil.mapSize.y;

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            Vector3 randomPosition = new Vector3(max_x, 0, Random.Range(-max_y, max_y));
            return randomPosition;
        }

        else
        {
            Vector3 randomPosition = new Vector3(Random.Range(-max_x, max_x), 0, max_y);
            return randomPosition;
        }
    }
}
