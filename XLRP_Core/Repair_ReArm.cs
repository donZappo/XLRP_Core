using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using BattleTech.UI;
using Harmony;
using Localize;

namespace XLRP_Core
{
    class Repair_ReArm
    {
        [HarmonyPatch(typeof(AAR_UnitStatusWidget), "FillInPilotData")]
        public static class Add_Fatigue_To_Pilots_Prefix
        {
            public static void Postfix(AAR_UnitStatusWidget __instance, SimGameState ___simState)
            {
                var sim = UnityGameInstance.BattleTechGame.Simulation;

                UnitResult unitResult = Traverse.Create(__instance).Field("UnitData").GetValue<UnitResult>();
                float armorDamage = unitResult.mech.MechDefCurrentArmor / unitResult.mech.MechDefAssignedArmor;
                string armorDamageTag = "XLRPArmor_" + armorDamage.ToString();

                if (!unitResult.mech.MechTags.Contains("XLRP_R&R") && armorDamage < 1)
                {
                    unitResult.mech.MechTags.Add("XLRP_R&R");
                    unitResult.mech.MechTags.Add(armorDamageTag);
                }
                else if (armorDamage < 1)
                {
                    unitResult.mech.MechTags.Where(tag => tag.StartsWith("XLRPArmor")).Do(x => sim.CompanyTags.Remove(x));
                    unitResult.mech.MechTags.Add(armorDamageTag);
                }
            }
        }

        [HarmonyPatch(typeof(MechValidationRules), "GetMechFieldableWarnings")]
        public static class LanceConfiguratorPanel_ContinueConfirmedClicked_Patch
        {
            public static void Postfix(ref List<Text> __result, MechDef mechDef)
            {
                if (mechDef != null && mechDef.MechTags.Contains("XLRP_R&R"))
                {
                    Text RR_Text = new Text("REPAIR & REARM: 'Mech has armor damage that needs repair", Array.Empty<object>());
                    __result.Add(RR_Text);
                }
            }
        }

        [HarmonyPatch(typeof(SimGameState), "OnDayPassed")]
        public static class SimGameState_OnDayPassed_Patch
        {
            static void Prefix(SimGameState __instance, int timeLapse)
            {
                foreach (var mech in __instance.ActiveMechs.Values)
                {
                    if (mech.MechTags.Contains("XLRP_R&R"))
                    {
                        mech.MechTags.Where(tag => tag.StartsWith("XLRPArmor")).Do(x => __instance.CompanyTags.Remove(x));
                        mech.MechTags.Remove("XLRP_R&R");
                    }
                }
            }
        }
    }
}
