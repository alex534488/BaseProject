using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;


public class BarbareLeader : INewDay
{
    private BarbarianClan myClan;

    public BarbareLeader(BarbarianClan myClan)
    {
        this.myClan = myClan;
    }

    public void NewDay()
    {
        List<int> enemyTerritories = new List<int>();
        enemyTerritories = Universe.Map.GetAdjacentEnemyTerritory(myClan.GetPosition(), BarbareManager.TEAM);

        if (enemyTerritories.Count > 0)
        {
            if (myClan.GetCounter() <= 0)
            {
                // le clan attaque!
                myClan.Attack(enemyTerritories[Random.Range(0, enemyTerritories.Count - 1)]);
            }
            else
            {
                myClan.DecreaseCounter(1);
            }
        }
        else
        {
            myClan.UpdatePosition();
            myClan.DecreaseCounter(1);
        }
    }

     public virtual void ApplyModification()
    {
        // rien ici
    }
}

// Exemple de classe qui heriterait de barbareleader
public class BarbareLeaderTest : BarbareLeader
{
    private BarbarianClan myClan;
    private int armypower; // Cet exemple de leader donne un bonus de ArmyPower a son clan

    public BarbareLeaderTest(BarbarianClan myClan, int armypower) : base(myClan)
    {
        this.armypower = armypower;
    }

    public override void ApplyModification()
    {
        base.ApplyModification();
        myClan.AddPower(armypower);
    }
}
