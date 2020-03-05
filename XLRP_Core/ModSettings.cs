using System.Collections.Generic;
using BattleTech;


namespace XLRP_Core
{
    public class ModSettings
    {
        public bool Debug = false;
        public string modDirectory;

        //New settings
        public bool UpgradeDegradedOpFor = false;
        public bool JumpStopsCalledShot = false;
        public bool UpgradeItems = false;
        public bool UpgradePilots = false;
        public bool ShowAllArgoUpgrades = true;
        public bool ObfuscateEventRequirements = false;
        public bool COIL_Heat_Multiply_EP = false;

        //Old settings
        public float BulwarkMalus = -1f;
        public float SimSpotterDistance = 100.0f;
        public float SimSensorDistance = 100.0f;
        public int NormalMedTechSkill = 6;
        public int NormalMechTechSkill = 3000;

        public float CSMasteryNerf = 0.85f;
        public int LessResolveAtMaxMorale = 5;

        public float ArtemisIVBonus = 1.5f;

    }
}
