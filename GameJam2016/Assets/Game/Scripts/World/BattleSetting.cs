using UnityEngine;
using System.Collections;

public class BattleSetting {

    private int invaderPower;
    private int defenderPower;
    private float invaderBuff;
    private float defenderBuff;

    public BattleSetting(int invaderPower, int defenderPower, float invaderBuff = 1, float defenderBuff = 1)
    {
        this.invaderPower = invaderPower;
        this.defenderPower = defenderPower;
        this.invaderBuff = invaderBuff;
        this.defenderBuff = defenderBuff;
    }

	public int GetInvaderPower()
    {
        return invaderPower;
    }

    public int GetDefenderPower()
    {
        return defenderPower;
    }

    public float GetInvaderBuff()
    {
        return invaderBuff;
    }

    public float GetDefenderBuff()
    {
        return defenderBuff;
    }
}
