using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class BaseMilitaire : MonoBehaviour {

    private List<Transform> listeMurs = new List<Transform>();          // Liste de la position des differents murs
    private List<Transform> listeTurrets = new List<Transform>();      // Liste des turrets
    private List<Policier> listePoliciers = new List<Policier>();     // Liste des policiers

    private bool zoneControle;                 // True si la zone n'est pas attaque ou n'a pas perdu une bataille récemment
    private bool zoneAttaque;                 // True si la zone est presentement attaque par le joueur

    public float noMoreAttackTimer;

    private float activeCooldownTimer;
    private float activeAttackTimer;

    public float cooldownTimer;

    void Start ()
    {
        PositionMurs();
        PositionTurrets();
	}
	
	void Update ()
    {
        UpdateTimer();
        CheckTimer();
    }

    void UpdateTimer()
    {
        if (zoneAttaque == true)
            activeAttackTimer = activeAttackTimer - Time.deltaTime;

        if (zoneControle == false)
            activeCooldownTimer = activeCooldownTimer - Time.deltaTime;
    }

    void CheckTimer()
    {
        if (zoneAttaque == true)
        {
            if (activeAttackTimer <= 0)
                zoneAttaque = false;
        }

        if (zoneControle == false)
        {
            if (activeCooldownTimer <= 0)
            {
                zoneControle = true;
                SpawnWalls();
                SpawnTurrets();
                SpawnPolicier();
            }
        }
    }

    void PositionMurs()
    {
        Transform unEnfant;

        for (int i = 0; i < GetComponent<Transform>().childCount; i++)
        {
            unEnfant = GetComponent<Transform>().GetChild(i);

            if (unEnfant.tag == "Wall")
                listeMurs.Add(unEnfant);
        }
    }

    void PositionTurrets()
    {
        Transform unEnfant;

        for (int i = 0; i < GetComponent<Transform>().childCount; i++)
        {
            unEnfant = GetComponent<Transform>().GetChild(i);

            if (unEnfant.tag == "Turret")
                listeTurrets.Add(unEnfant);
        }
    }

    void MortPolicier(Personnage unePersonne)
    {
        Policier unPolicier = unePersonne as Policier;
        listePoliciers.Remove(unPolicier);

        zoneAttaque = true;
        activeAttackTimer = noMoreAttackTimer;

        CheckControle(); // Permet de verifier si les policiers ont perdu le controle de la zone
    }

    void CheckControle() // TO DO : Play audio destruction
    {
        if (listePoliciers.Count == 0) // Si tous les policiers sont morts
        {
            if (listeTurrets.Count == 0) // Si toutes les turrets sont demolies
            {
                for (int i = 0; i < listeMurs.Count; i++)
                {
                    listeMurs[i].gameObject.SetActive(false); // Demoli les murs restants
                    // Play audio destruction
                }
                zoneControle = false;
                activeCooldownTimer = cooldownTimer;
            }
        }
    }

    void SpawnPolicier()
    {
        Policier unPolicier = (GetComponentInChildren<SpawnPoint>().SpawnObject("Policier")).GetComponent<Policier>();
        unPolicier.onDeath.AddListener(MortPolicier);
    }

    void SpawnWalls()
    {
        for (int i = 0; i < listeMurs.Count; i++)
        {
            listeMurs[i].gameObject.SetActive(true);
        }
    }

    void SpawnTurrets()
    {
        for (int i = 0; i < listeTurrets.Count; i++)
        {
            listeTurrets[i].gameObject.SetActive(true);
        }
    }
}
