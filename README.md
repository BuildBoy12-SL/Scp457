# SCP-457

Adds an active SCP into the game. It spawns in place of another SCP subject when the round starts based off of the configurable spawn chance. SCP-457 has passive, primary, and ability based actions. Designed to be a glass cannon, it has a lower health pool and high damage by default.

# Installation

**[EXILED](https://github.com/Exiled-Team/EXILED) must be installed for this to work.**


### Passive

The passive is the ability to inflict the Burned effect at a configurable radius, which makes every afflicted user take more damage from any sources.

### Primary

The primary fire is a long range line which is optionally indicated by a tracer. This attack inflicts afterburn and can optionally pierce players.

### Ability

The ability is strictly command based. Given the name "combustion", an explosion will occur and every user currently afflicted with the passive ability will be flashed and receive an initial damage burst plus afterburn.

# Configs
## Main
| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| is_enabled | Boolean | True | Whether the plugin should load. |
| show_debug | Boolean | False | Whether debug messages should show. |

## Attack
Configs for the main attack of Scp457, which overrides the default Scp0492 swing attack.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| damage | Float | 40 | The amount of inflicted damage. |
| distance | Float | 150 | The maximum attack range. |
| burn_duration | Float | 10 | The duration, in seconds, of the applied burning effect. |
| show_attack | Boolean | True | Indicates whether a line should be drawn when Scp457 attacks. |
| orb_spacing | Float | 1 | The spacing of the drawn markers. |

## Burn
Configs for the afterburn caused by being hit by one of Scp457's attacks

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| damage | Float | 5 | The amount of dealt damage per tick. |
| tick_duration | Float | 1 | The seconds between each tick. |
| maximum_duration | Float | 20 | The maximum amount of seconds a user can be on fire. |
| healed_by | ItemType Array | Medkit, SCP500 | A collection of items that can heal a burn. |

## Combust
Configs for the combustion ability.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| cooldown | Integer | 30 | The amount of seconds between uses. |
| damage | Float | 15 | The initial damage of the explosion. |
| burn_duration | Float | 10 | The duration, in seconds, of the applied burning effect. |
| flash_duration | Float | 2 | Whether a Scp035 item instance will spawn when a Scp035 host dies. |
| cooldown_message | String | <color=red>Wait %seconds% second(s) to use that command again.<\/color> | The message to be sent to a user who is on cooldown. |
| used_message | String | <color=green>Done.<\/color> | The message to be sent when combustion executes successfully. |

## Scp457
Configs for the core aspects of Scp457.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| health | Integer | 1200 | The base health of the Scp. |
| spawn_chance | Float | 20 | The percentage chance that Scp457 will spawn in place of another Scp. |
| size | Vector | X: 1.1, Y: 1.15, Z: 1.1 | The scale of Scp457. |
| spawn_door | String | HCZ_ARMORY | The name of the door that the Scp will spawn in. |
| burn_radius | Float | 30 | The radius around Scp457 where players who have line of sight will have the burned effect applied. |
| spawn_message | String | You have spawned as <color=red>SCP-457<\/color>\nKill everyone. | The message to be displayed to a Scp457 when they spawn. |
| spawn_message_duration | Unsigned Short | 10 | The amount of time that the spawn message is displayed. |
| badge | String | <color=#990000>Scp-457<\/color> | The message to be shown where Scp457s role would normally be. |
