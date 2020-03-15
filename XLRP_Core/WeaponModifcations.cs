using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using BattleTech;
using UnityEngine;
using BattleTech.UI;

namespace XLRP_Core.NewTech
{
    public static class COIL
    {
        //Modifies COILs to allow heat per shot
        [HarmonyPatch(typeof(Weapon), "GetProjectedCOILHeat")]
        public static class Weapon_GetProjectedCOILHeat_Patch
        {
            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (!Core.Settings.COIL_Heat_Multiply_EP)
                    return;

                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (__instance.weaponDef.Type == WeaponType.COIL && !__instance.parent.SprintedLastRound
                    && (__instance.parent.JumpedLastRound && !sim.CombatConstants.ResolutionConstants.COILUsesJumping))
                {
                    __result = __result * __instance.parent.EvasivePipsCurrent;
                }
            }
        }

        [HarmonyPatch(typeof(Weapon), "HeatGenerated", MethodType.Getter)]
        public static class Weapon_HeatGenerated_Patch
        {
            public static void Postfix(Weapon __instance, ref float __result)
            {
                if (!Core.Settings.COIL_Heat_Multiply_EP)
                    return;

                var sim = UnityGameInstance.BattleTechGame.Simulation;
                if (__instance.weaponDef.Type == WeaponType.COIL && !__instance.parent.SprintedLastRound
                    && (__instance.parent.JumpedLastRound && !sim.CombatConstants.ResolutionConstants.COILUsesJumping))
                {
                    __result = __result * __instance.parent.EvasivePipsCurrent;
                }
            }
        }
    }

    public static class TAG
    {
        //Allows Tag to enable called shot
        [HarmonyPatch(typeof(SelectionStateFire), "NeedsCalledShot", MethodType.Getter)]
        public static class SelectionStateFire_NeedsCalledShot_Patch
        {
            public static void Postfix(SelectionStateFire __instance, ref bool __result)
            {
                if (Core.Settings.Tagged_Called_Shots && __instance.TargetedCombatant != null)
                {
                    var isTagged = __instance.TargetedCombatant.Combat.EffectManager.GetAllEffectsTargeting(__instance.TargetedCombatant)
                        .Any(x => x.EffectData.Description.Name == "TAG MARKED");
                    if (isTagged)
                        __result = true;
                }
            }
        }


        [HarmonyPatch(typeof(AbstractActor), "IsVulnerableToCalledShots")]
        public static class AbstractActor_IsVulnerableToCalledShots_Patch
        {
            public static void Postfix(AbstractActor __instance, ref bool __result)
            {
                if (Core.Settings.Tagged_Called_Shots)
                {
                    var combat = UnityGameInstance.BattleTechGame.Combat;
                    var isTagged = combat.EffectManager.GetAllEffectsTargeting(__instance)
                    .Any(x => x.EffectData.Description.Name == "TAG MARKED");
                    if (isTagged)
                        __result = true;
                }
            }
        }
    }
}


//        //Area that makes streaks not consume ammo upon firing.
//        public static class Streaks
//    {
//        [HarmonyPatch(typeof(MissileLauncherEffect), "AllMissilesComplete")]
//        public static class MissileLauncherEffect_AllMissilesComplete
//        {
//            public static void Postfix(MissileLauncherEffect __instance, ref bool __result)
//            {
//                __result = true;
//                bool flag = __instance.currentState != WeaponEffect.WeaponEffectState.Firing && __instance.currentState != WeaponEffect.WeaponEffectState.WaitingForImpact;
//                if (flag)
//                {
//                    __result = false;
//                }
//                for (int i = 0; i < __instance.missiles.Count; i++)
//                {
//                    bool flag2 = !__instance.weapon.componentDef.ComponentTags.Contains("component_streak") || __instance.hitInfo.DidShotHitChosenTarget(i);
//                    if (flag2)
//                    {
//                        bool flag3 = !__instance.missiles[i].FiringComplete;
//                        if (flag3)
//                        {
//                            __result = false;
//                        }
//                    }
//                }
//            }
//        }

//        [HarmonyPatch(typeof(MissileLauncherEffect), "LaunchMissile")]
//        public static class MissileLauncherEffect_LaunchMissile
//        {
//            public static bool Prefix(MissileLauncherEffect __instance, ref int ___emitterIndex, ref float ___rate, bool ___isIndirect, float ___firingIntervalRate, float ___t, AkGameObj ___parentAudioObject)
//            {
//                bool flag = __instance.weapon.componentDef.ComponentTags.Contains("component_streak");
//                bool result;
//                if (flag)
//                {
//                    bool flag2 = __instance.hitInfo.DidShotHitChosenTarget(__instance.hitIndex);
//                    if (flag2)
//                    {
//                        MissileEffect missileEffect = __instance.missiles[__instance.hitIndex];
//                        missileEffect.tubeTransform = __instance.weaponRep.vfxTransforms[___emitterIndex];
//                        missileEffect.Fire(__instance.hitInfo, __instance.hitIndex, ___emitterIndex, ___isIndirect);
//                    }
//                    else
//                    {
//                        for (int i = 0; i < __instance.weapon.ammoBoxes.Count; i++)
//                        {
//                            bool flag3 = __instance.weapon.ammoBoxes[i].CurrentAmmo < __instance.weapon.ammoBoxes[i].AmmoCapacity;
//                            if (flag3)
//                            {
//                                __instance.weapon.ammoBoxes[i].StatCollection.ModifyStat<int>(__instance.weapon.uid, 0, "CurrentAmmo", StatCollection.StatOperation.Int_Add, 1, -1, true);
//                                i = __instance.weapon.ammoBoxes.Count;
//                            }
//                        }
//                    }
//                    bool flag4 = __instance.hitIndex == 0 && __instance.hitInfo.DidShotHitChosenTarget(0);
//                    if (flag4)
//                    {
//                        bool isSRM = __instance.isSRM;
//                        if (isSRM)
//                        {
//                            WwiseManager.PostEvent<AudioEventList_srm>(AudioEventList_srm.srm_launcher_start, ___parentAudioObject, null, null);
//                        }
//                        else
//                        {
//                            WwiseManager.PostEvent<AudioEventList_lrm>(AudioEventList_lrm.lrm_launcher_start, ___parentAudioObject, null, null);
//                        }
//                    }
//                    __instance.hitIndex++;
//                    ___emitterIndex++;
//                    ___rate = ___firingIntervalRate;
//                    bool flag5 = ___rate < 0.01f;
//                    if (flag5)
//                    {
//                        Traverse.Create(__instance).Method("FireNextMissile", new object[0]).GetValue();
//                    }
//                    result = false;
//                }
//                else
//                {
//                    result = true;
//                }
//                return result;
//            }
//        }

//        [HarmonyPatch(typeof(AttackDirector.AttackSequence), "GetIndividualHits")]
//        public static class AttackDirectorAttackSequence_GetIndividualHits
//        {
//            // Token: 0x06000044 RID: 68 RVA: 0x0000447C File Offset: 0x0000267C
//            public static bool Prefix(AttackDirector.AttackSequence __instance, ref WeaponHitInfo hitInfo, int groupIdx, int weaponIdx, Weapon weapon, float toHitChance, float prevDodgedDamage)
//            {
//                try
//                {
//                    bool flag = weapon != null && weapon.weaponDef != null;
//                    if (flag)
//                    {
//                        bool flag2 = weapon.weaponDef.ComponentTags.Contains("component_streak");
//                        if (flag2)
//                        {
//                            bool isLogEnabled = AttackDirector.hitLogger.IsLogEnabled;
//                            if (isLogEnabled)
//                            {
//                                AttackDirector.hitLogger.Log(string.Format("???????? RANDOM HIT ROLLS (GetIndividualHits): Weapon Group: {0} // Weapon: {1}", groupIdx, weaponIdx));
//                            }
//                            hitInfo.toHitRolls = __instance.GetRandomNumbers(groupIdx, weaponIdx, hitInfo.numberOfShots);
//                            for (int i = 1; i < hitInfo.toHitRolls.Length; i++)
//                            {
//                                hitInfo.toHitRolls[i] = hitInfo.toHitRolls[0];
//                            }
//                            bool isLogEnabled2 = AttackDirector.hitLogger.IsLogEnabled;
//                            if (isLogEnabled2)
//                            {
//                                AttackDirector.hitLogger.Log(string.Format("???????? RANDOM LOCATION ROLLS (GetIndividualHits): Weapon Group: {0} // Weapon: {1}", groupIdx, weaponIdx));
//                            }
//                            hitInfo.locationRolls = __instance.GetRandomNumbers(groupIdx, weaponIdx, hitInfo.numberOfShots);
//                            bool isLogEnabled3 = AttackDirector.hitLogger.IsLogEnabled;
//                            if (isLogEnabled3)
//                            {
//                                AttackDirector.hitLogger.Log(string.Format("???????? DODGE ROLLS (GetIndividualHits): Weapon Group: {0} // Weapon: {1}", groupIdx, weaponIdx));
//                            }
//                            hitInfo.dodgeRolls = __instance.GetRandomNumbers(groupIdx, weaponIdx, hitInfo.numberOfShots);
//                            hitInfo.hitVariance = __instance.GetVarianceSums(groupIdx, weaponIdx, hitInfo.numberOfShots, weapon);
//                            AbstractActor abstractActor = __instance.chosenTarget as AbstractActor;
//                            Team team = (weapon != null && weapon.parent != null && weapon.parent.team != null) ? weapon.parent.team : null;
//                            float num = __instance.attacker.CalledShotBonusMultiplier;
//                            for (int j = 0; j < hitInfo.numberOfShots; j++)
//                            {
//                                float value = Traverse.Create(__instance).Method("GetCorrectedRoll", new object[]
//                                {
//                                    hitInfo.toHitRolls[j],
//                                    team
//                                }).GetValue<float>();
//                                bool flag3 = value <= toHitChance;
//                                bool flag4 = team != null;
//                                if (flag4)
//                                {
//                                    team.ProcessRandomRoll(toHitChance, flag3);
//                                }
//                                bool flag5 = false;
//                                bool flag6 = abstractActor != null;
//                                if (flag6)
//                                {
//                                    flag5 = abstractActor.CheckDodge(__instance.attacker, weapon, hitInfo, j, __instance.IsBreachingShot);
//                                }
//                                bool flag7 = flag3 && flag5;
//                                if (flag7)
//                                {
//                                    hitInfo.dodgeSuccesses[j] = true;
//                                    __instance.FlagAttackContainsDodge(abstractActor.GUID);
//                                }
//                                else
//                                {
//                                    hitInfo.dodgeSuccesses[j] = false;
//                                }
//                                bool isLogEnabled4 = AttackDirector.attackLogger.IsLogEnabled;
//                                if (isLogEnabled4)
//                                {
//                                    string message = string.Format("SEQ:{0}: WEAP:{1} SHOT:{2} Roll Value: {3}", new object[]
//                                    {
//                                        __instance.id,
//                                        weaponIdx,
//                                        j,
//                                        value
//                                    });
//                                    AttackDirector.attackLogger.Log(message);
//                                }
//                                bool flag8 = flag3 && !flag5;
//                                if (flag8)
//                                {
//                                    hitInfo.hitLocations[j] = __instance.chosenTarget.GetHitLocation(__instance.attacker, __instance.attackPosition, hitInfo.locationRolls[j], __instance.calledShotLocation, num);
//                                    num = Mathf.Lerp(num, 1f, __instance.Director.Combat.Constants.HitTables.CalledShotBonusDegradeLerpFactor);
//                                    bool isLogEnabled5 = AttackDirector.attackLogger.IsLogEnabled;
//                                    if (isLogEnabled5)
//                                    {
//                                        AttackDirector.attackLogger.Log(string.Format("SEQ:{0}: WEAP:{1} SHOT:{2} Hit! Location: {3}", new object[]
//                                        {
//                                            __instance.id,
//                                            weaponIdx,
//                                            j,
//                                            hitInfo.hitLocations[j]
//                                        }));
//                                    }
//                                    bool isLogEnabled6 = AttackDirector.hitminLogger.IsLogEnabled;
//                                    if (isLogEnabled6)
//                                    {
//                                        AttackDirector.hitminLogger.Log(string.Format("WEAPON: {0} - SHOT: {1} Hits! ////// HEX VAL {2}", weapon.Name, j, hitInfo.hitLocations[j]));
//                                    }
//                                    hitInfo.hitQualities[j] = __instance.Director.Combat.ToHit.GetBlowQuality(__instance.attacker, __instance.attackPosition, weapon, __instance.chosenTarget, __instance.meleeAttackType, __instance.IsBreachingShot);
//                                    __instance.FlagShotHit();
//                                }
//                                else
//                                {
//                                    hitInfo.hitLocations[j] = 0;
//                                    bool isLogEnabled7 = AttackDirector.attackLogger.IsLogEnabled;
//                                    if (isLogEnabled7)
//                                    {
//                                        AttackDirector.attackLogger.Log(string.Format("SEQ:{0}: WEAP:{1} SHOT:{2} Miss!", __instance.id, weaponIdx, j));
//                                    }
//                                    bool isLogEnabled8 = AttackDirector.hitminLogger.IsLogEnabled;
//                                    if (isLogEnabled8)
//                                    {
//                                        AttackDirector.hitminLogger.Log(string.Format("WEAPON: {0} - SHOT: {1} Misses!", weapon.Name, j));
//                                    }
//                                    __instance.FlagShotMissed();
//                                }
//                                hitInfo.hitPositions[j] = __instance.chosenTarget.GetImpactPosition(__instance.attacker, __instance.attackPosition, weapon, ref hitInfo.hitLocations[j], ref hitInfo.attackDirections[j], ref hitInfo.secondaryTargetIds[j], ref hitInfo.secondaryHitLocations[j]);
//                            }
//                            return false;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Logger.Error(ex);
//                }
//                return true;
//            }
//        }
//    }
//}
