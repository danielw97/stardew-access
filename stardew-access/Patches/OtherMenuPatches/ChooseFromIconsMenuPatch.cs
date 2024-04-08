using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace stardew_access.Patches;

internal class ChooseFromIconsMenuPatch : IPatch
{
    public void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ChooseFromIconsMenu), "draw"),
            postfix: new HarmonyMethod(typeof(ChooseFromIconsMenuPatch), nameof(ChooseFromIconsMenuPatch.DrawPatch))
        );
    }

    private static void DrawPatch(ChooseFromIconsMenu __instance)
    {
        try
        {
            int x = Game1.getMouseX(true), y = Game1.getMouseY(true); // Mouse x and y position
            foreach (var icon in __instance.icons)
            {
                if (icon is not { visible: true } || !icon.containsPoint(x, y)) continue;

                MainClass.ScreenReader.SayWithMenuChecker(icon.hoverText, true);
            }
        }
        catch (Exception e)
        {
            Log.Error($"An error occurred in choose from icons menu patch:\n{e.Message}\n{e.StackTrace}");
        }
    }
}
