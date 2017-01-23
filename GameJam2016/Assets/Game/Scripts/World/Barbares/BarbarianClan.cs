using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

public class BarbarianClan : INewDay
{
    private int mapPosition;
    private int attackCooldown;
    private int attackCoolDownCounter;
    private Stat<int> armyPower = new Stat<int>(0);

    public BarbarianClan(int armyPower, int attackCooldown, int mapPosition)
    {
        this.armyPower.Set(armyPower);
        this.attackCooldown = attackCooldown;
        this.mapPosition = mapPosition;
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
        // TODO: Esquise, a changer en machine a etat fini si on veut

        List<int> enemyTerritories = new List<int>();
        enemyTerritories = Universe.Map.GetAdjacentEnemyTerritory(mapPosition, 2);

        if (enemyTerritories.Count >= 0)
        {
            if (attackCoolDownCounter <= 0)
            {
                // le clan attaque!
                OnAttacking(Random.Range(0, enemyTerritories.Count-1));
            }
        } else
        {
            // le clan fait autre chose
        }

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
            // TODO : Kill this clan
        }
    }

    private void OnAttacking(int position)
    {
        Village village = Universe.Map.GetVillage(position);
        BattleLauncher.LaunchBattle(this, village);
        if(armyPower > 0 && village.Get(Village_ResourceType.armyPower) <= 0)
        {
            Universe.Map.ChangeTerritoryOwner(position, 2);
            // TODO: Detruire le village
        }
    }

    public void OnMoving(int newPosition)
    {
        if (Universe.Map.IsAdjacent(mapPosition, newPosition))
        {
            if (!(Universe.Map.IsEnemyTerritory(2, newPosition)))
            {
                mapPosition = newPosition;
            } else // TODO: A ENLEVER ET REMPLACER PAR LE FUTUR SYSTEM D'AI
            {
                OnAttacking(newPosition);
            }
        }
    }

    public void OnDefending()
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