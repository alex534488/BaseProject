using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Hopital : MonoBehaviour {

    [Header("Spawn Point")]
    public Vector2 xBounds;
    public Vector2 zBounds;

    [Header("Timer")]
    public float spawnTimer;                   // En secondes
    public float levelUpTimer;                 // En secondes
    public float defeatTimer;                  // En secondes
    public float cooldownTimer;                // En secondes

    [Header("Prefab")]
    public Policier prefabPolicier;

    private List<Transform> listeMurs;         // Liste de la position des differents murs
    private List<Policier> listePoliciers;     // Liste des policiers

  


    private float activeSpawnTimer;            // Timer utilise pour gerer le spawn
    private float activeLevelUpTimer;          // Timer utilise pour gerer le level up du batiment
    private float activeDefeatTimer;           // Timer qui s'active lorsque un hopital atteint le niveau 5
    private float activeCooldownTimer;

    private bool zoneControle;                 // True si la zone n'est pas attaque ou n'a pas perdu une bataille récemment
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
        spawnTimer = spawnTimer * 1000;
        activeSpawnTimer = spawnTimer;

        levelUpTimer = levelUpTimer * 1000;
        activeLevelUpTimer = levelUpTimer;

        defeatTimer = defeatTimer * 1000;
        activeDefeatTimer = defeatTimer;

        cooldownTimer = cooldownTimer * 1000;
        activeCooldownTimer = cooldownTimer;
    }

    void ResetBool()
    {
        zoneControle = true;
        zoneAttaque = false;
        rechercheRemede = false;
    }

    void CheckTimer()
    {
        if (activeSpawnTimer <= 0)
        {
            SpawnPolicier();
        }

        if (levelUpTimer <= 0)
        {
            LevelUp();
            ChangeSprite();
            Warning();
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
            if (zoneAttaque == true)
            {
                // Le joueur perd la partie puisque la zone n'est pas présentement attaqué par le joueur
            }
        }
    }

    void LevelUp()
    {
        currentLevel = currentLevel + 1;

        if (currentLevel == 5)
            rechercheRemede = true;
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
                print("Un hôpital recherche présentement un remède contre le virus (Temps estimé : " + defeatTimer / 1000 + " secondes)");

            else
                print("Un Hopital vient d'atteindre le niveau " + currentLevel);
        }

    }

    void MortPolicier(Personnage unePersonne)
    {
        Policier unPolicier = unePersonne as Policier;
        listePoliciers.Remove(unPolicier);

        if (listePoliciers.Count == 0)
        {
            zoneControle = false;
        }

    }

    void SpawnPolicier()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-0.35f, 0.35f), Random.Range(-0.6f, -1.3f));
        Policier unPolicier = (Instantiate(prefabPolicier.gameObject, randomPosition, Quaternion.identity) as GameObject).GetComponent<Policier>();
        
        unPolicier.onDeath.AddListener(MortPolicier);
    }
}

