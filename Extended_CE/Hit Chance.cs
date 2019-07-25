using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;
using BattleTech;

namespace Extended_CE.HitChance
{
    // 
    [HarmonyPatch(typeof(ToHit), "GetUMChance")]
    public static class ToHit_GetUMChance
    {
        public static bool Prefix(ToHit __instance, ref float __result, CombatGameState ___combat, float baseChance, float totalModifiers)
        {
            float num;
            if (___combat.Constants.ToHit.ToHitUseSteppedAlgorithm)
            {
                num = __instance.GetSteppedValue(baseChance, totalModifiers);
            }
            else
            {
                float toHitModifierDivisor = ___combat.Constants.ToHit.ToHitModifierDivisor;
                float num2 = 1f - totalModifiers / (totalModifiers + toHitModifierDivisor);
                num = baseChance * num2;
            }
            num = Mathf.Min(0.95f, num);
            __result = Mathf.Max(0.05f, num);
            return false;
        }
    }
}
