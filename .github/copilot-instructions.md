# GitHub Copilot Instructions for Prosthetic No Missing Body Parts (Continued)

## Mod Overview and Purpose

**Prosthetic No Missing Body Parts (Continued)** is a RimWorld mod aimed at enhancing gameplay for players who utilize prosthetics in their colonies. Originally created by iamrespawns and updated to improve compatibility and add new features, this mod ensures that pawns with bionic arms and legs can continue wearing gear for their hands and feet.

### Key Features and Systems

1. **Non-Destructive Harmony Patches**: Utilizes postfix instead of destructive prefix Harmony patches to enhance compatibility with other mods.
2. **Mod Settings for Customization**: Players can configure settings to specify which bionic replacements allow wearing gear on hands and feet.
3. **Compatibility with Other Mods**: Works seamlessly with popular mods like EPOE, VAE, Jewelry, and more, without the need for a new save game.
4. **Graceful Recovery**: If removed, the mod will not negatively impact the save game.
5. **Targeted Harmony Patching**: Patches the "HasPartsToWear" method to allow pawns with specific bionic limbs to wear appropriate apparel.

### Coding Patterns and Conventions

- **Harmony Patches**: Follow best practices by applying postfix patches for compatibility:
  - Ensure target methods are patched with careful consideration to avoid conflicts.
  - Use conditional logic to check for the presence of specific bionic parts before altering behavior.

- **XML Integration**: The mod does not alter XML body part configurations directly. Instead, dynamic checks are performed using Harmony patches to confirm apparel-wearing eligibility.

- **C# Practices**:
  - Use clear and descriptive class and method names (e.g., `ApparelUtility_HasPartsToWear`, `BodyPartUtils`).
  - Leverage static classes and methods for utility operations.
  - Employ `Mod` and `ModSettings` base classes to manage mod behavior and user settings.

### Harmony Patching

**Key Harmony Patch**:
The core functionality revolves around altering the `ApparelUtility.HasPartsToWear` method:
- **Objective**: Allow apparel-wearing if certain bionic parts are present.
- **Method**: Use a postfix patch to check for bionic arms or legs, adjusting the return value when conditions are met.

**Example Patch Structure**:
csharp
using HarmonyLib;

[HarmonyPatch(typeof(ApparelUtility), "HasPartsToWear")]
public static class HarmonyPatches
{
    static void Postfix(Pawn pawn, ref bool __result)
    {
        // Check conditions for bionic arms or legs and modify __result accordingly.
    }
}


### Suggestions for Copilot

1. **Automate Harmony Patch Detection**:
   - Copilot can assist in identifying the best methods to patch based on method names and their usage in the mod.

2. **Suggest Code Completion for Settings Management**:
   - Provide templated suggestions for settings retrieval and saving within `ProstheticNoMissingBodyPartsSettings`.

3. **XML Integration Guidance**:
   - Although this mod focuses on Harmony, Copilot could support future XML additions by recommending best practices for writing configuration files.

4. **Priority in Mod Load Order**:
   - Remind users to position this mod after other mods that introduce bionic parts for optimal compatibility.

5. **Error Handling Support**:
   - Offer suggestions for robust error checking and logging, especially when interfacing with a large number of other mods.

---

By following these instructions, the development and maintenance of the Prosthetic No Missing Body Parts mod can be both streamlined and enhanced, ensuring a smooth gaming experience for users. Make sure to regularly test with new RimWorld updates and other mod interactions to preserve mod functionality and compatibility.
