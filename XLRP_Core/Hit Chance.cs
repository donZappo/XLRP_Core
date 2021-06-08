namespace BTR_Core.Weapons
{
    //This block of code here can be used to have COIL weapons show an indicator for how long it has been since they fired. 
    //[HarmonyPatch(typeof(CombatHUDWeaponSlot), "RefreshDisplayedWeapon")]
    //public static class CombatHUDWeaponSlot_RefreshDisplayedWeapon_Patch
    //{
    //    public static void Postfix(CombatHUDWeaponSlot __instance)
    //    {
    //        if (__instance.DisplayedWeapon == null)
    //        {
    //            return;
    //        }
    //        if (__instance.DisplayedWeapon.Type == WeaponType.COIL)
    //        {
    //            var charges = Mathf.Clamp(__instance.DisplayedWeapon.roundsSinceLastFire, 0, 4);
    //            __instance.WeaponText.SetText($"{__instance.WeaponText.text} {new string('*', charges)}");
    //        }
    //    }
    //}


    // 
    //[HarmonyPatch(typeof(ToHit), "GetUMChance")]
    //public static class ToHit_GetUMChance
    //{
    //    public static bool Prefix(ToHit __instance, ref float __result, CombatGameState ___combat, float baseChance, float totalModifiers)
    //    {
    //        float num;
    //        if (___combat.Constants.ToHit.ToHitUseSteppedAlgorithm)
    //        {
    //            num = __instance.GetSteppedValue(baseChance, totalModifiers);
    //        }
    //        else
    //        {
    //            float toHitModifierDivisor = ___combat.Constants.ToHit.ToHitModifierDivisor;
    //            float num2 = 1f - totalModifiers / (totalModifiers + toHitModifierDivisor);
    //            num = baseChance * num2;
    //        }
    //        num = Mathf.Min(0.95f, num);
    //        __result = Mathf.Max(0.05f, num);
    //        return false;
    //    }
    //}
}
