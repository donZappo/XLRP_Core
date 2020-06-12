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
using BattleTech.Data;
using HBS.Collections;
using System.Reflection.Emit;
using System.Reflection;

namespace XLRP_Core
{
    class BugFixes_QoL
    {
        //Area set up for logging methods for data.
        [HarmonyPatch(typeof(SimGameState))]
        [HarmonyPatch("GetMechComponentRefForUID")]
        public static class LogAMethod
        {
            public static void Prefix(MechDef mech, string simGameUID, string componentID, ComponentType componentType)
            {
                try
                {
                    Logger.Log("================");
                    Logger.Log($"mech exist {mech != null}");
                    Logger.Log($"mech: {mech.Name}");
                    Logger.Log($"simGameUID exist {simGameUID != null}");
                    Logger.Log($"simGameUID: {simGameUID}");
                    Logger.Log($"componentID exist {componentID != null}");
                    Logger.Log($"componentID: {componentID}");
                    Logger.Log($"componentType exist {componentType != null}");
                    Logger.Log($"componentType: {componentType}");
                }
                catch (Exception e)
                {
                    Logger.Log("Name Bad");
                }
            }
        }

        ////If mechs with COIL weapons don't move before they fire, they fire with their evasive pips generated from the previous round.
        //[HarmonyPatch(typeof(Weapon), "ShotsWhenFired", MethodType.Getter)]
        //public static class Weapon_ShotsWhenFired_Patch
        //{
        //    public static void Postfix(Weapon __instance, ref int __result)
        //    {
        //        var actor = __instance.parent;

        //        if (!actor.HasMovedThisRound && __instance.weaponDef.Type == WeaponType.COIL)
        //            __result = 1;
        //    }
        //}

        //Mech blue portraits in combat stick around if free sensor lock is enabled. 
        [HarmonyPatch(typeof(CombatHUDMechwarriorTray), "ShowMoraleBackground")]
        public static class CombatHUDMechwarriorTray_ShowMoraleBackground_Patch
        {
            public static void Prefix(ref bool show)
            {
                show = false;
            }
        }

        //If mechs with COIL weapons don't move before they fire, they fire with their evasive pips generated from the previous round.
        //****** This is still a problem. But by enabling this code mechs cannot generate bonus shots when meleeing. 
        //[HarmonyPatch(typeof(Weapon), "ShotsWhenFired", MethodType.Getter)]
        //public static class Weapon_ShotsWhenFired_Patch
        //{
        //    public static void Postfix(Weapon __instance, ref int __result)
        //    {
        //        var actor = __instance.parent;
        //        actor.HasSprintedThisRound
        //        if (!actor.HasMovedThisRound && __instance.weaponDef.Type == WeaponType.COIL)
        //            __result = 1;
        //    }
        //}


        //PathNodeGrid.BuildPathNetwork AbstractActor.IsFriendly throws a million NREs
            [HarmonyPatch(typeof(AbstractActor), "IsFriendly")]
        public static class AbstractActor_IsFriendly_Patch
        {
            public static bool Prefix(AbstractActor __instance, ICombatant target)
            {
                if (Core.Settings.IsFriendlyBugSuppression)
                {
                    try
                    {
                        return __instance.team.IsFriendly(target.team);
                    }
                    catch { return false; }
                }
                return true;
            }
        }

        //This will show that requirements are not met for the events, but not say what exactly they are.
            [HarmonyPatch(typeof(SimGameInterruptManager), "QueueEventPopup")]
        public static class SimGameInterruptManager_QueueEventPopup_Patch
        {
            public static void Prefix(ref SimGameEventDef evt)
            {
                //if (Core.Settings.ObfuscateEventRequirements)
                //    Traverse.Create(evt)
                //        .Property("Options")
                //        .SetValue(evt.Options.Where(o => o.RequirementList.Length == 0).ToArray());
            }
        }


        // Fix for PC controlled NPC pilots that die during Flashpoints such as Fangerholm and Kell
        [HarmonyPatch(typeof(Contract), "FinalizeKilledMechWarriors")]
        public static class Contract_FinalizeKilledMechWarriors_Patch
        {
            static void Prefix(Contract __instance, SimGameState sim)
            {
                foreach (UnitResult unitResult in __instance.PlayerUnitResults)
                {
                    bool MakeImmortal = true;
                    foreach (Pilot pilot in sim.PilotRoster)
                    {
                        if (pilot.Name == unitResult.pilot.Name)
                            MakeImmortal = false;
                    }
                    if (MakeImmortal)
                        Traverse.Create(unitResult.pilot.pilotDef).Property("IsImmortal").SetValue(true);
                }
            }
        }

        // Fix for potential negative damage values being applied based on reducing self DFA damage
        [HarmonyPatch(typeof(Mech), "TakeWeaponDamage")]
        public static class Mech_TakeWeaponDamage_Patch
        {
            static void Prefix(Mech __instance, ref float damageAmount)
            {
                if(damageAmount < 0f)
                {
                    damageAmount = 0f;
                }
            }
        }

        //removes logging of excessively long tags from bundled mods.
        [HarmonyPatch(typeof(TagSetQueryExtensions), "CanRandomlySelectUnitDef")]
        public static class TagSetQueryExtensions_GetMatchingUnitDefs_Patch
        {
            static void Prefix(ref TagSet companyTags)
            {
                var tempTagSet = new TagSet(companyTags);
                foreach (var tag in tempTagSet)
                {
                    if (tag.StartsWith("PilotQuirksSave") || tag.StartsWith("GalaxyAtWarSave"))
                        companyTags.Remove(tag);
                }
            }
        }

        //Shows all the Argo upgrade icons at all times. I am convinced this is a bug, not a feature. Thank you for the dark magic, gnivler!
        [HarmonyPatch(typeof(SGEngineeringScreen), "PopulateUpgradeDictionary")]
        public static class SGEngineeringScreen_PopulateUpgradeDictionary_Patch
        {
            static bool Prepare() { return Core.Settings.ShowAllArgoUpgrades; }
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = instructions.ToList();
                var targetMethod = AccessTools.Method(typeof(ShipModuleUpgrade), "get_RequiredModules");
                // want 2nd and final occurence of targetMethod
                var index = codes.FindIndex(c =>
                    c == codes.Last(x => x.opcode == OpCodes.Callvirt && (MethodInfo)x.operand == targetMethod));
                // nop out the instructions for the 2nd conditional
                // && this.simState.HasShipUpgrade(shipModuleUpgrade2.RequiredModules, list)
                for (var i = -3; i < 4; i++)
                {
                    codes[index + i].opcode = OpCodes.Nop;
                }
                return codes.AsEnumerable();
            }
        }
    }
}
