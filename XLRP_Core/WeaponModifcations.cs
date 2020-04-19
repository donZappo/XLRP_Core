using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using BattleTech;
using UnityEngine;
using BattleTech.UI;

namespace XLRP_Core.NewTech
{
    public static class COIL
    {
        //Modifies COILs to allow heat per shot
        [HarmonyPatch(typeof(Weapon), "GetProjectedCOILHeat")]
        public static class Weapon_GetProjectedCOILHeat_Patch
        {
            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (!Core.Settings.COIL_Heat_Multiply_EP)
                    return;

                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (__instance.weaponDef.Type == WeaponType.COIL && (!__instance.parent.SprintedLastRound
                   || (__instance.parent.JumpedLastRound && sim.CombatConstants.ResolutionConstants.COILUsesJumping)))
                {
                    __result = __result * __instance.parent.EvasivePipsCurrent;
                }
            }
        }

        [HarmonyPatch(typeof(Weapon), "HeatGenerated", MethodType.Getter)]
        public static class Weapon_HeatGenerated_Patch
        {
            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (!Core.Settings.COIL_Heat_Multiply_EP)
                    return;

                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (__instance.weaponDef.Type == WeaponType.COIL && (!__instance.parent.SprintedLastRound
                    || (__instance.parent.JumpedLastRound && sim.CombatConstants.ResolutionConstants.COILUsesJumping)))
                {
                    __result = __result * __instance.parent.EvasivePipsCurrent;
                }
            }
        }
    }

    public static class TAG
    {
        //Allows Tag to enable called shot
        [HarmonyPatch(typeof(SelectionStateFire), "NeedsCalledShot", MethodType.Getter)]
        public static class SelectionStateFire_NeedsCalledShot_Patch
        {
            public static void Postfix(SelectionStateFire __instance, ref bool __result)
            {
                if (__instance.TargetedCombatant == null || __instance.TargetedCombatant.Combat.EffectManager == null)
                    return;

                if (__instance.TargetedCombatant.UnitType != UnitType.Mech &&
                    __instance.TargetedCombatant.UnitType != UnitType.Vehicle)
                    return;

                if (Core.Settings.Tagged_Called_Shots)
                {
                    var isTagged = __instance.TargetedCombatant.Combat.EffectManager.GetAllEffectsTargeting(__instance.TargetedCombatant)
                        .Any(x => x.EffectData.Description.Name == "TAG MARKED");
                    if (isTagged)
                        __result = true;
                }
            }
        }


        [HarmonyPatch(typeof(AbstractActor), "IsVulnerableToCalledShots")]
        public static class AbstractActor_IsVulnerableToCalledShots_Patch
        {
            public static void Postfix(AbstractActor __instance, ref bool __result)
            {
                if (Core.Settings.Tagged_Called_Shots)
                {
                    if (__instance.UnitType != UnitType.Mech && __instance.UnitType != UnitType.Vehicle)
                        return;

                    var combat = UnityGameInstance.BattleTechGame.Combat;
                    var isTagged = combat.EffectManager.GetAllEffectsTargeting(__instance)
                    .Any(x => x.EffectData.Description.Name == "TAG MARKED");
                    if (isTagged)
                        __result = true;
                }
            }
        }
    }
}