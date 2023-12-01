Now Ported over and rebuild for tModLoader 1.4!
Ophioid is a mod focused on a boss and a multi-phase rematch later on that cuts down on soul griding in Hardmode after the Mech Bosses.
Source code available on github (1.4 branch https://github.com/IDGCaptainRussia94/Ophioid-Mod/tree/1.4 )
Please see https://forums.terraria.org/index.php?threads/the-ophioid-mod.73083/ for more info on the mod
Terraria Mods Wiki page: https://terrariamods.fandom.com/wiki/Ophioid

With the introduction of the 1.4 Port, some things have changed, see V2.15

Current Content:
- 2 summoning items for both evil biomes and 1 for the rematch fight
- 2 (2nd boss has 3 phases) Bosses (With trophy and mask and a pet item!)

Version History :

V2.30
- Updated to tModLoader v2022.10
- Removed Corruption/Crimson restriction on Dead Fungusbug/Living Carrion. You can now use either to summon the 1st Ophiopede.
- Dead Fungusbug and Living Carrion now Shimmer Transmute into each other.
- Boss Summon items are now sorted with other boss summon items when sorting chests or in Journey Mode research menu.
- All items in the mod that are stackable now stack to 9999.
- Redid the loot on the bosses to make it easier to understand. Updated the Treasure Bag to match.
- Ophiopede no longer drops the alternate boss summon item.
- The bosses can now be summoned in multiplayer! There are still some minor issues, but at least it works now.

V2.29
- Ported to 1.4.4
- Updated Treasure Bag
- Fixed Ophiopede Mask not counting as vanity.
- Fixed offset on Ophiopede Mask
- Phase 1 Ophiopede now plays the custom music instead of Boss 2
- Increased the volume of the music
- Gave the boss bar a shield if the bosses are invulnerable.

V2.28
- Nothing changed, just recompiled to work on the July, 2022 build of 1.4 TML

V2.27
- Updated Treasure Bag
- Added Journey Mode researching requirements on all items.
- Re-added the Boss Checklist images to Boss Checklist
- Added Boss Relics for both Ophiopede and Ophiofly
- Added Music Boxes for both of the songs in the mod
- Added a Master Mode pet dropped by Ophiofly
- Spore Infested Egg now sells for 1 gold
- Ophioid Mask now sells for 75 silver
- Refactored the code

V2.26
- Updated PlaySound
- Updated Boss Checklist to the new format

V2.25
- Updated IEntitySource
- Fixed some Bestiary entries not unlocking

V2.24
- Added new IEntitySource
- Made the Bestiary better by hiding some NPCs and decorating the boss pages
- Nerfed the amount of money tier 1 Ophiopede drops (1.25 Platinum -> 50 Gold, which is still a lot)
- Ophiofly now drops 1 Platinum

V2.23
- Fixed all tile related things that broke in the recent tML update.
- Updated Boss Checklist stuff
- Custom boss health bar!

V2.22

- Fixed TagCompound saving
- Boss Checklist support has been re-enabled

V2.21

-Removed obsolete TML code, mod now works again
-Music now works, yay!

V2.15
Is now running on TML 1.4! The following has changed, going forward or only for now:
-The bosses drop variable amounts of items and even more in Expert due to how 1.4 handles item drops now, getting a solid 50 items to drop BEFORE luck I haven't figured out yet.
-There is NO netcode support, all NetCode methods in 1.4 TML appear to be broken and inaccessible.
-Music is currently busted in TML, so the mod's OST cannot be heard yet
-Boss Checklist and Yet Another Boss Healthbar Support has been disabled
-IDGLib is no longer required by the mod
-Projectiles don't make you Stinky
-Any other shenanigans 1.4 might have introduced are very likely

V2.13
Got rid of the troublesome FindNPCsMultitype errors, I hope

V2.11
Ophiodpede drops his items at the body segment closest to who he was targeting

V2.10
Proper Boss-checklist support!

V2.09
Fly minions now fly off when the boss is not longer alive
Ophiodpede (rematch) becames invulnerable at 5% HP, and his body segments are now properly buffed

V2.08
Made it so the Worm phase cannot despawn as long as atleast player is alive for it to target

V2.07
This isn't really an update to the mod, but rather an update to information about the mod. The mod's homepage has been moved to my github as the Terraria forums staff will not give me control over the mod's thread without Kranot's approval (who practically doesn't exist at this point so you see the problem here), so I am unable to update any of out of date info on the mod's homepage.
So now the homepage links to a wiki page on my github, where you can find UPDATED info on the mod as well as other things you may want to know about, the forum page is still a valid place to leave feedback for me as I still read there, but this update is really just a PSA.

V2.06
So remember when I said there wouldn't be a treasure bag? Well as it turns out Kranot DID make one and I forgot to add it, so here you go! It only drops from the rematch however
Did a sort of hard-counter to make the music play properly
Discovered a bug, it's rare and how to cause it: by having no NPCs in your world. Something that's bound to not happen anyways, but if there are no NPCs, the boss with spawn with only 1 segment. So if you have this problem, this is why.


V2.04
Added unfinished music tracks for the rematch, read below why they are unfinished and for an annoucement.
The Fly phase was aparently missing its custom health bar support from YABHB, fixed.

V2.03
The Worm phase has received a slightly stronger velocity cap when charging at the player. Furthermore, when airborne, if Ophoiopede is too far away from the player, his X velocity will slow down and he will become less likely to despawn

V2.01
Updated to the new TMODLoader

V2.0
-Alrighty here it is! The big update that finally adds the part 2 of the fight!
-just be warning while you can fight it after golem, it's tougher than you may be prepared for...
-I'll be rolling out minor updates with feedback and bug fixes as I discover them, thank you for waiting... WAY too long for this!
-Fixed the Infested Compost not working in a Crimson World

V1.94
-Fixed messed up body sprites, the WIP version of the boss has been buffed...

V1.93
-Is now linked with GitHub! Link is on the 2nd post in the homepage.

V1.07
-Fixed Projectiles not appearing for clients in network games

V1.05
-Mod release

-Disclaimer
So some time ago Kranot and I stopped talking and I recently cut contact with him all together, So there won't be any more sprites being made (by him atleast), and that means no more content outside of what was planned.
I don't think he even knows I finished 'his' mod, as he likes to view it.
I have been working on a content mod I am very passionate about called SGAmod and am looking for spriters
Check it out, if you like Ophioid-Mod, you'll like SGAmod <3