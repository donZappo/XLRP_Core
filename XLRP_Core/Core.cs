using System;
using System.Collections;
using System.Reflection;
using Harmony;
using Newtonsoft.Json;
using static XLRP_Core.Logger;
using BattleTech;
using BattleTech.UI;

namespace XLRP_Core
{
    //public enum DifficultySetting
    //{
    //    Normal = 0,
    //    Hard = 21,
    //    Simulation = 42
    //}

    public static class Core
    {
        #region Init

        public static void Init(string modDir, string settings)
        {
            var harmony = HarmonyInstance.Create("XLRP-Core.Misc.Fixes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            // read settings
            try
            {
                Settings = JsonConvert.DeserializeObject<ModSettings>(settings);
                Settings.modDirectory = modDir;
            }
            catch (Exception)
            {
                Settings = new ModSettings();
            }

            // blank the logfile
            Clear();
            PrintObjectFields(Settings, "Settings");
        }
        // logs out all the settings and their values at runtime
        internal static void PrintObjectFields(object obj, string name)
        {
            LogDebug($"[START {name}]");

            var settingsFields = typeof(ModSettings)
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var field in settingsFields)
            {
                if (field.GetValue(obj) is IEnumerable &&
                    !(field.GetValue(obj) is string))
                {
                    LogDebug(field.Name);
                    foreach (var item in (IEnumerable)field.GetValue(obj))
                    {
                        LogDebug("\t" + item);
                    }
                }
                else
                {
                    LogDebug($"{field.Name,-30}: {field.GetValue(obj)}");
                }
            }

            LogDebug($"[END {name}]");
        }

        #endregion

        internal static ModSettings Settings;
    }

    //[HarmonyPatch(typeof(SimGameState), "Dehydrate")]
    //public static class SimGameState_Dehydrate_Patch
    //{
    //    public static void Prefix(SimGameState __instance)
    //    {
    //        if (!__instance.CompanyTags.Contains("Extended_CE_Initialized"))
    //        {
    //            var stats = __instance.CompanyStats;
    //            var MedTechSkillChange = __instance.Constants.Story.StartingMedTechSkill - Core.Settings.NormalMedTechSkill;
    //            stats.ModifyStat<int>("SimGame", 0, "MedTechSkill", StatCollection.StatOperation.Int_Add, MedTechSkillChange, -1, true);

    //            var MechTechSkillChange = __instance.Constants.Story.StartingMechTechSkill - Core.Settings.NormalMechTechSkill;
    //            stats.ModifyStat<int>("SimGame", 0, "MechTechSkill", StatCollection.StatOperation.Int_Add, MechTechSkillChange, -1, true);
    //            __instance.CompanyTags.Add("Extended_CE_Initialized");
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(SimGameDifficultySettingsModule), "SaveSettings")]
    //public static class SGDSM_SaveSettings_Patch
    //{
    //    public static int Difficulty;
    //    public static int Morale;
    //    public static int MechTechSkill;
    //    public static int MedTechSkill;

    //    public static void Prefix()
    //    {
    //        try
    //        {
    //            StoryConstantsDef simConstants = UnityGameInstance.BattleTechGame.Simulation.Constants.Story;
    //            SimGameState sim = UnityGameInstance.BattleTechGame.Simulation;
    //            Difficulty = simConstants.MaximumDebt;
    //            Morale = simConstants.StartingMorale;
    //            MechTechSkill = simConstants.StartingMechTechSkill;
    //            MedTechSkill = simConstants.StartingMedTechSkill;
    //        }
    //        catch { }
    //    }
    //    public static void Postfix()
    //    {
    //        try
    //        {
    //            StoryConstantsDef simConstants = UnityGameInstance.BattleTechGame.Simulation.Constants.Story;
    //            SimGameState sim = UnityGameInstance.BattleTechGame.Simulation;
    //            if (sim.CompanyTags.Contains("Extended_CE_Initialized") && Difficulty != simConstants.MaximumDebt)
    //            {
    //                var MoraleChange = simConstants.StartingMorale - Morale;
    //                var MechTechSkillChange = simConstants.StartingMechTechSkill - MechTechSkill;
    //                var MedTechSkillChange = simConstants.StartingMedTechSkill - MedTechSkill;

    //                var stats = sim.CompanyStats;
    //                stats.ModifyStat<int>("SimGame", 0, "Morale", StatCollection.StatOperation.Int_Add, MoraleChange, -1, true);
    //                stats.ModifyStat<int>("SimGame", 0, "MechTechSkill", StatCollection.StatOperation.Int_Add, MechTechSkillChange, -1, true);
    //                stats.ModifyStat<int>("SimGame", 0, "MedTechSkill", StatCollection.StatOperation.Int_Add, MedTechSkillChange, -1, true);
    //                sim.RoomManager.RefreshDisplay();
    //            }
    //        }
    //        catch { }
    //    }
    //}
}
