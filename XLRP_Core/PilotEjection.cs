﻿using System;
using Harmony;
using BattleTech;

namespace BTR_Core
{
    //Make ejecting dangerous
   [HarmonyPatch(typeof(Pilot), "EjectPilot")]
    public static class Pilot_Ejection_Patch
    {
        static void Postfix(Pilot __instance)
        {
            try
            {
                int injuries = 1;
                var cockpit = __instance.ParentActor.allComponents.Find(x => x.componentDef.ComponentTags.Contains("Cockpit"));
                var rand = new System.Random();
                var chance = rand.NextDouble();
                if (cockpit.componentDef.ComponentTags.Contains("standard_cockpit"))
                {
                    if (chance < 0.25)
                        injuries = 2;
                    else if (chance < 0.75)
                        injuries = 1;
                    else
                        injuries = 0;
                }
                else if (cockpit.componentDef.ComponentTags.Contains("safe_cockpit"))
                {
                    if (chance < 0.5)
                        injuries = 1;
                    else
                        injuries = 0;
                }
                else if (cockpit.componentDef.ComponentTags.Contains("dangerous_cockpit"))
                {
                    if (chance < 0.125)
                        injuries = 3;
                    else if (chance < 0.5)
                        injuries = 2;
                    else if (chance < 0.875)
                        injuries = 1;
                    else
                        injuries = 0;
                }

                __instance.InjurePilot("ejection", 0, injuries, DamageType.Combat, null, null);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
