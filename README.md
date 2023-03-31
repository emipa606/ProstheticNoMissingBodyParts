# ProstheticNoMissingBodyParts

![Image](https://i.imgur.com/buuPQel.png)

Update of iamrespawns mod
https://steamcommunity.com/sharedfiles/filedetails/?id=2103563425

- Replaced the destructive Harmony prefix with a postfix instead to increase compatibility
- Added settings for hand and feet-bionics
- Verified working in CE
- Added support for https://steamcommunity.com/sharedfiles/filedetails/?id=1386412863]Android tiers.

![Image](https://i.imgur.com/pufA0kM.png)

	
![Image](https://i.imgur.com/Z4GOv8H.png)

This mod allow your pawns with bionic arms and legs continue wearing any gear for feet and hands.

Works with vanilla, EPOE, VAE, Jewelry, AVP (gauntlets), etc. Does not require new save game. Can be removed any time.

This mod does not change body part groups and do not try to connect fingers and hands to torso directly like as [KV] Keep Hands and Feet - 1.1

Instead of this, this mod patch original Rimworld ApparelUtility method "HasPartsToWear" on the fly. 
If your pawn has two bionic legs, it cannot wear boots by default. This happens cause game remove all feet, when you try to install bionic upgrade on your legs.
When original game code return that pawn does not have any body part for wearing boots or gloves (negative check), patch code starts and try to find any bionic body part on upper levels, like Bionic Arm or Bionic Leg. 
If any bionic part exists in right place, patch change original result value to positive. And your pawn can continue wearing any gear on their hands, feet, etc...

**Allowed arm replacement**:
- SimpleProstheticArm
- BionicArm
- AdvancedBionicArm
- ArchotechArm

**Allowed leg replacement**:
- SimpleProstheticLeg
- BionicLeg
- AdvancedBionicLeg
- ArchotechLeg
- MuscleStimulatorLegs

**Why not all replacements are allowed by default?**
For example you could still wearing you gloves with Bionic Arms, but not with some kind of simple prosthetic, like wooden hand. Can you wear gloves on wooden hand? I think no. For balance purpose only. You can change settings of allowed replacement any time in mod settings. And yes, you can allow all replacement, if you want this. Reloading does not required.

Please, locate this mod in the end part of your mod list. After all othe mods that adds bionic parts.

Compatibility
- Must be compatible with all other mod, exclude mods that do the same patch as this mod.
- Tested with 100+ mods, including EPOE, VAE, Jewelry, AVP (gauntlets), etc... 
- Does not test with any CE mods, cause I does not use CE in my games.
- HarmonyLib Required.

Source Code
https://github.com/RimworldFixes/ProstheticNoMissingBodyParts

Non Steam Version
https://github.com/RimworldFixes/ProstheticNoMissingBodyParts/releases

**Why I write this mod?**
When I install advanced bionic arm (leg) on my pawn, it took off his boots and went to stockpile. Then other pawn do the same. And other. And other, but with gloves. What the hell. I tried to find a mod that could solve this problem. And... Did not find. I try to install something like [KV] Keep Hands and Feet - 1.1, but it not works. I don`t know why.
Then same happens with Jewelry with bracelets and mood debuff... It was the last straw. That's why this mod exists.

For people who hate that pawns can't wear boots with bionic upgrades.

![Image](https://i.imgur.com/PwoNOj4.png)



-  See if the the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using https://steamcommunity.com/workshop/filedetails/?id=818773962]HugsLib and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.



https://steamcommunity.com/sharedfiles/filedetails/changelog/2739055353]Last updated 2023-03-31
