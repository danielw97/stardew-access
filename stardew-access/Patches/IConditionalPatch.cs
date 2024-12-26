using StardewModdingAPI;

 namespace stardew_access.Patches;

internal interface IConditionalPatch : IPatch
{
    bool ShouldApply(IModHelper helper);
}
