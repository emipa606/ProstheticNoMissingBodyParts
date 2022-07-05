using System.Collections.Generic;
using Verse;

namespace ProstheticNoMissingBodyParts;

public class ProstheticNoMissingBodyPartsSettings : ModSettings
{
    // default whitelisted arms, Vanilla and EPOE
    public List<string> ArmsWhitelist = new List<string>
    {
        "SimpleProstheticArm",
        "BionicArm",
        "AdvancedBionicArm",
        "ArchotechArm"
    };

    public List<string> FeetWhitelist = new List<string>
    {
        "SimpleProstheticLeg",
        "BionicLeg",
        "AdvancedBionicLeg",
        "ArchotechLeg",
        "MuscleStimulatorLegs"
    };

    public List<string> HandsWhitelist = new List<string>();

    // default whitelisted legs, Vanilla and EPOE
    public List<string> LegsWhitelist = new List<string>
    {
        "SimpleProstheticLeg",
        "BionicLeg",
        "AdvancedBionicLeg",
        "ArchotechLeg",
        "MuscleStimulatorLegs"
    };

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref ArmsWhitelist, "armsWhitelist", LookMode.Value);
        Scribe_Collections.Look(ref LegsWhitelist, "legsWhitelist", LookMode.Value);
        Scribe_Collections.Look(ref HandsWhitelist, "handsWhitelist", LookMode.Value);
        Scribe_Collections.Look(ref FeetWhitelist, "feetWhitelist", LookMode.Value);
    }
}