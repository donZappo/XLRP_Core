using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using HBS.Collections;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;

namespace XLRP_Core.EnemySelection
{
    public static class Upgrade_Equipment
    {
        [HarmonyPatch(typeof(Team), "CollectUnitBaseline")]
        public static class Team_CollectUnitBaseline_Patch
        {
            private static void Postfix(Team __instance)
            {
                if (__instance.Combat.ActiveContract.ContractTypeValue.UsesFury)
                    return;

                foreach (AbstractActor actor in __instance.units)
                {
                    if (!Core.Settings.UpgradeItems || actor.EncounterTags.Contains("Upgraded") || actor.team.IsLocalPlayer)
                        continue;

                    actor.EncounterTags.Add("Upgraded");
                    foreach (var foo in actor.allComponents)
                    { 
                        if (foo.componentType == ComponentType.Weapon && foo.componentDef.ComponentTags.Contains("component_type_stock"))
                        {
                            Traverse.Create(foo).Property("componentDef").
                                SetValue(UpgradeWeapons(__instance.Combat.ActiveContract, foo.componentDef));
                        }
                        if (foo.componentType == ComponentType.Upgrade && foo.componentDef.ComponentTags.Contains("component_type_stock"))
                            Traverse.Create(foo).Property("componentDef").
                                SetValue(UpgradeUpgrades(__instance.Combat.ActiveContract, foo.componentDef));
                    }
                }
            }
        }


        //These patches and methods will dynamically upgrade the weapons and equipment on the OpFor.
        //[HarmonyPatch(typeof(UnitSpawnPointGameLogic), "SpawnMech")]
        //public static class UnitSpawnPointGameLogic_SpawnMech_Patch
        //{
        //    public static void Postfix(UnitSpawnPointGameLogic __instance, ref Mech __result)
        //    {
        //        if (!Core.Settings.UpgradeItems)
        //            return;
        //        var tempMechInventory = new List<MechComponentRef>(__result.MechDef.Inventory).ToArray();
        //        int i = 0;
        //        foreach (var component in tempMechInventory)
        //        {
        //            if (component.ComponentDefType == ComponentType.Weapon && component.Def.ComponentTags.Contains("component_type_stock"))
        //            {
        //                Traverse.Create(__result.MechDef.Inventory[i]).Property("Def").
        //                    SetValue(UpgradeWeapons(__instance.Combat.ActiveContract, component.Def));
        //            }
        //            if (component.ComponentDefType == ComponentType.Upgrade && component.Def.ComponentTags.Contains("component_type_stock"))
        //                Traverse.Create(__result.MechDef.Inventory[i]).Property("Def").
        //                    SetValue(UpgradeUpgrades(__instance.Combat.ActiveContract, component.Def));
        //            i++;
        //        }

        //    }
        //}

        //[HarmonyPatch(typeof(UnitSpawnPointGameLogic), "SpawnVehicle")]
        //public static class UnitSpawnPointGameLogic_SpawnVehicle_Patch
        //{
        //    public static void Postfix(UnitSpawnPointGameLogic __instance, ref Vehicle __result)
        //    {
        //        if (!Core.Settings.UpgradeItems)
        //            return;

        //        var tempMechInventory = new List<VehicleComponentRef>(__result.VehicleDef.Inventory).ToArray();
        //        int i = 0;
        //        foreach (var component in tempMechInventory)
        //        {
        //            if (component.ComponentDefType == ComponentType.Weapon && component.Def.ComponentTags.Contains("component_type_stock"))
        //            {
        //                Traverse.Create(__result.VehicleDef.Inventory.ElementAt(i)).Property("Def").
        //                    SetValue(UpgradeWeapons(__instance.Combat.ActiveContract, component.Def));
        //            }
        //            if (component.ComponentDefType == ComponentType.Upgrade && component.Def.ComponentTags.Contains("component_type_stock"))
        //                Traverse.Create(__result.VehicleDef.Inventory.ElementAt(i)).Property("Def").
        //                    SetValue(UpgradeUpgrades(__instance.Combat.ActiveContract, component.Def));
        //            i++;
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(UnitSpawnPointGameLogic), "SpawnTurret")]
        //public static class UnitSpawnPointGameLogic_SpawnTurret_Patch
        //{
        //    public static void Postfix(UnitSpawnPointGameLogic __instance, ref Turret __result)
        //    {
        //        if (!Core.Settings.UpgradeItems)
        //            return;

        //        var tempMechInventory = new List<TurretComponentRef>(__result.TurretDef.Inventory).ToArray();
        //        int i = 0;
        //        foreach (var component in tempMechInventory)
        //        {
        //            if (component.ComponentDefType == ComponentType.Weapon && component.Def.ComponentTags.Contains("component_type_stock"))
        //            {
        //                Traverse.Create(__result.TurretDef.Inventory.ElementAt(i)).Property("Def").
        //                    SetValue(UpgradeWeapons(__instance.Combat.ActiveContract, component.Def));
        //            }
        //            if (component.ComponentDefType == ComponentType.Upgrade && component.Def.ComponentTags.Contains("component_type_stock"))
        //                Traverse.Create(__result.TurretDef.Inventory.ElementAt(i)).Property("Def").
        //                    SetValue(UpgradeUpgrades(__instance.Combat.ActiveContract, component.Def));
        //            i++;
        //        }
        //    }
        //}

        public static MechComponentDef UpgradeUpgrades(Contract contract, MechComponentDef def)
        {
            var sc = UnityGameInstance.BattleTechGame.Simulation.Constants;
            var rand = new Random();
            double num = rand.Next();
            float num2 = ((float)contract.Override.finalDifficulty + sc.Salvage.VeryRareUpgradeChance) / sc.Salvage.UpgradeChanceDivisor;
            float num3 = ((float)contract.Override.finalDifficulty + sc.Salvage.RareUpgradeChance) / sc.Salvage.UpgradeChanceDivisor;
            float[] array = null;

            MechComponentDef mechComponentDef = def;
            if (num < num2)
            {
                array = sc.Salvage.VeryRareUpgradeLevel;
            }
            else if (num < num3)
            {
                array = sc.Salvage.RareUpgradeLevel;
            }
            if (array != null)
            {
                List<UpgradeDef_MDD> upgradesByRarityAndOwnership = MetadataDatabase.Instance.GetUpgradesByRarityAndOwnership(array);
                if (upgradesByRarityAndOwnership != null && upgradesByRarityAndOwnership.Count > 0)
                {
                    upgradesByRarityAndOwnership.Shuffle<UpgradeDef_MDD>();
                    UpgradeDef_MDD upgradeDef_MDD = upgradesByRarityAndOwnership[0];
                    var DataManager = (DataManager)Traverse.Create(contract).Field("dataManager").GetValue();
                    mechComponentDef = DataManager.UpgradeDefs.Get(upgradeDef_MDD.UpgradeDefID);
                }
            }
            return mechComponentDef;
        }

        public static WeaponDef UpgradeWeapons(Contract contract, MechComponentDef def)
        {
            var rand = new Random();
            var sc = UnityGameInstance.BattleTechGame.Simulation.Constants;
            var num = rand.NextDouble();
            float num2 = ((float)contract.Override.finalDifficulty + sc.Salvage.VeryRareWeaponChance) / sc.Salvage.WeaponChanceDivisor;
            float num3 = ((float)contract.Override.finalDifficulty + sc.Salvage.RareWeaponChance) / sc.Salvage.WeaponChanceDivisor;
            float[] array = null;
            if (num < num2)
            {
                array = sc.Salvage.VeryRareWeaponLevel;
            }
            else if (num < num3)
            {
                array = sc.Salvage.RareWeaponLevel;
            }
            WeaponDef weaponDef = def as WeaponDef;
            if (array != null)
            {
                List<WeaponDef_MDD> weaponsByTypeAndRarityAndOwnership = MetadataDatabase.Instance.GetWeaponsByTypeAndRarityAndOwnership(weaponDef.WeaponSubType, array);
                if (weaponsByTypeAndRarityAndOwnership != null && weaponsByTypeAndRarityAndOwnership.Count > 0)
                {
                    weaponsByTypeAndRarityAndOwnership.Shuffle<WeaponDef_MDD>();
                    WeaponDef_MDD weaponDef_MDD = weaponsByTypeAndRarityAndOwnership[0];
                    var DataManager = (DataManager)Traverse.Create(contract).Field("dataManager").GetValue();
                    weaponDef = DataManager.WeaponDefs.Get(weaponDef_MDD.WeaponDefID);
                }
            }
            return weaponDef;
        }

        //Cut off salvage upgrading at the knees
        [HarmonyPatch(typeof(Contract), "AddMechComponentToSalvage")]
        public static class Contract_AddWeaponToSalvage_Patch
        {
            }
            public static bool Prefix(Contract __instance, SimGameConstants sc, MechComponentDef def)
            {
            if (def.ComponentTags.Contains("BLACKLISTED"))
                return false;

            SalvageDef salvageDef = new SalvageDef();
            salvageDef.MechComponentDef = def;
            salvageDef.Description = new DescriptionDef(def.Description);
            salvageDef.RewardID = __instance.GenerateRewardUID();
            salvageDef.Type = SalvageDef.SalvageType.COMPONENT;
            salvageDef.ComponentType = def.ComponentType;
            salvageDef.Damaged = false;
            salvageDef.Weight = sc.Salvage.DefaultComponentWeight;
            salvageDef.Count = 1;
            if (def.Description.Rarity < sc.Salvage.ItemAutoCullLevel)
            {
                if ((bool)Traverse.Create(__instance).Method("IsSalvageInContent", new Type[] { typeof(SalvageDef) }).GetValue(salvageDef))
                {
                    var foo = (List<SalvageDef>)Traverse.Create(__instance).Field("finalPotentialSalvage").GetValue();
                    foo.Add(salvageDef);
                    return false;
                }
            }
            else
            {
                Logger.Log(string.Format("CULLED ILLEGAL MECH COMPONENT ({0}) of RARITY ({1}). From Random Upgrade? {2}.", def.Description.Id, def.Description.Rarity, false));
            }
            return false;
        }
    }


    //// Put better pilots in their proper 'Mechs. Thanks Haree!!
    [HarmonyPatch(typeof(UnitSpawnPointOverride), "RequestPilot")]
    public static class UnitSpawnPointOverride_RequestPilot
    {
        static bool Prefix(UnitSpawnPointOverride __instance, ref LoadRequest request, MetadataDatabase mdd, string lanceName, int unitIndex)
        {
            if (UnityGameInstance.BattleTechGame.Simulation != null && Core.Settings.UpgradePilots)
            {
                try
                {
                    DataManager dataManager = Traverse.Create(__instance).Field("dataManager").GetValue<DataManager>();

                    if (__instance.pilotDefId == UnitSpawnPointGameLogic.PilotDef_Tagged &&
                        __instance.selectedUnitType == UnitType.Mech &&
                        __instance.selectedUnitDefId != UnitSpawnPointGameLogic.MechDef_None &&
                        dataManager != null &&
                        __instance.pilotTagSet.IsEmpty &&
                        __instance.pilotExcludedTagSet.IsEmpty)
                    {
                        //Logger.Log($"Adding Exclusions to pilot: {__instance.pilotDefId}, for lance: {lanceName}, unit index: {unitIndex}, in Mech: {__instance.selectedUnitDefId}");
                        /*
                        Skill	    -	            Gunnery 8	    Piloting 8	    Guts 8	        Tactics 8
                        Gunnery 5	Gunner		    -               Flanker	        Gladiator	    Striker
                        Piloting 5	Pilot	        Skirmisher	    -               Brawler	        Scout
                        Guts 5      Defender	    Lancer	        Outrider	    -               Vanguard
                        Tactics 5	Tactician	    Sharpshooter	Recon	        Sentinel        -
                         */

                        // Add the Tags we want
                        MechDef mechDef = dataManager.MechDefs.Get(__instance.selectedUnitDefId);
                        TagSet excludeTags = new TagSet();


                        if (mechDef.Chassis.ChassisTags.Contains("mech_quirk_multitrac"))
                        {
                            // Excluding tier 2 skilled pilots that don't have Multi Target for Multi-Trac quirk
                            excludeTags.Add("pilot_npc_outrider");
                            excludeTags.Add("pilot_npc_recon");
                            excludeTags.Add("pilot_npc_brawler");
                            excludeTags.Add("pilot_npc_sentinel");
                            excludeTags.Add("pilot_npc_scout");
                            excludeTags.Add("pilot_npc_vanguard");
                        }

                        // Brawlers
                        if (mechDef.MechTags.Contains("unit_role_brawler"))
                        {
                            // Excluding tier 2 pilots that have no or next to no survivability skills in brawlers
                            excludeTags.Add("pilot_npc_sharpshooter");
                            excludeTags.Add("pilot_npc_skirmisher");

                            if (!mechDef.MechTags.Contains("unit_speed_high"))
                            {
                                // We aren't fast, get rid of shoot and move with no additional survivability
                                excludeTags.Add("pilot_npc_striker");
                            }

                            if (mechDef.MechTags.Contains("unit_speed_low") &&
                                !mechDef.MechTags.Contains("unit_jumpOK"))
                            {
                                // We are slow, can't jump and want to brawl, tactics 8 with Piloting 5 will not be enough for us to live on
                                excludeTags.Add("pilot_npc_scout");
                            }
                        }

                        // Snipers
                        if (mechDef.MechTags.Contains("unit_role_sniper"))
                        {
                            if (mechDef.MechTags.Contains("unit_speed_low") ||
                                (!mechDef.MechTags.Contains("unit_speed_high") && !mechDef.MechTags.Contains("unit_jumpOK")))
                            {
                                // Excluding all piloting 8 if a sniper in medium speed or less mech
                                excludeTags.Add("pilot_npc_flanker");
                                excludeTags.Add("pilot_npc_outrider");
                                excludeTags.Add("pilot_npc_recon");
                            }
                        }

                        // Scouts
                        if (mechDef.MechTags.Contains("unit_role_scout"))
                        {
                            // Let's throw away Coolant Vent in scouts
                            excludeTags.Add("pilot_npc_gladiator");
                            excludeTags.Add("pilot_npc_brawler");
                            excludeTags.Add("pilot_npc_sentinel");

                            if (mechDef.MechTags.Contains("unit_light") ||
                               mechDef.MechTags.Contains("unit_medium"))
                            {
                                // Excluding Gunnery 8 on light and medium scouts
                                excludeTags.Add("pilot_npc_skirmisher");
                                excludeTags.Add("pilot_npc_lancer");
                                excludeTags.Add("pilot_npc_sharpshooter");
                            }
                        }

                        // Slow ass Mechs
                        if (mechDef.MechTags.Contains("unit_speed_low"))
                        {
                            if (!mechDef.MechTags.Contains("unit_jumpOK"))
                            {
                                // Excluding non Outrider, piloting 8 in slow, non JJ Mechs
                                // Leaving Outrider as a brawler might want it even if slow
                                excludeTags.Add("pilot_npc_flanker");
                                excludeTags.Add("pilot_npc_recon");
                            }
                        }

                        // Fast fuckers
                        if (mechDef.MechTags.Contains("unit_speed_high"))
                        {
                            if (mechDef.MechTags.Contains("unit_armor_low") &&
                                !mechDef.MechTags.Contains("unit_role_sniper"))
                            {
                                // We are protected by paper but fast, we are not a sniper, let's use certain combos if we have a tier 2 skill
                                // Excluding Gunnery 8
                                excludeTags.Add("pilot_npc_skirmisher");
                                excludeTags.Add("pilot_npc_lancer");
                                excludeTags.Add("pilot_npc_sharpshooter");

                                // Excluding Gunnery 5 if we don't have Piloting 8
                                excludeTags.Add("pilot_npc_striker");
                                excludeTags.Add("pilot_npc_gladiator");
                            }

                            if (!mechDef.MechTags.Contains("unit_armor_high"))
                            {
                                if (!mechDef.MechTags.Contains("unit_hot"))
                                {
                                    // We aren't high armour, we are fast, and we aren't hot
                                    // Let's throw away Coolant Vent in non hot mechs to improve chances of more appropriate skills
                                    excludeTags.Add("pilot_npc_brawler");
                                    excludeTags.Add("pilot_npc_sentinel");
                                }
                            }
                        }
                        /*
                        Logger.Log($"Exclusions:");
                        foreach (string anExclude in excludeTags)
                        {
                            Logger.Log("   " + anExclude);
                        }*/

                        // TagSet should remove duplicates so no need to do that
                        __instance.pilotExcludedTagSet.AddRange(excludeTags);

                        /*Logger.Log($"Final Exclusions:");
                        foreach (string anExclude in __instance.pilotExcludedTagSet)
                        {
                            Logger.Log("   " + anExclude);
                        }*/

                        // Now do the same as the method would and don't call original method
                        PilotDef_MDD pilotDef_MDD = UnitSpawnPointOverride.SelectTaggedPilotDef(mdd, __instance.pilotTagSet, __instance.pilotExcludedTagSet, lanceName, unitIndex);
                        __instance.selectedPilotDefId = pilotDef_MDD.PilotDefID;

                        request.AddBlindLoadRequest(BattleTechResourceType.PilotDef, __instance.selectedPilotDefId, new bool?(false));

                        return false;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            return true;
        }
    }
}

