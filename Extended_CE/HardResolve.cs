using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Harmony;
using Random = UnityEngine.Random;
using BattleTech;

namespace XLRP_Core.HardResolveChanges
{
    //// Apply Hard mode Called Shot Mastery Nerf
    //[HarmonyPatch(typeof(Pilot), "AddAbility")]
    //public static class Pilot_AddAbility
    //{
    //    static void Postfix(Pilot __instance, string abilityName)
    //    {
    //        try
    //        {
    //            if (UnityGameInstance.BattleTechGame.Simulation != null &&
    //                (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Hard ||
    //                UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation))
    //            {
    //                if (__instance.ParentActor is Mech mech)
    //                {
    //                    if (abilityName == "TraitDefCalledShotMaster")
    //                    {
    //                        // We just added Called Shot Mastery to a Mech Pilot and we are in Hard game mode, nerf that shit
    //                        int Uid() => Random.Range(1, int.MaxValue);
    //                        var effectManager = UnityGameInstance.BattleTechGame.Combat.EffectManager;

    //                        EffectData hardCalledShotMasteryNerf =
    //                            new EffectData
    //                            {
    //                                effectType = EffectType.StatisticEffect,
    //                                targetingData = new EffectTargetingData
    //                                {
    //                                    effectTriggerType = EffectTriggerType.Passive,
    //                                    triggerLimit = 0,
    //                                    extendDurationOnTrigger = 0,
    //                                    specialRules = AbilityDef.SpecialRules.NotSet,
    //                                    auraEffectType = AuraEffectType.NotSet,
    //                                    effectTargetType = EffectTargetType.SingleTarget,
    //                                    alsoAffectCreator = false,
    //                                    range = 0f,
    //                                    forcePathRebuild = false,
    //                                    forceVisRebuild = false,
    //                                    showInTargetPreview = false,
    //                                    showInStatusPanel = false,
    //                                },
    //                                Description = new DescriptionDef("HardCalledShotMasteryNerf", "CSNERF", "",
    //                                    "uixSvgIcon_ability_mastertactician", 0, 0, false, null, null, null),
    //                                durationData = new EffectDurationData()
    //                                {
    //                                    duration = -1,
    //                                    stackLimit = 1
    //                                },
    //                                statisticData = new StatisticEffectData
    //                                {
    //                                    statName = "CalledShotBonusMultiplier",
    //                                    operation = StatCollection.StatOperation.Float_Multiply,
    //                                    modValue = Core.Settings.CSMasteryNerf.ToString(CultureInfo.InvariantCulture),
    //                                    modType = "System.Single"
    //                                }
    //                            };

    //                        effectManager.CreateEffect(hardCalledShotMasteryNerf, "HardCalledShotMasteryNerf", Uid(), mech, mech, new WeaponHitInfo(), 0);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //    }
    //}


    //// Apply Hard mode less resolve at max gain from morale
    //[HarmonyPatch(typeof(Team), "CollectSimGameBaseline")]
    //public static class Team_CollectSimGameBaseline
    //{
    //    static void Postfix(Team __instance, ref int __result, MoraleConstantsDef moraleConstants)
    //    {
    //        try
    //        {
    //            if (UnityGameInstance.BattleTechGame.Simulation != null &&
    //                (UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Hard ||
    //                UnityGameInstance.BattleTechGame.Simulation.Constants.Story.MaximumDebt == (int)DifficultySetting.Simulation))
    //            {
    //                if (moraleConstants.BaselineAddFromSimGame)
    //                {
    //                    int maxMorale = moraleConstants.BaselineAddFromSimGameThresholds[moraleConstants.BaselineAddFromSimGameThresholds.Length - 1];

    //                    if (__result == maxMorale)
    //                    {
    //                        __result -= Core.Settings.LessResolveAtMaxMorale;
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //    }
    //}
}
