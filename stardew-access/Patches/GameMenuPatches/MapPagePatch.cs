using HarmonyLib;
using StardewValley.Menus;

namespace stardew_access.Patches;

// TODO Figure out why the map page isn't detected in IClickableMenuPatch::DrawPatch()
public class MapPagePatch : IPatch
{
    public void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(MapPage), "draw"),
            postfix: new HarmonyMethod(typeof(MapPagePatch), nameof(DrawPatch))
        );
    }
    
    private static void DrawPatch(MapPage __instance)
    {
        try
        {
            MainClass.ScreenReader.SayWithMenuChecker(__instance.hoverText, true);
        }
        catch (Exception e)
        {
            Log.Error($"An error occurred in map page patch:\n{e.Message}\n{e.StackTrace}");
            throw;
        }
    }
}