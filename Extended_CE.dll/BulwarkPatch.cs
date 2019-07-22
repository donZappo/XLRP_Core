using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;
using BattleTech;
using BattleTech.UI;

namespace Extended_CE
{
    // Apply to hit bulwark change
    [HarmonyPatch(typeof(ToHit), "GetAllModifiers")]
    public static class ToHit_GetAllModifiers
    {
        static void Postfix(ToHit __instance, ref float __result, ICombatant target, Vector3 attackPosition, Vector3 targetPosition, ref CombatGameState ___combat)
        {
            try
            {
                if (target is Mech mech)
                {
                    if(mech.GuardLevel > 0 && mech.HasBulwarkAbility)
                    {
                        __result += Core.Settings.BulwarkMalus;
                    }

                    if (__result < 0f && !___combat.Constants.ResolutionConstants.AllowTotalNegativeModifier)
                    {
                        __result = 0f;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }

    // Add in Bulwark modifiers in to the weapon to hit preview tooltip
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "UpdateToolTipsFiring")]
    public static class CombatHUDWeaponSlot_UpdateToolTipsFiring
    {
        static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target)
        {
            try
            {
                if (target is Mech selectedMech)
                {
                    if (selectedMech.GuardLevel > 0 && selectedMech.HasBulwarkAbility && Core.Settings.BulwarkMalus != 0f)
                    {
                        Traverse.Create(__instance).Method("AddToolTipDetail", new object[] { "USING BULWARK", (int)Core.Settings.BulwarkMalus }).GetValue();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
