using System.Collections.Generic;
using Mlie;
using UnityEngine;
using Verse;

namespace ProstheticNoMissingBodyParts;

public class ProstheticNoMissingBodyPartsMod : Mod
{
    private static ProstheticNoMissingBodyPartsMod _mod;
    private static string currentVersion;

    private readonly List<HediffDef> _armsHediff = new List<HediffDef>();
    private readonly Dictionary<string, bool[]> _armsWhitelistMap = new Dictionary<string, bool[]>();
    private readonly List<HediffDef> _feetHediff = new List<HediffDef>();
    private readonly Dictionary<string, bool[]> _feetWhitelistMap = new Dictionary<string, bool[]>();
    private readonly List<HediffDef> _handsHediff = new List<HediffDef>();
    private readonly Dictionary<string, bool[]> _handsWhitelistMap = new Dictionary<string, bool[]>();
    private readonly List<HediffDef> _legsHediff = new List<HediffDef>();
    private readonly Dictionary<string, bool[]> _legsWhitelistMap = new Dictionary<string, bool[]>();
    private readonly ProstheticNoMissingBodyPartsSettings _settings;
    private Vector2 _allViewScroll = new Vector2(0, 0);

    private bool _isInitialized;

    public ProstheticNoMissingBodyPartsMod(ModContentPack content) : base(content)
    {
        _settings = GetSettings<ProstheticNoMissingBodyPartsSettings>();
        _mod = this;
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void WriteSettings()
    {
        Log.Message("[ProstheticNoMissingBodyParts] Save settings");

        if (_mod._settings.ArmsWhitelist != null)
        {
            _mod._settings.ArmsWhitelist.Clear();
            foreach (var kv in _armsWhitelistMap)
            {
                if (kv.Value[0])
                {
                    _mod._settings.ArmsWhitelist.Add(kv.Key);
                }
            }
        }

        if (_mod._settings.HandsWhitelist != null)
        {
            _mod._settings.HandsWhitelist.Clear();
            foreach (var kv in _handsWhitelistMap)
            {
                if (kv.Value[0])
                {
                    _mod._settings.HandsWhitelist.Add(kv.Key);
                }
            }
        }

        if (_mod._settings.LegsWhitelist != null)
        {
            _mod._settings.LegsWhitelist.Clear();
            foreach (var kv in _legsWhitelistMap)
            {
                if (kv.Value[0])
                {
                    _mod._settings.LegsWhitelist.Add(kv.Key);
                }
            }
        }

        if (_mod._settings.FeetWhitelist != null)
        {
            _mod._settings.FeetWhitelist.Clear();
            foreach (var kv in _feetWhitelistMap)
            {
                if (kv.Value[0])
                {
                    _mod._settings.FeetWhitelist.Add(kv.Key);
                }
            }
        }

        base.WriteSettings();
    }

    private void Init()
    {
        //check if init already done
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        Log.Message("[ProstheticNoMissingBodyParts] Init Settings");

        if (_mod._settings.ArmsWhitelist == null)
        {
            _mod._settings.ArmsWhitelist = new List<string>();
        }

        if (_mod._settings.HandsWhitelist == null)
        {
            _mod._settings.HandsWhitelist = new List<string>();
        }

        if (_mod._settings.LegsWhitelist == null)
        {
            _mod._settings.LegsWhitelist = new List<string>();
        }

        if (_mod._settings.FeetWhitelist == null)
        {
            _mod._settings.FeetWhitelist = new List<string>();
        }

        // sets for checkboxes default checks
        var currentArmsSet = new HashSet<string>(_mod._settings.ArmsWhitelist);
        var currentHandsSet = new HashSet<string>(_mod._settings.HandsWhitelist);
        var currentLegsSet = new HashSet<string>(_mod._settings.LegsWhitelist);
        var currentFeetSet = new HashSet<string>(_mod._settings.FeetWhitelist);

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
                _armsWhitelistMap[recipeDef.addsHediff.defName] =
                    new[] { currentArmsSet.Contains(recipeDef.addsHediff.defName) };
                _armsHediff.Add(recipeDef.addsHediff);
            }

            // catch hand
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.HandDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Hand {recipeDef.addsHediff.defName}");
                _handsWhitelistMap[recipeDef.addsHediff.defName] =
                    new[] { currentHandsSet.Contains(recipeDef.addsHediff.defName) };
                _handsHediff.Add(recipeDef.addsHediff);
            }

            // catch leg
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.LegDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Leg {recipeDef.addsHediff.defName}");
                _legsWhitelistMap[recipeDef.addsHediff.defName] =
                    new[] { currentLegsSet.Contains(recipeDef.addsHediff.defName) };
                _legsHediff.Add(recipeDef.addsHediff);
            }

            // catch foot
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => HarmonyPatches.FootDefNames.Contains(x.defName)))
            {
                Log.Message($"[ProstheticNoMissingBodyParts] Add Foot {recipeDef.addsHediff.defName}");
                _feetWhitelistMap[recipeDef.addsHediff.defName] =
                    new[] { currentFeetSet.Contains(recipeDef.addsHediff.defName) };
                _feetHediff.Add(recipeDef.addsHediff);
            }
        }
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Init();

        var allHolder = new Rect(inRect.x, inRect.y + 25, inRect.width, inRect.height - 25);
        var allView = new Rect(allHolder.x, allHolder.y, allHolder.width - 24f,
            ((_armsHediff.Count + _handsHediff.Count + _legsHediff.Count + _feetHediff.Count) * 24f) + 200);

        var listingStandard = new Listing_Standard();
        Widgets.BeginScrollView(allHolder, ref _allViewScroll, allView);
        listingStandard.Begin(allView);
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedArmsName".Translate());
        foreach (var hediffDef in _armsHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref _armsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedHandsName".Translate());
        foreach (var hediffDef in _handsHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref _handsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedLegsName".Translate());
        foreach (var hediffDef in _legsHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref _legsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.GapLine();

        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedFeetName".Translate());
        foreach (var hediffDef in _feetHediff)
        {
            listingStandard.CheckboxLabeled(
                $"{hediffDef.label.CapitalizeFirst()} ({hediffDef.defName})",
                ref _feetWhitelistMap[hediffDef.defName][0],
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