﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;

namespace XLRP_Core.SpecialContractSalvage
{
    //public static class BountyHunterContractReward
    //{
    //    internal static bool AllowOnePieceOfLosTechBountyHunter = true;

    //    // Allow you to salvage 1, and only 1 Double Heat Sink off of the Bounty Hunter
    //    [HarmonyPatch(typeof(Contract), "AddMechComponentToSalvage")]
    //    public static class Contract_AddMechComponentToSalvage
    //    {
    //        public static void Postfix(Contract __instance, MechComponentDef def, SimGameConstants sc, ref List<SalvageDef> ___finalPotentialSalvage)
    //        {
    //            try
    //            {
    //                if (__instance.Name == "Tables Turned" &&
    //                    AllowOnePieceOfLosTechBountyHunter &&
    //                    def.ComponentTags.Contains("BLACKLISTED") &&
    //                    def.ComponentTags.Contains("component_type_lostech"))
    //                {
    //                    SalvageDef salvageDef = new SalvageDef
    //                    {
    //                        MechComponentDef = def,
    //                        Description = new DescriptionDef(def.Description),
    //                        RewardID = __instance.GenerateRewardUID(),
    //                        Type = SalvageDef.SalvageType.COMPONENT,
    //                        ComponentType = def.ComponentType,
    //                        Damaged = false,
    //                        Weight = sc.Salvage.DefaultComponentWeight,
    //                        Count = 1
    //                    };

    //                    ___finalPotentialSalvage.Add(salvageDef);
    //                    //Traverse.Create(__instance).Field("finalPotentialSalvage").GetValue<List<SalvageDef>>().Add(salvageDef);

    //                    AllowOnePieceOfLosTechBountyHunter = false;
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                Logger.Error(e);
    //            }
    //        }
    //    }
    //}
}
