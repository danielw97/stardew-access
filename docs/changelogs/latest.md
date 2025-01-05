## Changelog

### New Features

- Enabled braille output.
- Added machines from the mod [Cornucopia - Artisan Machines](https://www.nexusmods.com/stardewvalley/mods/24842)

### Feature Updates

When "Sort By Proximity" is enabled, the ObjectTracker will focus on the closest item after map updates. E.G., after harvesting a specific crop, focus moves to the next closest crop, rather than the next closest crop of the same kind. Sorting alphabetically retains the old behavior.

### Bug Fixes

- Fix LoadGameMenu keyboard navigation broken by Stardew Valley 1.6.15 update.
- Fix keyboard navigation breaks after clicking any back button from a submenu of title screen, such as Load Game or New Game menus.
- Fixed empty fish ponds being prefixed with "Error Item"; #424
- Fixed social page reading tokenized string in place of relationship status.
- Fixed map page not being read.
ObjectTracker autorefresh is fixed! No more mashing the home button to force a refresh.
Garden Pots have been moved from the "Other" category to the "Pending" category to better reflect their role.
When holding a crop, pots will stay in "Pending" if they are unwatered. They will move to "Crops" once watered.
When holding a bush such as a tea sapling, pots will appear in "Bushes".
When any pot can be harvested, it will appear in "Ready".
- Fixed a bug where the categories of certain items were not being applied correctly from `stardew-access/assets/TileData/QualifiedItemIds.json`
- Removed incorrect door sound that plays when changing maps with grid movement on even when no door has been passed through.

### Tile Tracker Changes

- Tracked Krobus' hiding bush. #359
- Moved animals with produce or pettable animals to pending category. Also changed the translation to prefix harvestable and/or pettable accordingly.
- Moved dropped animal produces (eggs, truffles, rabbit's foot & duck feathers) in ready category.
- Added new forageables category
- Moved major forageable items into forageables category
- Moved various items into new categories to better reflect their role as part of a category sweep

### Guides And Docs


### Misc

- The machines are now tracked by their qualified item ids instead of their names.

### Translation Changes

- Modified(en.ftl): `npc-farm_animal_info` = [English value](https://github.com/khanshoaib3/stardew-access/blob/ad211b0ae16d7a3bf91eb822befb2660d28a1aea/stardew-access/i18n/en.ftl#L339-L360)

### Development Chores

- General code cleanup
- Updated refs for v1.6.15

