using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;

namespace Extended_CE.NewTech
{/*
    public static class ArtemisIV
    {
        internal static bool ArtemisWeapon = false;

        // Capture if this is an Artemis LRM
        [HarmonyPatch(typeof(AttackDirector.AttackSequence), "GetClusteredHits")]
        public static class AttackDirectorAttackSequence_GetClusteredHits
        {
            public static void Prefix(Weapon weapon)
            {
                try
                {
                    if (weapon != null &&
                    weapon.weaponDef != null)
                    {
                        if (weapon.weaponDef.ComponentTags.Contains("component_artemis"))
                        {
                            ArtemisWeapon = true;
                        }
                        else
                        {
                            ArtemisWeapon = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    ArtemisWeapon = false;
                }
            }

            //Reset Artemis
            public static void Postfix()
            {
                ArtemisWeapon = false;
            }
        }

        // Apply bonus to hit original location if Artemis IV weapon
        [HarmonyPatch(typeof(HitLocation), "GetAdjacentHitLocation")]
        public static class HitLocation_GetAdjacentHitLocation
        {
            public static void Prefix(HitLocation __instance, ref float originalMultiplier)
            {
                try
                {
                    if (ArtemisWeapon)
                    {
                        originalMultiplier *= Core.Settings.ArtemisIVBonus;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
    }*/
}
