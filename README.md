# Elin Action Hook mod

TBD

## Events

| EventType | SubType | Phase |
|-----------|---------|-------|
| `EnterZone` | `Nefia`, `PCFaction` | `Before`, `After` |
| `Sleep` | none | `Before` |
| `GoDownStairs` | none | `Before` |
| `GoUpStairs` | none | `Before` |
| `StartCrafting` | `Carpentry`, `Blacksmith`, `Alchemy`, `Sculpture`, `Jewelry`, `Weaving`, `Handicraft`, `Cooking`, `Reading` | `Before` |

* `EnterZone`: Triggered when entering a zone.
  * `Before` phase is triggered only when the player is entering a zone from the global map.
  * `After` phase is triggered for other cases, such as entering a zone with stairs.
* `Sleep`: Triggered when the player sleeps from the hotbar.
* `GoDownStairs`: Triggered when the player goes down stairs.
  * For `After` phase, please use `EnterZone` event with `Nefia` `SubType`.
* `GoUpStairs`: Triggered when the player goes up stairs.
  * For `After` phase, please use `EnterZone` event with `Nefia` `SubType`.
* `StartCrafting`: Triggered when the player starts crafting.

## Actions

| ActionType | ActionArgs |
|------------|------------|
| `ShowMessage`      | none |
| `ChangeEquipment` | number |
| `ChangeToolbelt` | number/number |
| `Save`     | none |

* `ShowMessage`: Show a message with a popup.
* `ChangeEquipment`: Change the equipment set with a preset on the hotbar.
  * `ActionArgs` is a one-based index of the hotbar slot to change.
* `ChangeToolbelt`: Change the selected item on the toolbelt.
  * `ActionArgs` has two numbers separated by a slash (`/`):
    - The first number is the page of the toolbelt (1 or 2).
    - The second number is the index of the item on the toolbelt (1-9).

## Build

First, put `Directory.Build.props` with the following content in the root directory of the project.
Change the `ElinGamePath` to the path of your Elin installation.

```xml
<Project>
  <PropertyGroup>
    <ElinGamePath>C:\Program Files (x86)\Steam\steamapps\common\Elin</ElinGamePath>
  </PropertyGroup>
</Project>
```

Then, run the following command to build the project.

```console
dotnet build
```
