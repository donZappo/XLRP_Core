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
    //class DifficultySettings
    //{
    //    [HarmonyPatch(typeof(LineOfSight), "GetAllSensorRangeAbsolutes")]
    //    public static class LineOfSight_GetAllSensorRangeAbsolutes_Patch
    //    {
    //        public static void Postfix(ref float __result)
    //        {
    //            if (UnityGameInstance.BattleTechGame.Simulation != null)
    //            {
    //                if (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation)
    //                    __result += Core.Settings.SimSensorDistance;
    //            }
    //        }
    //    }

    //    [HarmonyPatch(typeof(LineOfSight), "GetAllSpotterAbsolutes")]
    //    public static class LineOfSight_GetAllSpotterAbsolutes_Patch
    //    {
    //        public static void Postfix(ref float __result)
    //        {
    //            if (UnityGameInstance.BattleTechGame.Simulation != null)
    //            {
    //                if (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation)
    //                    __result += Core.Settings.SimSpotterDistance;
    //            }
    //        }
    //    }

    //    [HarmonyPatch(typeof(SGShipModuleUpgradeViewPopulator), "Populate")]
    //    public static class SGShipModuleUpgradeViewPopulator_Populate_Patch
    //    {
    //        static string DetailHolder;
    //        static SimGameState sim = UnityGameInstance.BattleTechGame.Simulation;
    //        public static void Prefix(ShipModuleUpgrade upgrade)
    //        {
    //            if (sim.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation)
    //            {
    //                if (upgrade.Description.Id == "argoUpgrade_mechBay_machineShop")
    //                {
    //                    DetailHolder = upgrade.Description.Details;
    //                    Traverse.Create(upgrade.Description).Property("Details").SetValue("Maintaining 'Mechs in the Succession War era is complicated by the relative rarity of precision-manufactured parts. The <i>Argo</i>'s machine shop, once repaired and brought online, can help address this lack.");
    //                }
    //            }
    //        }
    //        public static void Postfix(ShipModuleUpgrade upgrade)
    //        {
    //            if (sim.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation)
    //            {
    //                if (upgrade.Description.Id == "argoUpgrade_mechBay_machineShop")
    //                {
    //                    Traverse.Create(upgrade.Description).Property("Details").SetValue(DetailHolder);
    //                }
    //            }
    //        }
    //    }
    //}
}
