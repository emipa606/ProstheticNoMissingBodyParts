using System.Reflection;
using HarmonyLib;
using Verse;

namespace ProstheticNoMissingBodyParts;

[StaticConstructorOnStartup]
internal class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmony = new Harmony("com.prostheticnomissingbodyparts.rimworld.mod");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}