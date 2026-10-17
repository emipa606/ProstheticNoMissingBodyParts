using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ProstheticNoMissingBodyParts;

[Harmony]
[HarmonyPatch(typeof(ApparelUtility), nameof(ApparelUtility.HasPartsToWear))]
internal static class ApparelUtility_HasPartsToWear
{
    public static void Postfix(ref bool __result, Pawn p, ThingDef apparel)
    {
        if (__result)
        {
            return;
        }

        // *****
        // ProstheticNoMissingBodyParts patch code start
        // *****

        // this code run only if original Rimworld check return false (body part is missed)
        // and we try to fix this, if pawn has a bionic part in right place (Bionic Arm, Bionic Leg, etc...)


        // prepare bool set for apparel body part groups
        var isLeftHand = false;
        var isRightHand = false;
        var isHands = false;
        var isFeet = false;

        foreach (var groupDef in apparel.apparel.bodyPartGroups)
        {
            if (groupDef == ModDefOf.LeftHand)
            {
                isLeftHand = true;
                break;
            }

            if (groupDef == ModDefOf.RightHand)
            {
                isRightHand = true;
                break;
            }

            if (groupDef == ModDefOf.Hands)
            {
                isHands = true;
                break;
            }

            if (groupDef == ModDefOf.Feet)
            {
                isFeet = true;
                break;
            }

            var body = p.def.race.body.AllParts;

            if (BodyPartUtils.ExistsByGroupAndParent(body, ModDefOf.Shoulder, groupDef))
            {
                isHands = true;
                break;
            }

            if (BodyPartUtils.ExistsByGroupAndParent(body, ModDefOf.Leg, groupDef))
            {
                isFeet = true;
                break;
            }

            continue;
        }

        var hediffs = p.health.hediffSet.hediffs;
        // check if apparel needed left/right hand, especially for jewelry mod, pawns try to wear bracer... on fingers... omg...
        if (isLeftHand || isRightHand)
        {
            var part = isLeftHand ? ModDefOf.LeftHand : ModDefOf.RightHand;
            foreach (var h in hediffs)
            {
                if (h.Part?.def == null || h.def == null)
                {
                    continue;
                }

                // true if arm replaced with whitelisted bionic part
                if (ModDefOf.ShoulderDefs.Contains(h.Part.def))
                {
                    if (ModDefOf.ArmsAllowHediffDefs.Contains(h.def) && BodyPartUtils.ExistsDeep(h.Part, part))
                    {
                        __result = true;
                        return;
                    }
                    continue;
                }

                // true if hand replaced with whitelisted bionic part
                if (ModDefOf.HandDefs.Contains(h.Part.def))
                {
                    if (ModDefOf.HandsAllowHediffDefs.Contains(h.def) && BodyPartUtils.ExistsDeep(h.Part, part))
                    {
                        __result = true;
                        return;
                    }
                    continue;
                }
            }

            return;
        }

        // check if apparel needed hands, useful for gloves
        if (isHands)
        {
            foreach (var h in hediffs)
            {
                if (h.Part?.def == null || h.def == null)
                {
                    continue;
                }

                // true if arm replaced with whitelisted bionic part
                if (ModDefOf.ShoulderDefs.Contains(h.Part.def))
                {
                    if (ModDefOf.ArmsAllowHediffDefs.Contains(h.def))
                    {
                        __result = true;
                        return;
                    }
                    continue;
                }

                // true if hand replaced with whitelisted bionic part
                if (ModDefOf.HandDefs.Contains(h.Part.def))
                {
                    if (ModDefOf.HandsAllowHediffDefs.Contains(h.def))
                    {
                        __result = true;
                        return;
                    }
                    continue;
                }
            }

            return;
        }

        // check if apparel needed feet, useful for boots
        if (isFeet)
        {
            foreach (var h in hediffs)
            {
                if (h.Part?.def == null || h.def == null)
                {
                    continue;
                }

                // true if arm replaced with whitelisted bionic part
                if (ModDefOf.LegDefs.Contains(h.Part.def))
                {
                    if (ModDefOf.LegsAllowHediffDefs.Contains(h.def))
                    {
                        __result = true;
                        return;
                    }
                    continue;
                }

                // true if hand replaced with whitelisted bionic part
                if (ModDefOf.FootDefs.Contains(h.Part.def))
                {
                    if (ModDefOf.FeetAllowHediffDefs.Contains(h.def))
                    {
                        __result = true;
                        return;
                    }
                    continue;
                }
            }

            return;
        }

        // *****
        // ProstheticNoMissingBodyParts patch code end
        // *****

        // body part still missed

        __result = false;
    }
}
