using UnityEngine;
using System.Collections;

public class Battle
{

    public int invaderPower = 0;
    public int defenderPower = 0;
    public float invaderBuff = 0.9f;
    public float defenderBuff = 1.1f;
    public int invaderHitRate = 50;
    public int defenderHitRate = 50;

    public Battle()
    {

    }

    public Battle(int invaderPower, int defenderPower, int invaderHitRate, int defenderHitRate, float invaderBuff = 0.9f, float defenderBuff = 1.1f)
    {
        this.invaderPower = invaderPower;
        this.defenderPower = defenderPower;
        this.defenderHitRate = defenderHitRate;
        this.invaderHitRate = invaderHitRate;
        this.invaderBuff = invaderBuff;
        this.defenderBuff = defenderBuff;
    }

    public BattleResult Resolve(bool barbareAttack)
    {
        BattleResult result = new BattleResult();
        result.barbareAttack = barbareAttack;

        int invaderDamage = (int)(invaderPower * (((float)invaderHitRate) / 100 * Random.Range(0.7f, 1.3f)) * invaderBuff);
        int defenderDamage = (int)(defenderPower * (((float)defenderHitRate) / 100 * Random.Range(0.7f, 1.3f)) * defenderBuff);

        result.invaderLeft = invaderPower - defenderDamage;
        result.invaderDeath = invaderPower - result.invaderLeft;
        result.defenderLeft = defenderPower - invaderDamage;
        result.defenderDeath = defenderPower - result.defenderLeft;

        // On determine le gagnant...
        if (result.invaderLeft <= 0 && result.defenderLeft <= 0)
        {
            result.defenceWin = true;
        }
        else if (result.defenderLeft <= 0)
        {
            result.defenceWin = false;
        }
        else if (result.invaderLeft <= 0)
        {
            result.defenceWin = true;
        }
        else
        {
            result.defenceWin = true;
        }

        return result;
    }
}
