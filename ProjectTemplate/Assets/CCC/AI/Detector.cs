using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Detector : SlowBehaviour
{

    public List<Personnage> allyList = new List<Personnage>();
    public List<Personnage> enemyList = new List<Personnage>();
    List<Personnage> unapprovedUnits = new List<Personnage>();

    public LayerMask terrainMask;

    public Personnage.PersonnageEvent onEnemyEnter = new Personnage.PersonnageEvent();
    public Personnage.PersonnageEvent onEnemyExit = new Personnage.PersonnageEvent();

    public Personnage.PersonnageEvent onAllyEnter = new Personnage.PersonnageEvent();
    public Personnage.PersonnageEvent onAllyExit = new Personnage.PersonnageEvent();

    System.Type[] enemies;
    System.Type[] allies;

    bool hasInit = false;
    bool needAllyVision = false;
    bool needEnemyVision = false;

    void Awake()
    {
        if (!hasInit) gameObject.SetActive(false);
    }

    public void Init(System.Type[] enemy, bool needEnemyVision, System.Type[] ally, bool needAllyVision)
    {
        gameObject.SetActive(true);
        this.enemies = enemy;
        this.allies = ally;
        hasInit = true;
        this.needAllyVision = needAllyVision;
        this.needEnemyVision = needEnemyVision;
    }

    protected override void SlowUpdate()
    {
        base.SlowUpdate();
        UpdateUnitLists();
    }

    public bool CanSee(Transform target)
    {
        return !Physics.Linecast(target.position, transform.position, terrainMask);
    }

    public Personnage GetClosestAlly(System.Type filter = null)
    {
        return GetClosestFrom(allyList, filter);
    }

    public Personnage GetClosestEnemy(System.Type filter = null)
    {
        return GetClosestFrom(enemyList, filter);
    }

    void UpdateUnitLists()
    {
        //Check unapproved
        for (int i=0; i<unapprovedUnits.Count; i++)
        {
            if (i >= unapprovedUnits.Count) break;

            Personnage personnage = unapprovedUnits[i];

            //si est un ennemi
            if (IsIn(personnage.GetType(), enemies))
            {
                if (!needEnemyVision || CanSee(personnage.transform))
                {
                    enemyList.Add(personnage);
                    unapprovedUnits.Remove(personnage);
                    i--;
                    onEnemyEnter.Invoke(personnage);
                }
            }
            //Si est un allié
            else if (IsIn(personnage.GetType(), allies))
            {
                if (!needAllyVision || CanSee(personnage.transform))
                {
                    allyList.Add(personnage);
                    unapprovedUnits.Remove(personnage);
                    i--;
                    onAllyEnter.Invoke(personnage);
                }
            }
            //N'est ni un allié ni un ennemi, on s'en fou de lui...
            else unapprovedUnits.Remove(personnage);
        }

        //Check enemies
        if (needEnemyVision)
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (i >= enemyList.Count) break;

                Personnage enemy = enemyList[i];

                if (!CanSee(enemy.transform))
                {
                    unapprovedUnits.Add(enemy);
                    enemyList.Remove(enemy);
                    onEnemyExit.Invoke(enemy);
                    i--;
                }
            }

        //Check allies
        if (needAllyVision)
            for (int i = 0; i < allyList.Count; i++)
            {
                if (i >= allyList.Count) break;

                Personnage ally = allyList[i];

                if (!CanSee(ally.transform))
                {
                    unapprovedUnits.Add(ally);
                    allyList.Remove(ally);
                    onAllyExit.Invoke(ally);
                    i--;
                }
            }
    }

    Personnage GetClosestFrom(List<Personnage> liste, System.Type filter)
    {

        Personnage closest = null;
        float smallestDist = Mathf.Infinity;
        foreach (Personnage personnage in liste)
        {
            if (filter != null && personnage.GetType() != filter)
                continue;

            float dist = (personnage.transform.position - transform.position).magnitude;
            if (dist < smallestDist)
            {
                closest = personnage;
                smallestDist = dist;
            }
        }
        return closest;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform == transform.parent) return;

        Personnage personnage = col.GetComponent<Personnage>();
        if (personnage == null) return;

        unapprovedUnits.Add(personnage);
        UpdateUnitLists();
    }

    void OnTriggerExit(Collider col)
    {
        Personnage personnage = col.GetComponent<Personnage>();
        if (personnage == null) return;

        if (enemyList.Contains(personnage))
        {
            enemyList.Remove(personnage);
            onEnemyExit.Invoke(personnage);
        }
        else if (allyList.Contains(personnage))
        {
            allyList.Remove(personnage);
            onAllyExit.Invoke(personnage);
        }
        else unapprovedUnits.Remove(personnage);
    }

    bool IsIn(System.Type type, System.Type[] array)
    {
        if (array == null) return false;

        for (int i = 0; i < array.Length; i++)
        {
            if (type == array[i]) return true;
        }
        return false;
    }
}