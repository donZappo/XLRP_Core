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
        ////Area set up for logging methods for data.
        //[HarmonyPatch(typeof(SimGameState))]
        //[HarmonyPatch("GetMechComponentRefForUID")]
        //public static class LogAMethod
        //{
        //    public static void Prefix(MechDef mech, string simGameUID, string componentID, ComponentType componentType)
        //    {
        //        try
        //        {
        //            Logger.Log("======GetMechComponentRefForUID==========");
        //            Logger.Log($"mech exist {mech != null}");
        //            Logger.Log($"mech: {mech.Name}");
        //            Logger.Log($"simGameUID exist {simGameUID != null}");
        //            Logger.Log($"simGameUID: {simGameUID}");
        //            Logger.Log($"componentID exist {componentID != null}");
        //            Logger.Log($"componentID: {componentID}");
        //            Logger.Log($"componentType exist {componentType != null}");
        //            Logger.Log($"componentType: {componentType}");
        //        }
        //        catch (Exception e)
        //        {
        //            Logger.Log("Name Bad");
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(UnitSpawnPointGameLogic))]
        //[HarmonyPatch("Spawn")]
        //public static class LogASecondMethod
        //{
        //    public static bool Prefix(UnitSpawnPointGameLogic __instance, bool spawnOffScreen)
        //    {
        //        try
        //        { 
        //            Logger.Log("======UnitSpawnPointGameLogic.Spawn==========");
        //            var foo = __instance;
        //            if (!__instance.HasUnitToSpawn)
        //            {
        //                return false;
        //            }
        //            Team team = foo.Combat.TurnDirector.GetTurnActorByUniqueId(__instance.teamDefinitionGuid) as Team;
        //            if (__instance.teamDefinitionGuid == "421027ec-8480-4cc6-bf01-369f84a22012" || team == null)
        //            {
        //                UnitSpawnPointGameLogic.logger.LogError(string.Format("Invalid teamIndex for SpawnPoint [{0}][{1}] - TeamIndex[{2}]", foo.name, __instance.encounterObjectGuid, __instance.teamDefinitionGuid));
        //                return false;
        //            }
        //            PilotDef pilot = null;
        //            HeraldryDef heraldryDef = null;
        //            if (!string.IsNullOrEmpty(__instance.customHeraldryDefId) && !foo.Combat.DataManager.Heraldries.TryGet(__instance.customHeraldryDefId, out heraldryDef))
        //            {
        //                foo.LogError("Invalid custom heraldry id: " + __instance.customHeraldryDefId, false);
        //            }
        //            if (__instance.pilotDefOverride != null)
        //            {
        //                Logger.Log("PilotOverride?");
        //                pilot = __instance.pilotDefOverride;
        //                Logger.Log("PilotOverriden");
        //            }
        //            else
        //            {
        //                Logger.Log("PilotNotOverride?");
        //                string text = string.IsNullOrEmpty(__instance.pilotDefId) ? UnitSpawnPointGameLogic.PilotDef_Default : __instance.pilotDefId;
        //                if (!foo.Combat.DataManager.PilotDefs.TryGet(text, out pilot))
        //                {
        //                    foo.LogError(string.Format("PilotDef [{0}] not previously requested. Falling back to [{1}]", text, UnitSpawnPointGameLogic.PilotDef_Default), true);
        //                    pilot = foo.Combat.DataManager.PilotDefs.Get(UnitSpawnPointGameLogic.PilotDef_Default);
        //                }
        //                Logger.Log("PilotNotOverriden?");
        //            }
        //            Lance lanceByUID = team.GetLanceByUID(__instance.lanceGuid);
        //            string text2 = string.Empty;
        //            AbstractActor abstractActor;
        //            switch (__instance.unitType)
        //            {
        //                case UnitType.Mech:
        //                    {
        //                        Logger.Log("IsMech");
        //                        MechDef mechDef = null;
        //                        if (__instance.mechDefOverride != null)
        //                        {
        //                            Logger.Log("MechOverride?");
        //                            mechDef = __instance.mechDefOverride;

        //                            Logger.Log(mechDef.Name);
        //                        }
        //                        else
        //                        {
        //                            Logger.Log("MechNotOverriden");
        //                            if (!foo.Combat.DataManager.MechDefs.TryGet(__instance.mechDefId, out mechDef))
        //                            {
        //                                foo.LogError(string.Format("MechDef [{0}] not previously requested. Aborting unit spawn.", __instance.mechDefId), true);
        //                                return false;
        //                            }
        //                            if (!mechDef.DependenciesLoaded(1000u))
        //                            {
        //                                UnitSpawnPointGameLogic.logger.LogError(string.Format("Invalid mechdef for SpawnPoint [{0}][{1}] - [{2}]", foo.name, __instance.encounterObjectGuid, __instance.mechDefId), foo.gameObject);
        //                                return false;
        //                            }
        //                            mechDef.Refresh();
        //                        }
        //                        Logger.Log($"mechDef.ChassisID null: {mechDef.ChassisID == null}");
        //                        text2 = mechDef.ChassisID;
        //                        Logger.Log(text2);
        //                        Logger.Log("AbstractActorLogging");
        //                        Logger.Log($"mechDef null: {mechDef == null}");
        //                        Logger.Log($"pilot null: {pilot == null}");
        //                        Logger.Log($"team null: {team == null}");
        //                        Logger.Log($"lanceByUID null: {lanceByUID == null}");
        //                        Logger.Log($"heraldryDef null: {heraldryDef == null}");
        //                        Logger.Log(mechDef.Name);
        //                        Logger.Log(pilot.Description.Name);
        //                        Logger.Log(team.Name);
        //                        Logger.Log(lanceByUID.ToString());
        //                        abstractActor = __instance.SpawnMech(mechDef, pilot, team, lanceByUID, heraldryDef);
        //                        Logger.Log("Made abstractActor");
        //                        goto IL_303;
        //                    }
        //                case UnitType.Vehicle:
        //                    {
        //                        VehicleDef vehicleDef = null;
        //                        if (__instance.vehicleDefOverride != null)
        //                        {
        //                            vehicleDef = __instance.vehicleDefOverride;
        //                        }
        //                        else
        //                        {
        //                            if (!foo.Combat.DataManager.VehicleDefs.TryGet(__instance.vehicleDefId, out vehicleDef))
        //                            {
        //                                foo.LogError(string.Format("VehicleDef [{0}] not previously requested. Aborting unit spawn.", __instance.vehicleDefId), true);
        //                                return false;
        //                            }
        //                            vehicleDef.Refresh();
        //                        }
        //                        text2 = vehicleDef.ChassisID;
        //                        abstractActor = __instance.SpawnVehicle(vehicleDef, pilot, team, lanceByUID, heraldryDef);
        //                        goto IL_303;
        //                    }
        //                case UnitType.Turret:
        //                    {
        //                        TurretDef turretDef = null;
        //                        if (__instance.turretDefOverride != null)
        //                        {
        //                            turretDef = __instance.turretDefOverride;
        //                        }
        //                        else
        //                        {
        //                            if (!foo.Combat.DataManager.TurretDefs.TryGet(__instance.turretDefId, out turretDef))
        //                            {
        //                                foo.LogError(string.Format("TurretDef [{0}] not previously requested. Aborting unit spawn.", __instance.turretDefId), true);
        //                                return false;
        //                            }
        //                            turretDef.Refresh();
        //                        }
        //                        text2 = turretDef.ChassisID;
        //                        abstractActor = __instance.SpawnTurret(turretDef, pilot, team, lanceByUID, heraldryDef);
        //                        goto IL_303;
        //                    }
        //            }
        //            throw new ArgumentException("UnitSpawnPointGameLocic.SpawnUnit had invalid unitType: " + __instance.unitType.ToString());
        //            IL_303:
        //            Logger.Log($"Team.DisplayName is null: {team.DisplayName == null}");
        //            Logger.Log($"team.DisplayName: {team.DisplayName}");
        //            Logger.Log($"abstractActor.GUID is null: {abstractActor.GUID == null}");
        //            Logger.Log($"abstractActor.GUID: {abstractActor.GUID}");
        //            Logger.Log($"text2 is null: {text2 == null}");
        //            Logger.Log($"text2: {text2}");
        //            Logger.Log($"__instance.pilotDefId: {__instance.pilotDefId == null}");
        //            Logger.Log($"__instance.pilotDefId: {__instance.pilotDefId}");


        //            string message = string.Format("Spawning unit - Team [{0}], UnitId [{1}], ChassisId [{2}], PilotId [{3}]", new object[]
        //            {
        //        team.DisplayName,
        //        abstractActor.GUID,
        //        text2,
        //        __instance.pilotDefId
        //            });
        //            foo.LogMessage(message);
        //            abstractActor.OverriddenPilotDisplayName = __instance.customUnitName;
        //            __instance.lastSpawnedUnit = abstractActor;
        //            if (spawnOffScreen)
        //            {
        //                abstractActor.PlaceFarAwayFromMap();
        //                __instance.spawningOffScreen = true;
        //                __instance.timePlacedOffScreen = 0f;
        //            }
        //            AITeam aiteam = team as AITeam;
        //            if (aiteam != null && aiteam.ThinksOnThisMachine)
        //            {
        //                if (!Enum.IsDefined(typeof(BehaviorTreeIDEnum), (int)__instance.behaviorTree))
        //                {
        //                    __instance.behaviorTree = BehaviorTreeIDEnum.CoreAITree;
        //                }
        //                abstractActor.BehaviorTree = BehaviorTreeFactory.MakeBehaviorTree(foo.Combat.BattleTechGame, abstractActor, __instance.behaviorTree);
        //                for (int i = 0; i < __instance.aiOrderList.Count; i++)
        //                {
        //                    abstractActor.IssueAIOrder(__instance.aiOrderList[i]);
        //                }
        //            }
        //            for (int j = 0; j < __instance.spawnEffectTags.Count; j++)
        //            {
        //                abstractActor.CreateSpawnEffectByTag(__instance.spawnEffectTags[j], null);
        //            }
        //            UnitSpawnedMessage unitSpawnedMessage = new UnitSpawnedMessage(__instance.encounterObjectGuid, abstractActor.GUID);
        //            EncounterLayerParent.EnqueueLoadAwareMessage(unitSpawnedMessage);
        //            if (__instance.triggerInterruptPhaseOnSpawn)
        //            {
        //                abstractActor.IsInterruptActor = true;
        //                foo.Combat.StackManager.InsertInterruptPhase(team.GUID, unitSpawnedMessage.messageIndex);
        //            }
        //            return false;
        //        }
        //        catch (Exception e)
        //        {
        //            Logger.Log("Name Bad");
        //            return false;
        //        }
        //    }
        //}

        ////If mechs with COIL weapons don't move before they fire, they fire with their evasive pips generated from the previous round.
        //[HarmonyPatch(typeof(Weapon), "ShotsWhenFired", MethodType.Getter)]
        //public static class Weapon_ShotsWhenFired_Patch
        //{
        //    public static void Postfix(Weapon __instance, ref int __result)
        //    {
        //        var actor = __instance.parent;

        //        if (!actor.HasMoved__instanceRound && __instance.weaponDef.Type == WeaponType.COIL)
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
        //****** __instance is still a problem. But by enabling __instance code mechs cannot generate bonus shots when meleeing. 
        //[HarmonyPatch(typeof(Weapon), "ShotsWhenFired", MethodType.Getter)]
        //public static class Weapon_ShotsWhenFired_Patch
        //{
        //    public static void Postfix(Weapon __instance, ref int __result)
        //    {
        //        var actor = __instance.parent;
        //        actor.HasSprinted__instanceRound
        //        if (!actor.HasMoved__instanceRound && __instance.weaponDef.Type == WeaponType.COIL)
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

        //__instance will show that requirements are not met for the events, but not say what exactly they are.
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

        // Fix for potential negative damage values being applied food on reducing self DFA damage
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

        //Shows all the Argo upgrade icons at all times. I am convinced __instance is a bug, not a feature. Thank you for the dark magic, gnivler!
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
                // && __instance.simState.HasShipUpgrade(shipModuleUpgrade2.RequiredModules, list)
                for (var i = -3; i < 4; i++)
                {
                    codes[index + i].opcode = OpCodes.Nop;
                }
                return codes.AsEnumerable();
            }
        }
    }
}
