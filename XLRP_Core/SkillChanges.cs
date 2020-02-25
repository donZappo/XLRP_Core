using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using Localize;

namespace XLRP_Core.SkillChanges
{
    public static class CalledShotMods
    {
        [HarmonyPatch(typeof(AbstractActor), "CanUseOffensivePush")]
        public static class AbstractActor_CanUseOffensivePush_Patch
        {
            public static void Postfix(AbstractActor __instance, ref bool __result)
            {
                if (__instance.HasJumpedThisRound && Core.Settings.JumpStopsCalledShot)
                    __result = false;
            }
        }
    }
    //// If we are adding a floatie for Ace Pilot because we moved after shooting, change it to Master Tactician because of our skill changes
    //[HarmonyPatch(typeof(MessageCenter), "PublishMessage")]
    //public static class MessageCenter_PublishMessage
    //{
    //    public static void Prefix(ref MessageCenterMessage message)
    //    {
    //        if (message is FloatieMessage floatie)
    //        {
    //            if(floatie.text.ToString() == "ACE PILOT")
    //            {
    //                floatie.SetText(new Text("MASTER TACTICIAN", new object[0]));
    //            }
    //        }
    //    }
    //}
}
