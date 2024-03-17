using System.Collections.Generic;
using Verse;

namespace ProstheticNoMissingBodyParts;

public class ProstheticNoMissingBodyPartsSettings : ModSettings
{
    // default whitelisted arms, Vanilla and EPOE
    public List<string> ArmsWhitelist =
    [
        "SimpleProstheticArm",
        "BionicArm",
        "AdvancedBionicArm",
        "ArchotechArm"
    ];

    public List<string> FeetWhitelist =
    [
        "SimpleProstheticLeg",
        "BionicLeg",
        "AdvancedBionicLeg",
        "ArchotechLeg",
        "MuscleStimulatorLegs"
    ];

    public List<string> HandsWhitelist = [];

    // default whitelisted legs, Vanilla and EPOE
    public List<string> LegsWhitelist =
    [
        "SimpleProstheticLeg",
        "BionicLeg",
        "AdvancedBionicLeg",
        "ArchotechLeg",
        "MuscleStimulatorLegs"
    ];

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref ArmsWhitelist, "armsWhitelist", LookMode.Value);
        Scribe_Collections.Look(ref LegsWhitelist, "legsWhitelist", LookMode.Value);
        Scribe_Collections.Look(ref HandsWhitelist, "handsWhitelist", LookMode.Value);
        Scribe_Collections.Look(ref FeetWhitelist, "feetWhitelist", LookMode.Value);
    }
}