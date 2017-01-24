using UnityEngine;
using System.Collections;

public class BattleLauncher
{
    /// <summary>
    /// Lance et résoue une bataille entre des barbares et un village
    /// </summary>
    static public BattleResult LaunchBattle(BarbarianClan barbarianClan, Village village, bool barbareAttack = true)
    {
        // 1- Crée le battle setting en fonction du 'barbarianPower' et du 'village'
        Battle battle = null;
        if (barbareAttack)
            battle = new Battle(barbarianClan.GetPower(), village.Get(Village_ResourceType.armyPower),
                Universe.Barbares.HitRate, village.Empire.Get(Empire_ResourceType.armyHitRate));
        else
            battle = new Battle(village.Get(Village_ResourceType.armyPower), barbarianClan.GetPower(),
                village.Empire.Get(Empire_ResourceType.armyHitRate), Universe.Barbares.HitRate);

        // 2- Résoudre le combat
        BattleResult result = battle.Resolve(barbareAttack);

        // 3- Réduire la quantité de soldat dans le village, dans les barbares, etc.
        village.ApplyBattleResult(result);
        barbarianClan.ApplyBattleResult(result);

        //Renvoie le résultat de bataille à des fin informative
        return result;
    }
}
