using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject policierPrefab;
    public GameObject zombiePrefab;
    public GameObject civilPrefab;
    private GameObject erreurPrefab;

    private Vector3 spawnpointPosition3D;

    private Vector3 spawnScale;
    private float spawnIntervalle_X;
    private float spawnIntervalle_Z;

    private Vector2 spawn_X;
    private Vector2 spawn_Z;

    void Start()
    {
        spawnpointPosition3D = GetComponent<Transform>().position;

        spawnScale = GetComponent<Transform>().localScale * 10;

        spawnIntervalle_X = (spawnScale.x / 2);
        spawnIntervalle_Z= (spawnScale.z / 2);
    }

   public GameObject SpawnObject(string typePersonnage)
    {
        switch (typePersonnage)
        {
            case "Policier":
                {
                    return RandomSpawn(policierPrefab);
                }

            case "Zombie":
                {
                    return RandomSpawn(zombiePrefab);
                }

            case "Civil":
                {
                    return RandomSpawn(civilPrefab);
                }

            default:
                return RandomSpawn(erreurPrefab);
        }
    }

    GameObject RandomSpawn(GameObject prefab)
    {
        Vector3 randomPosition = new Vector3(Random.Range( (-spawnIntervalle_X), (spawnIntervalle_X)), 0f, Random.Range((-spawnIntervalle_Z), (spawnIntervalle_Z)));
        randomPosition = randomPosition + spawnpointPosition3D;

        return Instantiate(prefab.gameObject, randomPosition, Quaternion.identity) as GameObject;
    }

	
}
