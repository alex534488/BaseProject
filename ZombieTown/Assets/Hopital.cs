using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Hopital : MonoBehaviour {

    [Header("Timer")]
    public float spawnTimer;                   // En secondes
    public float levelUpTimer;                 // En secondes
    public float defeatTimer;                  // En secondes
    public float cooldownTimer;                // En secondes
    public float noMoreAttackTimer;            // En secondes

    private List<Transform> listeMurs = new List<Transform>();         // Liste de la position des differents murs
    private List<Policier> listePoliciers = new List<Policier>();     // Liste des policiers
    

    private float activeSpawnTimer;            // Timer utilise pour gerer le spawn
    private float activeLevelUpTimer;          // Timer utilise pour gerer le level up du batiment
    private float activeDefeatTimer;           // Timer qui s'active lorsque un hopital atteint le niveau 5
    private float activeCooldownTimer;
    private float activeAttackTimer;

    private bool zoneControle;                 // True si les policiers controlent la zone
    private bool zoneAttaque;                  // True si la zone est presentement attaque par le joueur
    private bool rechercheRemede;              // True si l'hopital est niveau 5 et recherche actuellement un remede

    private int currentLevel;                  // Le level actuel de l'hopital

    void Start()
    {
        PositionMurs();
        InitializeTimer();
        ResetBool();
        SpawnPolicier();
    }

    void Update()
    {
        UpdateTimer();
        CheckTimer();
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

    void InitializeTimer()
    {
        activeSpawnTimer = spawnTimer;
        activeLevelUpTimer = levelUpTimer;
    }

    void ResetBool()
    {
        zoneControle = true;
        zoneAttaque = false;
        rechercheRemede = false;
    }

    void CheckTimer()
    {
        if (zoneAttaque == true)
        {
            if (activeAttackTimer <= 0)
                zoneAttaque = false;
        }

        else if (zoneControle == true)
        {
            if (activeSpawnTimer <= 0)
            {
                SpawnPolicier();
                activeSpawnTimer = spawnTimer;
            }

            if (activeLevelUpTimer <= 0)
            {
                LevelUp();
                ChangeSprite();
                Warning();
                activeLevelUpTimer = levelUpTimer;
            }
        }

        else if (zoneControle == false)
        {
            if (activeCooldownTimer <= 0)
            {
                zoneControle = true;
                InitializeTimer();
                SpawnWalls();
                SpawnPolicier();
            }

        }
    }

    void UpdateTimer()
    {
        activeSpawnTimer = activeSpawnTimer - Time.deltaTime;

        if (currentLevel != 5)
            activeLevelUpTimer = activeLevelUpTimer - Time.deltaTime;

        if (rechercheRemede == true)
        {
            activeDefeatTimer = activeDefeatTimer - Time.deltaTime;
            CheckDefeat();
        }

        if (zoneAttaque == true)
        {
            activeAttackTimer = activeAttackTimer - Time.deltaTime;
        }

        if (zoneControle == false)
        {
            activeCooldownTimer = activeCooldownTimer - Time.deltaTime;
            
            if (activeCooldownTimer <= 0)
            {
                zoneControle = true;

                for (int i = 0; i < listeMurs.Count; i++)
                {
                    listeMurs[i].gameObject.SetActive(true);          // Remet en place tous les murs
                }
            }
        }
    }

    void CheckDefeat() // TO DO : Animation de defaite
    {
        if (activeDefeatTimer <= 0)
        {
            if (zoneAttaque == false)
            {
                print("Defaite ! Un hôpital a réussi à développer un remède contre le virus ");
                // Le joueur perd la partie puisque la zone n'est pas présentement attaqué par le joueur
            }
        }
    }

    void LevelUp()
    {
        currentLevel = currentLevel + 1;

        if (currentLevel == 5)
            rechercheRemede = true;

        for (int i = 0; i < listeMurs.Count; i++)
        {
            listeMurs[i].GetComponent<Walls>().GainHP(currentLevel);
        }
        
    }

    void ChangeSprite() // TO DO : Sprite du level display sur la croix de l'hopital 
    {
        // Modifie le sprite en fonction du niveau (Display le level sur la croix)
    }

    void Warning()
    {
        if (currentLevel >= 3)
        {
            if (currentLevel == 5)
            {
                print("Un hôpital vient d'atteindre le niveau 5 et recherche un remède contre le virus (Temps estimé : " + defeatTimer + " secondes)");
                activeDefeatTimer = defeatTimer;
            }

            else
                print("Un Hopital vient d'atteindre le niveau " + currentLevel);
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

    void CheckControle() // TO DO : Ajouter audio de demolition
    {
        if (listePoliciers.Count == 0)
        {
            for (int i = 0; i < listeMurs.Count; i++)
            {
                listeMurs[i].gameObject.SetActive(false); // Demoli les murs restants
                // Play audio destruction
            }

            zoneControle = false;
            activeCooldownTimer = cooldownTimer;
            currentLevel = 0;
        }
    }
}

