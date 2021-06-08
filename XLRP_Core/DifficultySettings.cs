using System;
using Harmony;
using BattleTech;

namespace BTR_Core
{
    class DifficultySettings
    {
        //Upgrade degraded armor for OpFor to make them more stunty.
        [HarmonyPatch(typeof(AbstractActor), "CreateSpawnEffectByTag", null)]
        public static class Armor_Upgrade
        {
            private static void Prefix(AbstractActor __instance, ref string effectTag)
            {
                if (Core.Settings.UpgradeDegradedOpFor &&  (effectTag == "spawn_poorly_maintained_50" || effectTag == "spawn_poorly_maintained_75"))
                {
                    effectTag = "spawn_poorly_maintained_25";
                }
            }
        }

        //Nerf crazy scaling of payments from contracts.
        [HarmonyPatch(typeof(SimGameState), "CalculateContractValueByContractType")]
        public static class SimGameState_CalculateContractValueByContractType_Patch
        {
            public static void Postfix(int diff, ref int __result)
            {
                if (Core.Settings.NerfContractPayments)
                {
                    __result = (int)(__result * Math.Pow(Core.Settings.NerfExponent, diff));
                }
            }
        }

    }

}
