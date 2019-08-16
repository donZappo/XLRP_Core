using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using UnityEngine;
using BattleTech.Framework;
using BattleTech.UI;
using BattleTech.Save;

namespace Extended_CE
{
    class BugFixes
    {
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
    }
}
