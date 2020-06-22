using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using UnityEngine;
using BattleTech.Framework;
using BattleTech.UI;
using BattleTech.Save;

namespace XLRP_Core
{
    class DifficultySettings
    {
        //Upgrade degraded armor for OpFor to make them more stunty.
        [HarmonyPatch(typeof(AbstractActor), "CreateSpawnEffectByTag", null)]
        public static class Armor_Upgrade
        {
            // Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
            private static void Prefix(AbstractActor __instance, ref string effectTag)
            {
                if (Core.Settings.UpgradeDegradedOpFor &&  (effectTag == "spawn_poorly_maintained_50" || effectTag == "spawn_poorly_maintained_75"))
                {
                    effectTag = "spawn_poorly_maintained_25";
                }
            }
        }
    }

}
