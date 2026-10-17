using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ProstheticNoMissingBodyParts;

[DefOf]
public class ModDefOf
{
    static ModDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(ModDefOf));

    public static BodyPartGroupDef LeftHand;
    public static BodyPartGroupDef RightHand;
    public static BodyPartGroupDef Hands;
    public static BodyPartGroupDef Feet;
    public static BodyPartDef Shoulder;
    public static BodyPartDef Leg;

    public static List<BodyPartDef> FootDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<BodyPartDef>(HarmonyPatches.FootDefNames);

            return field;
        }
        private set;
    }


    public static List<BodyPartDef> HandDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<BodyPartDef>(HarmonyPatches.HandDefNames);

            return field;
        }
        private set;
    }

    public static List<BodyPartDef> LegDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<BodyPartDef>(HarmonyPatches.LegDefNames);

            return field;
        }
        private set;
    }

    public static List<BodyPartDef> ShoulderDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<BodyPartDef>(HarmonyPatches.ShoulderDefNames);

            return field;
        }
        private set;
    }

    public static List<HediffDef> ArmsAllowHediffDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<HediffDef>(ProstheticNoMissingBodyPartsMod.settings.ArmsWhitelist);

            return field;
        }
        private set;
    }

    public static List<HediffDef> FeetAllowHediffDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<HediffDef>(ProstheticNoMissingBodyPartsMod.settings.FeetWhitelist);

            return field;
        }
        private set;
    }

    public static List<HediffDef> HandsAllowHediffDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<HediffDef>(ProstheticNoMissingBodyPartsMod.settings.HandsWhitelist);

            return field;
        }
        private set;
    }

    public static List<HediffDef> LegsAllowHediffDefs
    {
        get
        {
            field ??= BodyPartUtils.ResolveDefs<HediffDef>(ProstheticNoMissingBodyPartsMod.settings.LegsWhitelist);

            return field;
        }
        private set;
    }

    public static void resetCaches()
    {
        FootDefs = null;
        HandDefs = null;
        LegDefs = null;
        ShoulderDefs = null;
        ArmsAllowHediffDefs = null;
        LegsAllowHediffDefs = null;
        FeetAllowHediffDefs = null;
        HandsAllowHediffDefs = null;
        LegsAllowHediffDefs = null;
    }
}
