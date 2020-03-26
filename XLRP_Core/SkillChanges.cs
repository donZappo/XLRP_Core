using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using Localize;
using BattleTech.UI;

namespace XLRP_Core.SkillChanges
{
    public static class CalledShotMods
    {
        //Disable called shots for mechs that have jumped during the same turn.
        [HarmonyPatch(typeof(CombatHUDMechwarriorTray), "ResetMechwarriorButtons")]
        public static class CombatHUDMechwarriorTray_ResetMechwarriorButtons_Patch
        {
            public static void Postfix(CombatHUDMechwarriorTray __instance, AbstractActor actor)
            {
                if (actor == null || actor.UnitType != UnitType.Mech || __instance.MoraleButtons[0] == null)
                    return;

                if (Core.Settings.JumpStopsCalledShot && actor.HasJumpedThisRound && actor.SkillTactics < Core.Settings.TacticsForJumpedCS)
                {
                    __instance.MoraleButtons[0].DisableButton();
                    __instance.MoraleButtons[0].isAutoHighlighted = false;
                }
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
