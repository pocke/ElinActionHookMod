# Elin Action Hook mod

This mod allows you to hook into game events in Elin and execute actions.

For example, you can configure the following:

* Switch to combat equipment set before entering a Nefia.
* Cast Magic Map and activate a Detector after entering a Nefia and moving down the stairs.
* Switch to crafting equipment set before starting crafting.

## Configuration

To use this mod, place a `hooks.csv` file in the directory where this mod is installed.
The format of this file is as follows:

```csv
EventType,SubType,Phase,ActionType,ActionArgs
EnterZone,PCFaction,Before,ChangeEquipment,6
```

Example CSV files are available in the `sample/` directory. Copy and edit this file in your installation directory.

This CSV file defines pairs of events and actions. Details are described below.

### Events

Events define "when to execute an action." The `EventType`, `SubType`, and `Phase` columns in the CSV correspond to this.

* `EventType`: Specifies the type of event.
* `SubType`: Specifies the subtype of the event.
  * Depending on the `EventType`, you may need to specify detailed conditions in `SubType`.
* `Phase`: Specifies whether the event occurs before (`Before`) or after (`After`).

The list of `EventType`s and the possible `SubType` and `Phase` values are as follows:

| EventType      | SubType                | Phase             |
|----------------|-----------------------|-------------------|
| `EnterZone`    | Specify zone type      | `Before`, `After` |
| `Sleep`        | Not specified          | `Before` only     |
| `GoDownStairs` | Not specified          | `Before` only     |
| `GoUpStairs`   | Not specified          | `Before` only     |
| `StartCrafting`| Specify skill          | `Before` only     |

Details for each event are as follows.

#### `EnterZone`

Triggered when entering a new zone (when the map changes).

Possible values for `Phase`:

* `Before`
  * Triggered just before entering the zone.
  * Only triggered when entering a zone from the global map.
* `After`
  * Triggered after entering the zone.
  * Also triggered in cases other than from the global map, such as after using stairs in a Nefia.

Possible values for `SubType` (zone type):

* `Nefia`
  * Triggered when entering a Nefia.
* `PCFaction`
  * Triggered when entering a zone belonging to the player's faction.

#### `Sleep`

Triggered when the player sleeps from the hotbar.

Only `Before` can be specified for `Phase`.
`SubType` cannot be specified.

#### `GoDownStairs`, `GoUpStairs`

Triggered when the player goes down or up stairs.

Only `Before` can be specified for `Phase`. For the equivalent of `After`, use the `EnterZone` event.
`SubType` cannot be specified.

#### `StartCrafting`

Triggered when the player starts crafting.
This includes crafting facilities such as carpenter's table and processing equipment such as sawmill.

Only `Before` can be specified for `Phase`.
Specify the skill used by the crafting facility in `SubType`. The available skills are as follows:

* `Carpentry`: Carpentry
* `Blacksmith`: Blacksmith
* `Alchemy`:  Alchemy
* `Sculpture`: Sculpting
* `Jewelry`: Jewelry
* `Weaving`: Weaving
* `Handicraft`: Crafting
* `Cooking`: Cooking
* `Reading`: Literacy

### Actions

Actions define "what to do when an event occurs." The `ActionType` and `ActionArgs` columns in the CSV correspond to this.

* `ActionType`: Specifies the type of action to execute.
* `ActionArgs`: Specifies the arguments required for the action.
  * If the action does not require arguments, leave `ActionArgs` empty.

The list of `ActionType`s and the possible `ActionArgs` values are as follows:

| ActionType         | ActionArgs           |
|--------------------|---------------------|
| `ShowMessage`      | Not specified       |
| `ChangeEquipment`  | Number              |
| `ChangeToolbelt`   | Number/Number       |
| `Save`             | Not specified       |

Details for each action are as follows.

#### `ShowMessage`

Displays a message in a popup. Mainly used for debugging purposes.

Do not specify `ActionArgs`.

#### `ChangeEquipment`

Changes the equipment set using the hotbar widget preset.

Specify the hotbar slot number (starting from 1) in `ActionArgs`.
For example, to use the slot corresponding to the `F6` key, specify `6`.

#### `ChangeToolbelt`

Selects an item from the toolbelt.

Specify two numbers separated by a slash (`/`) in `ActionArgs`.
The first number is the toolbelt page number (1 or 2; switched with the control key).
The second number is the item position in the toolbelt (1-9).

#### `Save`

Saves the game.

Do not specify `ActionArgs`.

## Tips

* If the same event is used in multiple rows, actions are executed in order from the top.
* If there is an error in the CSV file, an error will be displayed in a popup in the game and in `Player.log`.
* If you register a spell on the toolbelt, you can cast it when that item is selected.
