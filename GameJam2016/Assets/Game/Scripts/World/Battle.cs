using UnityEngine;
using System.Collections;

public class Battle {

    public int invaderPower = 0;
    public int defenderPower = 0;
    public float invaderBuff = 1;
    public float defenderBuff = 1;

    public Battle()
    {

    }

    public Battle(int invaderPower, int defenderPower, float invaderBuff = 1, float defenderBuff = 1)
    {
        this.invaderPower = invaderPower;
        this.defenderPower = defenderPower;
        this.invaderBuff = invaderBuff;
        this.defenderBuff = defenderBuff;
    }

    //TODO: mettre les formule de combat
    public BattleResult Resolve()
    {
        return new BattleResult();
    }
}
