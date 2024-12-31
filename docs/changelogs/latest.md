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

### Tile Tracker Changes

- Tracked Krobus' hiding bush. #359
- Moved animals with produce or pettable animals to pending category. Also changed the translation to prefix harvestable and/or pettable accordingly.
- Moved dropped animal produces (eggs, truffles, rabbit's foot & duck feathers) in ready category.

### Guides And Docs


### Misc

- The machines are now tracked by their qualified item ids instead of their names.

### Translation Changes

- Modified(en.ftl): `npc-farm_animal_info` = [English value](https://github.com/khanshoaib3/stardew-access/blob/ad211b0ae16d7a3bf91eb822befb2660d28a1aea/stardew-access/i18n/en.ftl#L339-L360)

### Development Chores

- General code cleanup
- Updated refs for v1.6.15

