using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace stardew_access.Patches;

internal class SocialPagePatch : IPatch
{
    public void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(SocialPage), "draw"),
            postfix: new HarmonyMethod(typeof(SocialPagePatch), nameof(DrawPatch))
        );
    }

    private static void DrawPatch(SocialPage __instance, List<ClickableTextureComponent> ___sprites,
        int ___slotPosition)
    {
        try
        {
            int x = Game1.getMouseX(true), y = Game1.getMouseY(true); // Mouse x and y position
            for (var i = ___slotPosition; i < ___slotPosition + 5 && i < ___sprites.Count; i++)
            {
                if (!__instance.characterSlots[i].bounds.Contains(x, y))
                    continue;
                var entry = __instance.GetSocialEntry(i);

                var name = entry.Character.displayName;
                var has_talked = !entry.IsMet ? 2 : Game1.player.hasPlayerTalkedToNPC(entry.Character.Name) ? 1 : 0;
                var relationship_status = GetRelationshipStatus(entry) ?? "";
                var gifts_this_week = entry.Friendship?.GiftsThisWeek ?? 0;
                var heart_level = entry.HeartLevel;
                if (entry.IsPlayer)
                {
                    MainClass.ScreenReader.TranslateAndSayWithMenuChecker("menu-social_page-player_info", true, new
                    {
                        name,
                        relationship_status
                    });
                }
                else
                {
                    MainClass.ScreenReader.TranslateAndSayWithMenuChecker("menu-social_page-npc_info", true, new
                    {
                        name,
                        has_talked,
                        relationship_status,
                        heart_level,
                        gifts_this_week
                    });
                }

                break;
            }
        }
        catch (Exception e)
        {
            Log.Error($"An error occurred in social page patch:\n{e.Message}\n{e.StackTrace}");
        }
    }

    private static string? GetRelationshipStatus(SocialPage.SocialEntry entry)
    {
        // Copied (and modified) from StardewValley.Menus.SocialPage.DrawNPCSlot
        var gender = entry.Gender;
        var isDatable = entry.IsDatable;
        var currentPlayer = entry.IsMarriedToCurrentPlayer();
        var isRoommate = entry.IsRoommateForCurrentPlayer();
        var isDating = entry.IsDatingCurrentPlayer();
        var isCurrentSpouse = entry.IsMarriedToCurrentPlayer();
        string? relationshipStatus = null;
        
        if (!entry.IsPlayer)
        {
            if (!(isDatable | isRoommate)) return relationshipStatus;
            
            relationshipStatus = !isRoommate
                ? !currentPlayer
                    ? !entry.IsMarriedToAnyone()
                        ? !(!Game1.player.isMarriedOrRoommates() & isDating)
                            ? !entry.IsDivorcedFromCurrentPlayer()
                                ? gender == Gender.Female
                                    ? Game1.content.LoadString(
                                        "Strings\\StringsFromCSFiles:SocialPage_Relationship_Single_Female")
                                    : Game1.content.LoadString(
                                        "Strings\\StringsFromCSFiles:SocialPage_Relationship_Single_Male")
                                : gender == Gender.Female
                                    ? Game1.content.LoadString(
                                        "Strings\\StringsFromCSFiles:SocialPage_Relationship_ExWife")
                                    : Game1.content.LoadString(
                                        "Strings\\StringsFromCSFiles:SocialPage_Relationship_ExHusband")
                            : gender == Gender.Female
                                ? Game1.content.LoadString(
                                    "Strings\\StringsFromCSFiles:SocialPage_Relationship_Girlfriend")
                                : Game1.content.LoadString(
                                    "Strings\\StringsFromCSFiles:SocialPage_Relationship_Boyfriend")
                        : gender == Gender.Female
                            ? Game1.content.LoadString(
                                "Strings\\UI:SocialPage_Relationship_MarriedToOtherPlayer_FemaleNpc")
                            : Game1.content.LoadString(
                                "Strings\\UI:SocialPage_Relationship_MarriedToOtherPlayer_MaleNpc")
                    : gender == Gender.Female
                        ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Wife")
                        : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Husband")
                : gender == Gender.Female
                    ? Game1.content.LoadString(
                        "Strings\\StringsFromCSFiles:SocialPage_Relationship_Housemate_Female")
                    : Game1.content.LoadString(
                        "Strings\\StringsFromCSFiles:SocialPage_Relationship_Housemate_Male");

            return relationshipStatus;
        }

        var farmer = (Farmer)entry.Character;
        relationshipStatus = !Game1.content.ShouldUseGenderedCharacterTranslations()
            ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Single_Female")
            : gender == Gender.Male
                ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Single_Female")
                    .Split('/')[0]
                : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Single_Female")
                    .Split('/').Last();
        if (isCurrentSpouse)
            relationshipStatus = gender == Gender.Male
                ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Husband")
                : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Wife");
        else if (farmer.isMarriedOrRoommates() && !farmer.hasRoommate())
            relationshipStatus = gender == Gender.Male
                ? Game1.content.LoadString("Strings\\UI:SocialPage_Relationship_MarriedToOtherPlayer_MaleNpc")
                : Game1.content.LoadString("Strings\\UI:SocialPage_Relationship_MarriedToOtherPlayer_FemaleNpc");
        else if (!Game1.player.isMarriedOrRoommates() && entry.IsDatingCurrentPlayer())
            relationshipStatus = gender == Gender.Male
                ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Boyfriend")
                : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_Girlfriend");
        else if (entry.IsDivorcedFromCurrentPlayer())
            relationshipStatus = gender == Gender.Male
                ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_ExHusband")
                : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage_Relationship_ExWife");

        return relationshipStatus;
    }
}