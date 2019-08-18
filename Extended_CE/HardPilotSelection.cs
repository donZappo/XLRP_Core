using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using HBS.Collections;
using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;

namespace Extended_CE.PilotSelection
{
    // Apply all to hit based quirks
    [HarmonyPatch(typeof(UnitSpawnPointOverride), "RequestPilot")]
    public static class UnitSpawnPointOverride_RequestPilot
    {
        static bool Prefix(UnitSpawnPointOverride __instance, ref LoadRequest request, MetadataDatabase mdd, string lanceName, int unitIndex)
        {
            if (UnityGameInstance.BattleTechGame.Simulation != null &&
                (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Hard ||
                UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation))
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
                        Logger.Log($"Adding Exclusions to pilot: {__instance.pilotDefId}, for lance: {lanceName}, unit index: {unitIndex}, in Mech: {__instance.selectedUnitDefId}");
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

                            if(!mechDef.MechTags.Contains("unit_armor_high"))
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
