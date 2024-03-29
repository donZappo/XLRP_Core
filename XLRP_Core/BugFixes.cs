﻿using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using BattleTech;
using BattleTech.UI;
using BattleTech.Save;
using BattleTech.Data;
using HBS.Collections;
using System.Reflection.Emit;
using System.Reflection;
using CustAmmoCategories;

namespace BTR_Core
{
    class BugFixes_QoL
    {
        [HarmonyPatch(typeof(Shop), "OnItemCollectionRetrieved")]
        public class ShopOnItemCollectionRetrievedHackFixPatch
        {
            public static bool Prefix(Shop __instance, ItemCollectionDef def)
            {
                if (def == null)
                {
                    Logger.Log($"{__instance.system.Name} has invalid ItemCollectionDef.");
                }

                return def != null;
            }
        }
        
        [HarmonyPatch(typeof(ListElementController_InventoryWeapon_NotListView), "RefreshQuantity")]
        public static class Bug_Tracing_Fix
        {
            static bool Prefix(ListElementController_InventoryWeapon_NotListView __instance, InventoryItemElement_NotListView theWidget)
            {
                try
                {
                    if (__instance.quantity == -2147483648)
                    {
                        theWidget.qtyElement.SetActive(false);
                        return false;
                    }

                    theWidget.qtyElement.SetActive(true);
                    theWidget.quantityValue.SetText("{0}", __instance.quantity);
                    theWidget.quantityValueColor.SetUIColor(__instance.quantity > 0 || __instance.quantity == int.MinValue ? UIColor.White : UIColor.Red);
                    return false;
                }
                catch (Exception e)
                {
                    Logger.Log("*****Exception thrown with ListElementController_InventoryWeapon_NotListView");
                    Logger.Log($"theWidget null: {theWidget == null}");
                    Logger.Log($"theWidget.qtyElement null: {theWidget.qtyElement == null}");
                    Logger.Log($"theWidget.quantityValue null: {theWidget.quantityValue == null}");
                    Logger.Log($"theWidget.quantityValueColor null: {theWidget.quantityValueColor == null}");
                    if (theWidget.itemName != null)
                    {
                        Logger.Log("theWidget.itemName");
                        Logger.Log(theWidget.itemName.ToString());
                    }

                    if (__instance.GetName() != null)
                    {
                        Logger.Log("__instance.GetName");
                        Logger.Log(__instance.GetName());
                    }

                    Logger.Log(e);
                    return false;
                }
            }
        }

        ////Slight bug fix for Pathing Grid errors.
        //[HarmonyPatch(typeof(Pathing), "ResetPathGridIfTouching")]
        //public static class Logging_ResetPathGridIfTouching_Patch
        //{
        //    static PathingCapabilitiesDef pathingCapStorage = null;
        //    public static void Prefix(Pathing __instance, List<Rect> Rectangles, Vector3 origin, float beginAngle, AbstractActor actor)
        //    {
        //        if (__instance.PathingCaps == null)
        //            __instance.PathingCaps = pathingCapStorage;
        //        else
        //            pathingCapStorage = __instance.PathingCaps;
        //    }
        //}

        //Fix a bug with the Locust FP that would permanently disable the Black Market.
        [HarmonyPatch(typeof(SimGameState), "Rehydrate")]
        public static class SimGameState_Rehydrate_Patch
        {
            static void Postfix(SimGameState __instance, GameInstanceSave gameInstanceSave)
            {
                if (!__instance.CompanyTags.Contains("BTR_BugFix_LocustFPFixed") && __instance.CompanyTags.Contains("fp_legUp_kickingTheNest_Complete") &&
                    !__instance.CompanyTags.Contains("company_blackMarket_ON"))
                {
                    if (__instance.CompanyTags.Contains("company_blackMarket_OFF"))
                        __instance.CompanyTags.Remove("company_blackMarket_OFF");
                }

                if (!__instance.CompanyTags.Contains("BTR_BugFix_LocustFPFixed"))
                    __instance.CompanyTags.Add("BTR_BugFix_LocustFPFixed");
            }
        }

        //Boost AI sensor and spotter range to prevent them from being exploited during combat.
        [HarmonyPatch(typeof(AbstractActor), "ResolveAttackSequence")]
        public static class AbstractActor_ResolveAttackSequence_Patch
        {
            public static void Prefix(AbstractActor __instance)
            {
                if (Core.Settings.RemoveSpottingExploit && !__instance.EncounterTags.Contains("BTR_SensorsAdjusted")
                                                        && __instance.team.IsEnemy(__instance.Combat.LocalPlayerTeam))
                {
                    __instance.StatCollection.Set<float>("SpotterDistanceMultiplier", 2);
                    __instance.StatCollection.Set<float>("SensorDistanceMultiplier", 2);
                    __instance.EncounterTags.Add("BTR_SensorsAdjusted");
                }
            }
        }

        [HarmonyPatch(typeof(Mech), "ResolveAttackSequence")]
        public static class Mech_ResolveAttackSequence_Patch
        {
            public static void Prefix(Mech __instance)
            {
                if (Core.Settings.RemoveSpottingExploit && !__instance.EncounterTags.Contains("BTR_SensorsAdjusted")
                                                        && __instance.team.IsEnemy(__instance.Combat.LocalPlayerTeam))
                {
                    __instance.StatCollection.Set<float>("SpotterDistanceMultiplier", 2);
                    __instance.StatCollection.Set<float>("SensorDistanceMultiplier", 2);
                    __instance.EncounterTags.Add("BTR_SensorsAdjusted");
                }
            }
        }

        [HarmonyPatch(typeof(Vehicle), "ResolveAttackSequence")]
        public static class Vehicle_ResolveAttackSequence_Patch
        {
            public static void Prefix(Vehicle __instance)
            {
                if (Core.Settings.RemoveSpottingExploit && !__instance.EncounterTags.Contains("BTR_SensorsAdjusted")
                                                        && __instance.team.IsEnemy(__instance.Combat.LocalPlayerTeam))
                {
                    __instance.StatCollection.Set<float>("SpotterDistanceMultiplier", 2);
                    __instance.StatCollection.Set<float>("SensorDistanceMultiplier", 2);
                    __instance.EncounterTags.Add("BTR_SensorsAdjusted");
                }
            }
        }


        [HarmonyPatch(typeof(Turret), "ResolveAttackSequence")]
        public static class Turret_ResolveAttackSequence_Patch
        {
            public static void Prefix(Turret __instance)
            {
                if (Core.Settings.RemoveSpottingExploit && !__instance.EncounterTags.Contains("BTR_SensorsAdjusted")
                                                        && __instance.team.IsEnemy(__instance.Combat.LocalPlayerTeam))
                {
                    __instance.StatCollection.Set<float>("SpotterDistanceMultiplier", 2);
                    __instance.StatCollection.Set<float>("SensorDistanceMultiplier", 2);
                    __instance.EncounterTags.Add("BTR_SensorsAdjusted");
                }
            }
        }

        [HarmonyPatch(typeof(AbstractActor), "OnActivationEnd")]
        public static class AbstractActor_OnActivationEnd_Patch
        {
            public static void Prefix(AbstractActor __instance)
            {
                if (Core.Settings.RemoveSpottingExploit && __instance.EncounterTags.Contains("BTR_SensorsAdjusted")
                                                        && __instance.team.IsEnemy(__instance.Combat.LocalPlayerTeam))
                {
                    __instance.StatCollection.Set<float>("SpotterDistanceMultiplier", 1);
                    __instance.EncounterTags.Remove("BTR_SensorsAdjusted");
                }
            }
        }

        ////Why are melee attacks against vehicles not increasing? It appears to have been removed with CAC. Welcome back! 
        [HarmonyPatch(typeof(AdvWeaponHitInfoRec), "Apply")]
        public static class AdvWeaponHitInfoRec_Apply_Patch
        {
            public static void Prefix(AdvWeaponHitInfoRec __instance)
            {
                if (!Core.Settings.correctMeleeMultipliers)
                    return;

                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (__instance.target.UnitType == UnitType.Vehicle && __instance.parent.weapon.Type == WeaponType.Melee)
                    __instance.Damage *= sim.CombatConstants.ResolutionConstants.MeleeDamageMultiplierVehicle;
                if (__instance.target.UnitType == UnitType.Turret && __instance.parent.weapon.Type == WeaponType.Melee)
                    __instance.Damage *= sim.CombatConstants.ResolutionConstants.MeleeDamageMultiplierTurret;
            }
        }

        //Mech blue portraits in combat stick around if free sensor lock is enabled. 
        [HarmonyPatch(typeof(CombatHUDMechwarriorTray), "ShowMoraleBackground")]
        public static class CombatHUDMechwarriorTray_ShowMoraleBackground_Patch
        {
            public static void Prefix(ref bool show)
            {
                show = false;
            }
        }

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
        //[HarmonyPatch(typeof(SimGameInterruptManager), "QueueEventPopup")]
        //public static class SimGameInterruptManager_QueueEventPopup_Patch
        //{
        //    public static void Prefix(ref SimGameEventDef evt)
        //    {
        //        //if (Core.Settings.ObfuscateEventRequirements)
        //        //    Traverse.Create(evt)
        //        //        .Property("Options")
        //        //        .SetValue(evt.Options.Where(o => o.RequirementList.Length == 0).ToArray());
        //    }
        //}


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
                if (damageAmount < 0f)
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
            static bool Prepare()
            {
                return Core.Settings.ShowAllArgoUpgrades;
            }

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = instructions.ToList();
                var targetMethod = AccessTools.Method(typeof(ShipModuleUpgrade), "get_RequiredModules");
                // want 2nd and final occurence of targetMethod
                var index = codes.FindIndex(c =>
                    c == codes.Last(x => x.opcode == OpCodes.Callvirt && (MethodInfo) x.operand == targetMethod));
                // nop out the instructions for the 2nd conditional
                // && __instance.simState.HasShipUpgrade(shipModuleUpgrade2.RequiredModules, list)
                for (var i = -3; i < 4; i++)
                {
                    codes[index + i].opcode = OpCodes.Nop;
                }

                return codes.AsEnumerable();
            }
        }

        // automatically uninstalls any components installed in invalid locations (as a result of json editing and a saved mech)
        [HarmonyPatch(typeof(MechLabPanel), "LoadMech")]
        public class MechLabPanelLoadMechPatch
        {
            private static void Postfix(MechLabPanel __instance)
            {
                try
                {
                    var newMechDef = __instance.activeMechDef;
                    var sim = UnityGameInstance.BattleTechGame.Simulation;
                    var inventory = newMechDef?.Inventory;
                    for (var index = 0; index < inventory?.Length; index++)
                    {
                        var mechComponentRef = inventory[index];
                        if (mechComponentRef != null)
                        {
                            var loc = mechComponentRef.Def?.AllowedLocations & mechComponentRef.MountedLocation;
                            if (loc != mechComponentRef.MountedLocation)
                            {
                                var componentInstallWorkOrder = sim.CreateComponentInstallWorkOrder(
                                    newMechDef.GUID, mechComponentRef, ChassisLocations.None, mechComponentRef.MountedLocation);
                                componentInstallWorkOrder.Parent = componentInstallWorkOrder;
                                componentInstallWorkOrder.CBillCost = 0;
                                sim.MoveWorkOrderItemsToQueue(componentInstallWorkOrder);
                                __instance.pendingWorkOrders.Add(componentInstallWorkOrder);
                                __instance.ApplyWorkOrder(componentInstallWorkOrder);
                                __instance.DoConfirmRefit();
                                var popup = GenericPopupBuilder.Create(GenericPopupType.Info,
                                    $"Component removed from invalid location and placed in storage.\n{mechComponentRef.Def.Description.Name} in {mechComponentRef.MountedLocation}");
                                popup.AddButton("Understood");
                                popup.Render();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Log(ex.ToString());
                }

                // non-working prototype to display the view distance on the mechlab based on equipped rangefinder
                //FileLog.Log("PING");
                //LocalizableText component = ___obj_mech.FindFirstChildNamed("uixPrfPanl_ML_main-Widget(Clone)").FindFirstChildNamed("Centerline").FindFirstChildNamed("uixPrfPanl_ML_location-Widget-MANAGED")
                //    .FindFirstChildNamed("txt_location")
                //    .GetComponent<LocalizableText>();
                //FileLog.Log(component.text);
                //IEnumerable<MechComponentRef> enumerable = ___activeMechInventory.Where((MechComponentRef x) => x.Def.statusEffects.Any((EffectData y) => y.statisticData.statName == "SpotterDistanceAbsolute"));
                //float num = 0f;
                //FileLog.Log("Rangefinders: " + enumerable.Count());
                //foreach (MechComponentRef item in enumerable)
                //{
                //    StatisticEffectData statisticData = item.Def.statusEffects.First((EffectData x) => x.statisticData.statName == "SpotterDistanceAbsolute").statisticData;
                //    StatCollection.StatOperation operation = statisticData.operation;
                //    FileLog.Log(operation.ToString());
                //    int num2 = Convert.ToInt32(statisticData.modValue);
                //    FileLog.Log(num2.ToString());
                //    switch (operation)
                //    {
                //        case StatCollection.StatOperation.Float_Add:
                //            num += (float)num2;
                //            break;
                //        case StatCollection.StatOperation.Float_Subtract:
                //            num -= (float)num2;
                //            break;
                //    }
                //}
                //FileLog.Log(num.ToString());
                //string arg = null;
                //if (num > 0f)
                //{
                //    arg = $"+{num}m";
                //}
                //else if (num < 0f)
                //{
                //    arg = $"-{num}m";
                //}
                //component.SetText($"{component} {arg}");
            }
        }
    }
}
