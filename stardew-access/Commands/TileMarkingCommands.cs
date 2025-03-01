using Microsoft.Xna.Framework;
using stardew_access.Patches;
using stardew_access.Translation;
using stardew_access.Utils;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;
using StardewValley.TokenizableStrings;

namespace stardew_access.Commands;

public class TileMarkingCommands
{
    public static void BuildList(string[] args, bool fromChatBox = false)
    {
        Farm farm = (Farm)Game1.getLocationFromName("Farm");
        Netcode.NetCollection<Building> buildings = farm.buildings;
        int buildingIndex = 0;

        List<string> buildingInfos = [];
        foreach (var building in buildings)
        {
            BuildingOperations.availableBuildings[buildingIndex] = building;
            buildingInfos.Add(Translator.Instance.Translate("commands-tile_marking-build_list-building_info", new
            {
                index = buildingIndex,
                name = TokenParser.ParseText(building.GetData().Name),
                x_position = building.tileX.Value,
                y_position = building.tileY.Value
            }, translationCategory: TranslationCategory.CustomCommands));
            ++buildingIndex;
        }
        var toPrint = string.Join("\n", buildingInfos);
        string text;

        if (string.IsNullOrWhiteSpace(toPrint))
        {
            text = Translator.Instance.Translate("commands-tile_marking-build_list-no_building", translationCategory: TranslationCategory.CustomCommands);
        }
        else
        {
            text = Translator.Instance.Translate("commands-tile_marking-build_list-buildings_list", new
            {
                building_infos = toPrint
            }, translationCategory: TranslationCategory.CustomCommands);
        }

        if (fromChatBox) Game1.chatBox.addInfoMessage(text);
        else Log.Info(text);
    }

    public static void MarkPosition_mark(string[] args, bool fromChatBox = false)
    {
        string text;
        if (Game1.currentLocation is not Farm)
        {
            text = Translator.Instance.Translate("commands-tile_marking-mark-not_in_farm",
                translationCategory: TranslationCategory.CustomCommands);
            goto PrintText;
        }

        string? indexInString = args.ElementAtOrDefault(0);
        if (indexInString == null)
        {
            text = Translator.Instance.Translate("commands-tile_marking-mark-index_not_entered",
                translationCategory: TranslationCategory.CustomCommands);
            goto PrintText;
        }

        bool isParsable = int.TryParse(indexInString, out int index);

        if (!isParsable || !(index is >= 0 and <= 9))
        {
            text = Translator.Instance.Translate("commands-tile_marking-mark-wrong_index",
                translationCategory: TranslationCategory.CustomCommands);
            goto PrintText;
        }

        BuildingOperations.marked[index] = new Vector2(Game1.player.Tile.X, Game1.player.Tile.Y);
        text = Translator.Instance.Translate("commands-tile_marking-mark-location_marked", new
        {
            x_position = Game1.player.Tile.X,
            y_position = Game1.player.Tile.Y,
            index
        }, translationCategory: TranslationCategory.CustomCommands);

    PrintText:
        if (fromChatBox) Game1.chatBox.addInfoMessage(text);
        else Log.Info(text);
    }

    public static void ListMarked_marklist(string[] args, bool fromChatBox = false)
    {
        List<string> markInfos = [];
        for (int i = 0; i < BuildingOperations.marked.Length; i++)
        {
            if (BuildingOperations.marked[i] == Vector2.Zero) continue;

            markInfos.Add(Translator.Instance.Translate("commands-tile_marking-mark_list-mark_info", new
            {
                index = i,
                x_position = BuildingOperations.marked[i].X,
                y_position = BuildingOperations.marked[i].Y
            }, translationCategory: TranslationCategory.CustomCommands));
        }
        var toPrint = string.Join("\n", markInfos);
        string text;

        if (string.IsNullOrWhiteSpace(toPrint))
        {
            text = Translator.Instance.Translate("commands-tile_marking-mark_list-not_marked", translationCategory: TranslationCategory.CustomCommands);
        }
        else
        {
            text = Translator.Instance.Translate("commands-tile_marking-mark_list-marks_list", new
            {
                mark_infos = toPrint
            }, translationCategory: TranslationCategory.CustomCommands);
        }

        if (fromChatBox) Game1.chatBox.addInfoMessage(text);
        else Log.Info(text);
    }

    public static void SelectBuilding_buildsel(string[] args, bool fromChatBox = false)
    {
        string text = "";

        if ((Game1.activeClickableMenu is not CarpenterMenu && Game1.activeClickableMenu is not PurchaseAnimalsMenu &&
             Game1.activeClickableMenu is not AnimalQueryMenu) || (!CarpenterMenuPatch.isOnFarm &&
                                                                   !PurchaseAnimalsMenuPatch.isOnFarm &&
                                                                   !AnimalQueryMenuPatch.isOnFarm))
        {
            text = Translator.Instance.Translate("commands-tile_marking-build_sel-cannot_select",
                translationCategory: TranslationCategory.CustomCommands);
            goto PrintText;
        }

        string? indexInString = args.ElementAtOrDefault(0);
        if (indexInString == null)
        {
            text = Translator.Instance.Translate("commands-tile_marking-build_sel-building_index_not_entered",
                translationCategory: TranslationCategory.CustomCommands);
            goto PrintText;
        }

        bool isParsable = int.TryParse(indexInString, out int index);

        if (!isParsable)
        {
            text = Translator.Instance.Translate("commands-tile_marking-build_sel-wrong_index",
                translationCategory: TranslationCategory.CustomCommands);
            goto PrintText;
        }

        string? positionIndexInString = args.ElementAtOrDefault(1);
        int positionIndex = 0;

        if (CarpenterMenuPatch.isMoving)
        {
            if (CarpenterMenuPatch.isConstructing || CarpenterMenuPatch.isMoving)
            {
                if (BuildingOperations.availableBuildings[index] == null)
                {
                    text = Translator.Instance.Translate("commands-tile_marking-build_sel-no_building_found",
                        new { index }, translationCategory: TranslationCategory.CustomCommands);
                    goto PrintText;
                }

                if (positionIndexInString == null)
                {
                    text = Translator.Instance.Translate("commands-tile_marking-build_sel-marked_index_not_entered",
                        translationCategory: TranslationCategory.CustomCommands);
                    goto PrintText;
                }

                isParsable = int.TryParse(positionIndexInString, out positionIndex);

                if (!isParsable)
                {
                    text = Translator.Instance.Translate("commands-tile_marking-build_sel-wrong_index",
                        translationCategory: TranslationCategory.CustomCommands);
                    goto PrintText;
                }
            }
        }
        else if (CarpenterMenuPatch.isConstructing && !CarpenterMenuPatch.isUpgrading)
        {
            if (BuildingOperations.marked[index] == Vector2.Zero)
            {
                text = Translator.Instance.Translate("commands-tile_marking-build_sel-no_marked_position_found",
                    new { index }, translationCategory: TranslationCategory.CustomCommands);
                goto PrintText;
            }
        }
        else
        {
            if (BuildingOperations.availableBuildings[index] == null)
            {
                text = Translator.Instance.Translate("commands-tile_marking-build_sel-no_building_found",
                    new { index }, translationCategory: TranslationCategory.CustomCommands);
                goto PrintText;
            }
        }

        string? response = null;

        if (Game1.activeClickableMenu is PurchaseAnimalsMenu)
        {
            BuildingOperations.PurchaseAnimal(BuildingOperations.availableBuildings[index]);
        }
        else if (Game1.activeClickableMenu is AnimalQueryMenu)
        {
            BuildingOperations.MoveAnimal(BuildingOperations.availableBuildings[index]);
        }
        else
        {
            if (CarpenterMenuPatch.isConstructing && !CarpenterMenuPatch.isUpgrading)
            {
                response = BuildingOperations.Construct(BuildingOperations.marked[index]);
            }
            else if (CarpenterMenuPatch.isMoving)
            {
                response = BuildingOperations.Move(BuildingOperations.availableBuildings[index],
                    BuildingOperations.marked[positionIndex]);
            }
            else if (CarpenterMenuPatch.isDemolishing)
            {
                response = BuildingOperations.Demolish(BuildingOperations.availableBuildings[index]);
            }
            else if (CarpenterMenuPatch.isUpgrading)
            {
                response = BuildingOperations.Upgrade(BuildingOperations.availableBuildings[index]);
            }
            else if (CarpenterMenuPatch.isPainting)
            {
                response = BuildingOperations.Paint(BuildingOperations.availableBuildings[index]);
            }
        }

        if (response != null)
        {
            text = response;
        }

    PrintText:
        if (string.IsNullOrWhiteSpace(text)) return;
        if (fromChatBox) Game1.chatBox.addInfoMessage(text);
        else Log.Info(text);
    }
}
