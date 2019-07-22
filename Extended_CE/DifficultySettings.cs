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

namespace Extended_CE
{
    class DifficultySettings
    {
        [HarmonyPatch(typeof(LineOfSight), "GetAllSensorRangeAbsolutes")]
        public static class LineOfSight_GetAllSensorRangeAbsolutes_Patch
        {
            public static void Postfix(ref float __result)
            {
                if (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == 42)
                __result += Core.Settings.SimSensorDistance;
            }
        }

        [HarmonyPatch(typeof(LineOfSight), "GetAllSpotterAbsolutes")]
        public static class LineOfSight_GetAllSpotterAbsolutes_Patch
        {
            public static void Postfix(ref float __result)
            {
                if (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == 42)
                    __result += Core.Settings.SimSpotterDistance;
            }
        }
    }
}
