using UnityEngine;
using System.Collections;

public class BattleResult {

    private int invaderLeft;
    private int invaderDeath;
    private int defenderLeft;
    private int defenderDeath;
    private bool defenceWin;

    public BattleResult(int invaderLeft, int defenderLeft)
    {
        this.invaderLeft = invaderLeft;
        this.defenderLeft = defenderLeft;
        invaderDeath = 0;
        defenderDeath = 0;
        defenceWin = true; // la defence a gagner sauf si l'attaquant reussit a le battre?
    }

	public void AddInvaderDeaths(int amount)
    {
        invaderLeft -= amount;
        invaderDeath += amount;
    }

    public void AddDefenderDeaths(int amount)
    {
        defenderLeft -= amount;
        defenderDeath += amount;
    }

    public void InvaderWin()
    {
        defenceWin = false;
    }
}
