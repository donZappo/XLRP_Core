using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using Localize;

namespace XLRP_Core.SkillChanges
{
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
