using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ProstheticNoMissingBodyParts;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    public static readonly List<string> FootDefNames =
    [
        "Foot",
        "LeftMechanicalFoot",
        "RightMechanicalFoot",
        "ATR_MechanicalFoot"
    ];

    public static readonly List<string> HandDefNames =
    [
        "Hand",
        "LeftMechanicalHand",
        "RightMechanicalHand",
        "ATR_MechanicalHand"
    ];

    public static readonly List<string> LegDefNames =
    [
        "Leg",
        "LeftMechanicalLeg",
        "RightMechanicalLeg",
        "ATR_MechanicalLeg"
    ];

    public static readonly List<string> ShoulderDefNames =
    [
        "Shoulder",
        "LeftMechanicalShoulder",
        "RightMechanicalShoulder",
        "ATR_MechanicalShoulder"
    ];

    static HarmonyPatches()
    {
        var harmony = new Harmony("com.prostheticnomissingbodyparts.rimworld.mod");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}