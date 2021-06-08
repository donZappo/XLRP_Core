namespace BTR_Core
{
    public class ModSettings
    {
        public bool Debug = false;
        public string modDirectory;

        //New settings
        public bool UpgradeDegradedOpFor = false;
        public bool JumpStopsCalledShot = false;
        public bool IsFriendlyBugSuppression = false;
        public int TacticsForJumpedCS = 10;

        public bool RemoveSpottingExploit = true;

        public bool UpgradeItems = false;
        public float EliteRareUpgradeChance = 0;
        public float VeryRareUpgradeChance = 0;
        public float RareUpgradeChance = 0;
        public float UpgradeChanceDivisor = 0;
        public float[] EliteRareUpgradeLevel = { 0 };
        public float[] VeryRareUpgradeLevel = { 0 };
        public float[] RareUpgradeLevel = { 0 };

        public float EliteRareWeaponChance = 0;
        public float VeryRareWeaponChance = 0;
        public float RareWeaponChance = 0;
        public float WeaponChanceDivisor = 0;
        public float[] EliteRareWeaponLevel = { 0 };
        public float[] VeryRareWeaponLevel = { 0 };
        public float[] RareWeaponLevel = { 0 };

        public bool UpgradePilots = false;
        public bool ShowAllArgoUpgrades = true;
        public bool ObfuscateEventRequirements = false;
        public bool COIL_Heat_Multiply_EP = false;
        public bool Tagged_Called_Shots = false;
        public bool NerfContractPayments = false;
        public double NerfExponent = 0.9;

        //Old settings
        public float BulwarkMalus = -1f;
        public float SimSpotterDistance = 100.0f;
        public float SimSensorDistance = 100.0f;
        public int NormalMedTechSkill = 6;
        public int NormalMechTechSkill = 3000;

        public float CSMasteryNerf = 0.85f;
        public int LessResolveAtMaxMorale = 5;

        public float ArtemisIVBonus = 1.5f;
        public bool correctMeleeMultipliers = true;

    }
}
