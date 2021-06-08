using Harmony;
using BattleTech;
using BattleTech.UI;

namespace BTR_Core.SkillChanges
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
}
