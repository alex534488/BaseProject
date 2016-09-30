using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Detector : MonoBehaviour {
    
    List<Personnage> allyList = new List<Personnage>();
    List<Personnage> enemyList = new List<Personnage>();

    public LayerMask terrainMask;

    public Personnage.PersonnageEvent onEnemyEnter = new Personnage.PersonnageEvent();
    public Personnage.PersonnageEvent onEnemyExit = new Personnage.PersonnageEvent();

    public Personnage.PersonnageEvent onAllyEnter = new Personnage.PersonnageEvent();
    public Personnage.PersonnageEvent onAllyExit = new Personnage.PersonnageEvent();

    System.Type[] enemies;
    System.Type[] allies;

    void Awake()
    {
        Debug.LogWarning("Test, remove this!");
        System.Type[] enemy =
        {
            typeof(Policier),
            typeof(Civil)
        };
        System.Type[] ally =
        {
            typeof(Zombie)
        };
        Init(enemy, ally);
    }
    
    public void Init(System.Type[] enemy, System.Type[] ally)
    {
        this.enemies = enemy;
        this.allies = ally;
    }

    public bool CanSee(Transform target)
    {
        return !Physics.Linecast(target.position, transform.position, terrainMask);
    }

    public Personnage GetClosestAlly()
    {
        return GetClosestFrom(allyList);
    }

    public Personnage GetClosestEnemy()
    {
        return GetClosestFrom(enemyList);
    }

    Personnage GetClosestFrom(List<Personnage> liste)
    {
        Personnage closest = null;
        float smallestDist = Mathf.Infinity;
        foreach (Personnage personnage in liste)
        {
            float dist = (personnage.transform.position - transform.position).magnitude;
            if(dist < smallestDist)
            {
                closest = personnage;
                smallestDist = dist;
            }
        }
        return closest;
    }

    void OnTriggerEnter(Collider col)
    {
        Personnage personnage = col.GetComponent<Personnage>();
        if (personnage == null) return;

        if(IsIn(personnage.GetType(), enemies))
        {
            print("enemy in");
            enemyList.Add(personnage);
            onEnemyEnter.Invoke(personnage);
        }
        else if (IsIn(personnage.GetType(), allies))
        {
            print("ally in");
            allyList.Add(personnage);
            onAllyEnter.Invoke(personnage);
        }
    }

    void OnTriggerExit(Collider col)
    {
        Personnage personnage = col.GetComponent<Personnage>();
        if (personnage == null) return;

        if (IsIn(personnage.GetType(), enemies))
        {
            print("enemy out");
            enemyList.Remove(personnage);
            onEnemyExit.Invoke(personnage);
        }
        else if (IsIn(personnage.GetType(), allies))
        {
            print("ally out");
            allyList.Remove(personnage);
            onAllyExit.Invoke(personnage);
        }
    }

    bool IsIn(System.Type type, System.Type[] array)
    {
        for(int i=0; i<array.Length; i++)
        {
            if (type == array[i]) return true;
        }
        return false;
    }
}