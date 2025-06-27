using System.Collections.Generic;
using Mlie;
using UnityEngine;
using Verse;

namespace ProstheticNoMissingBodyParts;

public class ProstheticNoMissingBodyPartsMod : Mod
{
    private static ProstheticNoMissingBodyPartsMod mod;
    private static string currentVersion;

    private readonly List<HediffDef> armsHediff = [];
    private readonly Dictionary<string, bool[]> armsWhitelistMap = new();
    private readonly List<HediffDef> feetHediff = [];
    private readonly Dictionary<string, bool[]> feetWhitelistMap = new();
    private readonly List<HediffDef> handsHediff = [];
    private readonly Dictionary<string, bool[]> handsWhitelistMap = new();
    private readonly List<HediffDef> legsHediff = [];
    private readonly Dictionary<string, bool[]> legsWhitelistMap = new();
    private readonly ProstheticNoMissingBodyPartsSettings settings;
    private Vector2 allViewScroll = new(0, 0);

    private bool isInitialized;

    public ProstheticNoMissingBodyPartsMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<ProstheticNoMissingBodyPartsSettings>();
        mod = this;
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void WriteSettings()
    {
        Log.Message("[ProstheticNoMissingBodyParts] Save settings");

        if (mod.settings.ArmsWhitelist != null)
        {
            mod.settings.ArmsWhitelist.Clear();
            foreach (var kv in armsWhitelistMap)
            {
                if (kv.Value[0])
                {
                    mod.settings.ArmsWhitelist.Add(kv.Key);
                }
            }
        }

        if (mod.settings.HandsWhitelist != null)
        {
            mod.settings.HandsWhitelist.Clear();
            foreach (var kv in handsWhitelistMap)
            {
                if (kv.Value[0])
                {
                    mod.settings.HandsWhitelist.Add(kv.Key);
                }
            }
        }

        if (mod.settings.LegsWhitelist != null)
        {
            mod.settings.LegsWhitelist.Clear();
            foreach (var kv in legsWhitelistMap)
            {
                if (kv.Value[0])
                {
                    mod.settings.LegsWhitelist.Add(kv.Key);
                }
            }
        }

        if (mod.settings.FeetWhitelist != null)
        {
            mod.settings.FeetWhitelist.Clear();
            foreach (var kv in feetWhitelistMap)
            {
                if (kv.Value[0])
                {
                    mod.settings.FeetWhitelist.Add(kv.Key);
                }
            }
        }

        base.WriteSettings();
    }

    private void init()
    {
        //check if init already done
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;

        Log.Message("[ProstheticNoMissingBodyParts] Init Settings");

        mod.settings.ArmsWhitelist ??= [];

        mod.settings.HandsWhitelist ??= [];

        mod.settings.LegsWhitelist ??= [];

        mod.settings.FeetWhitelist ??= [];

        // sets for checkboxes default checks
        var currentArmsSet = new HashSet<string>(mod.settings.ArmsWhitelist);
        var currentHandsSet = new HashSet<string>(mod.settings.HandsWhitelist);
        var currentLegsSet = new HashSet<string>(mod.settings.LegsWhitelist);
        var currentFeetSet = new HashSet<string>(mod.settings.FeetWhitelist);

        // load all recipes definitions that replace original (natural) arms or legs and extract hediff from it
        foreach (var recipeDef in DefDatabase<RecipeDef>.AllDefs)
        {
            if (recipeDef.appliedOnFixedBodyParts == null || recipeDef.addsHediff == null)
            {
                continue;
            }

            // catch arm
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.ShoulderDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Shoulder {recipeDef.addsHediff.defName}");
                armsWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentArmsSet.Contains(recipeDef.addsHediff.defName)];
                armsHediff.Add(recipeDef.addsHediff);
            }

            // catch hand
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.HandDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Hand {recipeDef.addsHediff.defName}");
                handsWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentHandsSet.Contains(recipeDef.addsHediff.defName)];
                handsHediff.Add(recipeDef.addsHediff);
            }

            // catch leg
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.LegDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Leg {recipeDef.addsHediff.defName}");
                legsWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentLegsSet.Contains(recipeDef.addsHediff.defName)];
                legsHediff.Add(recipeDef.addsHediff);
            }

            // catch foot
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.FootDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Foot {recipeDef.addsHediff.defName}");
                feetWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentFeetSet.Contains(recipeDef.addsHediff.defName)];
                feetHediff.Add(recipeDef.addsHediff);
            }
        }
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        init();

        var allHolder = new Rect(inRect.x, inRect.y + 25, inRect.width, inRect.height - 25);
        var allView = new Rect(allHolder.x, allHolder.y, allHolder.width - 24f,
            ((armsHediff.Count + handsHediff.Count + legsHediff.Count + feetHediff.Count) * 24f) + 200);

        var listingStandard = new Listing_Standard();
        Widgets.BeginScrollView(allHolder, ref allViewScroll, allView);
        listingStandard.Begin(allView);
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedArmsName".Translate());
        foreach (var hediffDef in armsHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref armsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedHandsName".Translate());
        foreach (var hediffDef in handsHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref handsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedLegsName".Translate());
        foreach (var hediffDef in legsHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref legsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();

        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedFeetName".Translate());
        foreach (var hediffDef in feetHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref feetWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        if (currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            listingStandard.Label("ProstheticNoMissingBodyPartsCurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        Widgets.EndScrollView();

        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "ProstheticNoMissingBodyPartsModName".Translate();
    }
}