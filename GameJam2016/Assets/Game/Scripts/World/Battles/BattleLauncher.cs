using UnityEngine;
using System.Collections;

public class BattleLauncher {
    /// <summary>
    /// Lance et résoue une bataille entre des barbares et un village
    /// </summary>
    static public BattleResult LaunchBattle(BarbarianClan barbarianClan, Village village, bool barbareAttack = true)
    {
        // 1- Crée le battle setting en fonction du 'barbarianPower' et du 'village'
        Battle battle = new Battle(barbarianClan.GetPower(), village.Get(Village_ResourceType.armyPower));

        // 2- Résoudre le combat
        BattleResult result = battle.Resolve(barbareAttack);

        // 3- Réduire la quantité de soldat dans le village, dans les barbares, etc.
            // TODO

        //Renvoie le résultat de bataille à des fin informative
        return result;
    }
}
