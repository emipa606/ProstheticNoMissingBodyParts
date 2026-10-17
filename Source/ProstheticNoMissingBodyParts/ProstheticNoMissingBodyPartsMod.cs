using System;
using System.Collections.Generic;
using System.Linq;
using Mlie;
using UnityEngine;
using Verse;

namespace ProstheticNoMissingBodyParts;

public class ProstheticNoMissingBodyPartsMod : Mod
{
    private static string currentVersion;

    private readonly List<HediffDef> armsHediff = [];
    private readonly Dictionary<string, bool[]> armsWhitelistMap = new();
    private readonly List<HediffDef> feetHediff = [];
    private readonly Dictionary<string, bool[]> feetWhitelistMap = new();
    private readonly List<HediffDef> handsHediff = [];
    private readonly Dictionary<string, bool[]> handsWhitelistMap = new();
    private readonly List<HediffDef> legsHediff = [];
    private readonly Dictionary<string, bool[]> legsWhitelistMap = new();
    public static ProstheticNoMissingBodyPartsSettings settings { get; private set; }
    private Vector2 allViewScroll = new(0, 0);

    private bool isInitialized;
    private string searchText = string.Empty;

    public ProstheticNoMissingBodyPartsMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<ProstheticNoMissingBodyPartsSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void WriteSettings()
    {
        if (settings.ArmsWhitelist != null)
        {
            settings.ArmsWhitelist.Clear();
            foreach (var key in armsWhitelistMap.Keys.OrderBy(o=>o))
            {
                if (armsWhitelistMap.TryGetValue(key, out var value) && value[0])
                {
                    settings.ArmsWhitelist.Add(key);
                }
            }
        }

        if (settings.HandsWhitelist != null)
        {
            settings.HandsWhitelist.Clear();
            foreach (var key in handsWhitelistMap.Keys.OrderBy(o=>o))
            {
                if (handsWhitelistMap.TryGetValue(key, out var value) && value[0])
                {
                    settings.HandsWhitelist.Add(key);
                }
            }
        }

        if (settings.LegsWhitelist != null)
        {
            settings.LegsWhitelist.Clear();
            foreach (var key in legsWhitelistMap.Keys.OrderBy(o=>o))
            {
                if (legsWhitelistMap.TryGetValue(key, out var value) && value[0])
                {
                    settings.LegsWhitelist.Add(key);
                }
            }
        }

        if (settings.FeetWhitelist != null)
        {
            settings.FeetWhitelist.Clear();
            foreach (var key in feetWhitelistMap.Keys.OrderBy(o=>o))
            {
                if (feetWhitelistMap.TryGetValue(key, out var value) && value[0])
                {
                    settings.FeetWhitelist.Add(key);
                }
            }
        }

        ModDefOf.resetCaches();
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

        settings.ArmsWhitelist ??= [];

        settings.HandsWhitelist ??= [];

        settings.LegsWhitelist ??= [];

        settings.FeetWhitelist ??= [];

        // sets for checkboxes default checks
        var currentArmsSet = new HashSet<string>(settings.ArmsWhitelist);
        var currentHandsSet = new HashSet<string>(settings.HandsWhitelist);
        var currentLegsSet = new HashSet<string>(settings.LegsWhitelist);
        var currentFeetSet = new HashSet<string>(settings.FeetWhitelist);

        // load all recipes definitions that replace original (natural) arms or legs and extract hediff from it
        foreach (var recipeDef in DefDatabase<RecipeDef>.AllDefs)
        {
            if (recipeDef.appliedOnFixedBodyParts == null || recipeDef.addsHediff == null)
            {
                continue;
            }

            // catch arm
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => ModDefOf.ShoulderDefs.Contains(x)))
            {
                armsWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentArmsSet.Contains(recipeDef.addsHediff.defName)];
                armsHediff.Add(recipeDef.addsHediff);
            }

            // catch hand
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => ModDefOf.HandDefs.Contains(x)))
            {
                handsWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentHandsSet.Contains(recipeDef.addsHediff.defName)];
                handsHediff.Add(recipeDef.addsHediff);
            }

            // catch leg
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => ModDefOf.LegDefs.Contains(x)))
            {
                legsWhitelistMap[recipeDef.addsHediff.defName] =
                    [currentLegsSet.Contains(recipeDef.addsHediff.defName)];
                legsHediff.Add(recipeDef.addsHediff);
            }

            // catch foot
            if (!recipeDef.appliedOnFixedBodyParts.Exists(x => ModDefOf.FootDefs.Contains(x)))
            {
                continue;
            }

            feetWhitelistMap[recipeDef.addsHediff.defName] =
                [currentFeetSet.Contains(recipeDef.addsHediff.defName)];
            feetHediff.Add(recipeDef.addsHediff);
        }
        ModDefOf.resetCaches();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        init();

        var searchTextBoxRect = new Rect(inRect.x, inRect.y, inRect.width, 25);
        searchText = Widgets.TextEntryLabeled(searchTextBoxRect, "ProstheticNoMissingBodyPartsSearch".Translate(),
            searchText);
        TooltipHandler.TipRegion(searchTextBoxRect, "ProstheticNoMissingBodyPartsSearchTT".Translate());

        var armsFiltered = armsHediff.Where(def =>
            string.IsNullOrEmpty(searchText) ||
            def.label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.defName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.modContentPack?.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
        var handsFiltered = handsHediff.Where(def =>
            string.IsNullOrEmpty(searchText) ||
            def.label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.defName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.modContentPack?.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
        var legsFiltered = legsHediff.Where(def =>
            string.IsNullOrEmpty(searchText) ||
            def.label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.defName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.modContentPack?.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
        var feetFiltered = feetHediff.Where(def =>
            string.IsNullOrEmpty(searchText) ||
            def.label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.defName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
            def.modContentPack?.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);

        var allHolder = new Rect(inRect.x, inRect.y + 25, inRect.width, inRect.height - 25);
        var allView = new Rect(allHolder.x, allHolder.y, allHolder.width - 24f,
            ((armsFiltered.Count() + handsFiltered.Count() + legsFiltered.Count() + feetFiltered.Count()) * 24f) + 200);

        var listingStandard = new Listing_Standard();
        Widgets.BeginScrollView(allHolder, ref allViewScroll, allView);
        listingStandard.Begin(allView);
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedArmsName".Translate());
        foreach (var hediffDef in armsFiltered)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref armsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedHandsName".Translate());
        foreach (var hediffDef in handsFiltered)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref handsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedLegsName".Translate());
        foreach (var hediffDef in legsFiltered)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref legsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();

        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedFeetName".Translate());
        foreach (var hediffDef in feetFiltered)
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
