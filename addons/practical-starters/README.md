# Practical starter guns and ammunition

Optional single-player GAMMA DLTX configuration. Adds 12 selectable starting guns using existing installed items: Glock 17, Beretta Modern, USP, MP5, PP-2000, UMP45, MP-133, Winchester 1300, AKS-74U, AK-74, AKM BAS and SVT-40. Existing point budgets and faction gear remain in effect.

The separate ammo patch sets matching starting grants by caliber: 180 rounds for the configured pistol/SMG calibers, 150 for intermediate calibers, and 60 for the configured full-power and shotgun calibers. The native game grants ammo per selected weapon, so selecting two weapons with the same caliber grants both allocations. These settings apply to a new-game loadout; they do not grant items to an existing character.

## Installation and dependencies

Use a backed-up copy of the installation and saves. In that copy's Mod Organizer, create a new mod containing this `gamedata` folder and give it higher priority than other starting-loadout overrides. Launch through that Mod Organizer and inspect a new-game selection before starting a test campaign.

Requires [GAMMA](https://github.com/Grokitach/Stalker_GAMMA), its installed weapon/item definitions, the native new-game selection and ammunition-grant logic, and its DLTX-capable engine. No weapons, models, icons, sounds or animations are bundled.

## Evidence and test limits

Static checks resolved all 12 gun references, ammo classes, and inheritance across 13 faction loadouts. Tests of the native starting-ammo loop covered 52 weapon entries, including two selected weapons sharing a caliber. A full new-campaign gameplay check remains outstanding. Point costs and resupply economy still need player feedback.

**Co-op: experimental and unverified.** The launcher does not enable these patches. Match both files, versions, priorities and settings on host and clients before any trial. Confirm that the co-op engine actually uses the same new-game selection and ammo logic.

## Provenance

Both files are new original DLTX entries generated for this project. They refer to installed item identifiers; they do not copy the original loadout tables or redistribute the underlying weapon content.
