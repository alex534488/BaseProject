using UnityEngine;
using System.Collections;

public class Battle {

    BattleSetting battleSetting;
    BattleResult battleResult;

    Battle(BattleSetting battleSetting)
    {
        this.battleSetting = battleSetting;
        battleResult = new BattleResult(battleSetting.GetInvaderPower(),battleSetting.GetDefenderPower());
    }
	
	public BattleResult LaunchBattle()
    {
        // Resoudre la bataille et enregistrer le resultat dans battleResult

        return battleResult;
    }
}
