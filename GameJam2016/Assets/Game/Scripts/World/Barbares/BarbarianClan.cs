using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

public class BarbarianClan : INewDay
{
    private int mapPosition;
    private int attackCooldown;
    private int attackCoolDownCounter;
    private float hitRate;
    private Stat<int> armyPower = new Stat<int>(0);

    public BarbarianClan(int armyPower, int attackCooldown, int mapPosition, float hitRate)
    {
        this.armyPower.Set(armyPower);
        this.attackCooldown = attackCooldown;
        this.mapPosition = mapPosition;
        this.hitRate = hitRate;
        attackCoolDownCounter = attackCooldown;
    }

    public int GetCoolDown()
    {
        return attackCooldown;
    }

    public int GetCounter()
    {
        return attackCoolDownCounter;
    }

    public void AddPower(int power)
    {
        armyPower.Set(armyPower + power);
    }

    public void NewDay()
    {
        // A changer en machine a etat fini si on veut

        List<int> enemyTerritories = new List<int>();
        enemyTerritories = Universe.Map.GetAdjacentEnemyTerritory(mapPosition, BarbareManager.TEAM);

        if (enemyTerritories.Count > 0)
        {
            if (attackCoolDownCounter <= 0)
            {
                // le clan attaque!
                Attack(enemyTerritories[Random.Range(0, enemyTerritories.Count-1)]);
            } 
        } else
        {
            UpdatePosition();
            attackCoolDownCounter--;
        }
    }

    private void UpdatePosition()
    {
        // Deplacement Random pour l'instant
        List<int> listAdjacentPosition = new List<int>();
        listAdjacentPosition = Universe.Map.GetAdjacentAlliedTerritory(mapPosition, BarbareManager.TEAM);
        Move(listAdjacentPosition[Random.Range(0, listAdjacentPosition.Count)]);
    }

    public int GetPosition()
    {
        return mapPosition;
    }

    public int GetPower()
    {
        return armyPower;
    }
    
    private void SetArmyPower(int power)
    {
        int result = armyPower.Set(power);
        if(result <= 0)
        {
            Universe.Barbares.Delete(this);
        }
    }

    private void Attack(int position)
    {
        Village village = Universe.Map.GetVillage(position);
        BattleLauncher.LaunchBattle(this, village);

        if(armyPower > 0 && village.IsDestroyed)
        {
            //Ils ont gagné la bataille et le village enemie à été détruit. Le territoire est maintenant à eux
            Move(position);
        }
        attackCoolDownCounter = attackCooldown;
    }

    public void Move(int newPosition)
    {
        if (Universe.Map.IsAdjacent(mapPosition, newPosition))
        {
            if (!(Universe.Map.IsEnemyTerritory(2, newPosition)))
            {
                mapPosition = newPosition;
            } else // TODO: A ENLEVER ET REMPLACER PAR LE FUTUR SYSTEM D'AI
            {
                Attack(newPosition);
            }
        }
    }

    public void Defend()
    {
        // A enlever possiblement, voir IsAttacked() dans barbareManager
    }

    public void Idle()
    {
        attackCoolDownCounter--;
    }

    public void ApplyBattleResult(BattleResult result)
    {
        if (result.barbareAttack)
        {
            SetArmyPower(result.invaderLeft);
        } else
        {
            SetArmyPower(result.defenderLeft);
        }
        attackCoolDownCounter = attackCooldown;
    }
}