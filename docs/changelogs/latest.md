## Changelog

### New Features


### Feature Updates


### Bug Fixes


### Tile Tracker Changes


### Guides And Docs

- Added a fishing guide to the docs. This explains how to fish as a blind Stardew Access player and offers recommendations and resources to make the fishing minigame less painful.
- added a features showcase and installation section with links to setup instructions and Accessible Stardew Setup to readme.md

### Misc

- Added new api methods:
    - SpeakHoveredClickableComponentsFromList()
    - SpeakClickableComponent()
    - SpeakHoveredOptionsElementSlot()
    - SpeakHoveredOptionsElementFromList()
    - SpeakOptionsElement()
- The methods used for making custom menus accessible automatically have been improved, given that the menus follow certain guidelines.

### Translation Changes


### Development Chores

- Removed manual patches to options page, language selection and mine elevator menus as the improvement to IClickableMenuPatch is sufficient for them.
- Added an interface for the mod's api, IStardewAccessApi.
- Added 3 utility classes: ClickableComponentUtils, OptionsElementUtils and CommonUIButton

