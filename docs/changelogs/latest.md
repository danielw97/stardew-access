## Changelog

### New Features

- New keybind, `ReadFlooringKey`. An alternate to `flooring` command. (unset by default); #403
- New command, `refut`, to refresh user tiles file while in-game; #404

### Feature Updates

- Allow tile viewer in purchasing and moving animal menus.
- [Tile Viewer] Add continuous cursor movement when keys are held down. (Doesn't work with precision movement)

### Bug Fixes

- Updated priorities of widget/components fixing #398 (Should be removed from final changelog as it was caused from previous pr)
- fixed new categories not being loaded because of a bug in  `CATEGORY::AddNewCategory`.
- [Tile Viewer Builder Menus] Fixed indefinite panning of viewport when cursor reaches map's edges; #309
- Fix categories for certain items:
    - Moved workbench and feed hopper to interactable category
    - Fixed empty feeding benches showing up in other category
    - Moved storage furniture (dressers) to containers

### Tile Tracker Changes

- Retrieve tile information on maps via `TileDesc` tile property. This property can be used in custom maps to describe undetected tiles.
- Tracked anvil and solar panel as machines

### Guides And Docs

- Added a fishing guide to the docs. This explains how to fish as a blind Stardew Access player and offers recommendations and resources to make the fishing minigame less painful.
- added a features showcase and installation section with links to setup instructions and Accessible Stardew Setup to readme.md
- broke out docs/guides.md into docs/guides with several files containing guides on various aspects of playing Stardew Valley
- modified relevant page links in all docs files to point to the new guides subdirectory
- added official guide heading to the readme to give users easy access to the new guides
- - created docs/guides/guides-home.md to serve as the main page for all guides
- created docs/guides/mining-guide.md for a future mining guide in progress
- added the fishing guide back into docs/guides/general-guides.md
- added guides for the mines, quarry mine, skull cavvern, and relevant ancillary info to docs/guides/mining-guides.md

### Misc

- Added new api methods:
    - SpeakHoveredClickableComponentsFromList()
    - SpeakClickableComponent()
    - SpeakHoveredOptionsElementSlot()
    - SpeakHoveredOptionsElementFromList()
    - SpeakOptionsElement()
- The methods used for making custom menus accessible automatically have been improved, given that the menus follow certain guidelines.
- Exposed `PrimaryInfoKey` to the public api. This can be used in custom menus to speak extra info on key press for 3rd party mods.
- Speak HUD messages even if player isn't free to move.
- Add `Active` suffix to pressed pressure pads

### Translation Changes

- New(commands.en.ftl): `commands-other-refresh_user_tiles` = `User tiles refreshed!`
- Modified(en.ftl): `tile-volcano_dungeon-pressure_pad` = [Update English Value](https://github.com/khanshoaib3/stardew-access/blob/2fca1477b37a2671fce4b707c906d9c5b4313ac2/stardew-access/i18n/en.ftl#L246-L249)

### Development Chores

- Removed manual patches to options page, language selection and mine elevator menus as the improvement to IClickableMenuPatch is sufficient for them.
- Added an interface for the mod's api, IStardewAccessApi.
- Added 3 utility classes: ClickableComponentUtils, OptionsElementUtils and CommonUIButton
- [Features] The skipping condition (`Context.IsPlayerFree`) is moved into the `Update()` method of each feature (from `MainClass::OnUpdateTicked()`). This results in more control over when to skip the update logic.
- Updated binary files in ref/ used for build workflow.

