using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ProstheticNoMissingBodyParts;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    public static readonly List<string> FootDefNames = new List<string>
    {
        "Foot",
        "LeftMechanicalFoot",
        "RightMechanicalFoot",
        "ATR_MechanicalFoot"
    };

    public static readonly List<string> HandDefNames = new List<string>
    {
        "Hand",
        "LeftMechanicalHand",
        "RightMechanicalHand",
        "ATR_MechanicalHand"
    };

    public static readonly List<string> LegDefNames = new List<string>
    {
        "Leg",
        "LeftMechanicalLeg",
        "RightMechanicalLeg",
        "ATR_MechanicalLeg"
    };

    public static readonly List<string> ShoulderDefNames = new List<string>
    {
        "Shoulder",
        "LeftMechanicalShoulder",
        "RightMechanicalShoulder",
        "ATR_MechanicalShoulder"
    };

    static HarmonyPatches()
    {
        var harmony = new Harmony("com.prostheticnomissingbodyparts.rimworld.mod");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}