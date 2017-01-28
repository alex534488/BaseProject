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
    private BarbareLeader leader;

    public BarbarianClan(int armyPower, int attackCooldown, int mapPosition, float hitRate, BarbareLeader leader = null)
    {
        this.armyPower.Set(armyPower);
        this.attackCooldown = attackCooldown;
        this.mapPosition = mapPosition;
        this.hitRate = hitRate;
        attackCoolDownCounter = attackCooldown;
        if(leader == null)
        {
            this.leader = new BarbareLeader(this);
        } else
        {
            this.leader = leader;
        }
        
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
        if(leader != null)
        {
            leader.NewDay();
        } else
        {
            // Si pas de leader ?
        }
    }

    public void UpdatePosition()
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

    public void Attack(int position)
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
            if (!(Universe.Map.IsEnemyTerritory(BarbareManager.TEAM, newPosition)))
            {
                mapPosition = newPosition;
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
    }

    public void DecreaseCounter(int amount)
    {
        attackCoolDownCounter -= amount;
    }
}