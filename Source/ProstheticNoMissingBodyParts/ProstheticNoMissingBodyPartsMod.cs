using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ProstheticNoMissingBodyParts;

public class ProstheticNoMissingBodyPartsMod : Mod
{
    private static ProstheticNoMissingBodyPartsMod _mod;

    private readonly List<HediffDef> _armsHediff = new List<HediffDef>();

    private readonly Dictionary<string, bool[]> _armsWhitelistMap = new Dictionary<string, bool[]>();
    private readonly List<HediffDef> _legsHediff = new List<HediffDef>();
    private readonly Dictionary<string, bool[]> _legsWhitelistMap = new Dictionary<string, bool[]>();
    private readonly ProstheticNoMissingBodyPartsSettings _settings;

    private string _armSearchQuery = "";

    private Vector2 _armsViewScroll = new Vector2(0, 0);

    private bool _isInitialized;
    private string _legSearchQuery = "";
    private Vector2 _legsViewScroll = new Vector2(0, 0);

    public ProstheticNoMissingBodyPartsMod(ModContentPack content) : base(content)
    {
        _settings = GetSettings<ProstheticNoMissingBodyPartsSettings>();
        _mod = this;
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

        if (_mod._settings.LegsWhitelist == null)
        {
            _mod._settings.LegsWhitelist = new List<string>();
        }

        // sets for checkboxes default checks
        var currentArmsSet = new HashSet<string>(_mod._settings.ArmsWhitelist);
        var currentLegsSet = new HashSet<string>(_mod._settings.LegsWhitelist);

        // load all recipes definitions that replace original (natural) arms or legs and extract hediff from it
        foreach (var recipeDef in DefDatabase<RecipeDef>.AllDefs)
        {
            if (recipeDef.appliedOnFixedBodyParts == null || recipeDef.addsHediff == null)
            {
                continue;
            }

            // catch arm
            if (recipeDef.appliedOnFixedBodyParts.Exists(x => x.defName.Equals("Shoulder")))
            {
                Log.Message("[ProstheticNoMissingBodyParts] Add Shoulder " + recipeDef.addsHediff.defName);
                _armsWhitelistMap[recipeDef.addsHediff.defName] =
                    new[] { currentArmsSet.Contains(recipeDef.addsHediff.defName) };
                _armsHediff.Add(recipeDef.addsHediff);
            }

            // catch leg
            if (!recipeDef.appliedOnFixedBodyParts.Exists(x => x.defName.Equals("Leg")))
            {
                continue;
            }

            Log.Message("[ProstheticNoMissingBodyParts] Add Leg " + recipeDef.addsHediff.defName);
            _legsWhitelistMap[recipeDef.addsHediff.defName] =
                new[] { currentLegsSet.Contains(recipeDef.addsHediff.defName) };
            _legsHediff.Add(recipeDef.addsHediff);
        }
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Init();

        var listingStandard = new Listing_Standard();


        // arms settings GUI
        var armsGroup = new Rect(inRect.x, inRect.y, inRect.width, 180f);
        var armsSearchRect = new Rect(inRect.width - 200f, armsGroup.y, 200f, 24f);

        _armSearchQuery = Widgets.TextArea(armsSearchRect, _armSearchQuery);
        var armSearchQueryIsEmpty = _armSearchQuery.NullOrEmpty();

        listingStandard.Begin(armsGroup);
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedArmsName".Translate());
        listingStandard.End();

        var filteredArms = _armsHediff.FindAll(x =>
            armSearchQueryIsEmpty ||
            x.defName.ToLower().Contains(_armSearchQuery.ToLower()) ||
            x.label.ToLower().Contains(_armSearchQuery.ToLower())
        );

        var armsHolder = new Rect(armsGroup.x, armsGroup.y + 30, armsGroup.width, 150f);
        var armsView = new Rect(armsHolder.x, armsHolder.y, armsGroup.width - 24f, filteredArms.Count * 24f);

        Widgets.BeginScrollView(armsHolder, ref _armsViewScroll, armsView);
        listingStandard.Begin(armsView);

        foreach (var hediffDef in filteredArms)
        {
            listingStandard.CheckboxLabeled(
                hediffDef.label.CapitalizeFirst() + " (" + hediffDef.defName + ")",
                ref _armsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
        }

        listingStandard.End();
        Widgets.EndScrollView();


        // legs setting GUI
        var legsGroup = new Rect(inRect.x, inRect.y + 230f, inRect.width, 180f);
        var legsSearchRect = new Rect(inRect.width - 200f, legsGroup.y, 200f, 24f);

        _legSearchQuery = Widgets.TextArea(legsSearchRect, _legSearchQuery);
        var legSearchQueryIsEmpty = _legSearchQuery.NullOrEmpty();

        listingStandard.Begin(legsGroup);
        listingStandard.Label("ProstheticNoMissingBodyPartsWhitelistedLegsName".Translate());
        listingStandard.End();

        var filteredLegs = _legsHediff.FindAll(x =>
            legSearchQueryIsEmpty ||
            x.defName.ToLower().Contains(_legSearchQuery.ToLower()) ||
            x.label.ToLower().Contains(_legSearchQuery.ToLower())
        );

        var legsHolder = new Rect(legsGroup.x, legsGroup.y + 30, legsGroup.width, 150f);
        var legsView = new Rect(legsHolder.x, legsHolder.y, legsGroup.width - 24f, filteredLegs.Count * 24f);

        Widgets.BeginScrollView(legsHolder, ref _legsViewScroll, legsView);
        listingStandard.Begin(legsView);

        foreach (var hediffDef in filteredLegs)
        {
            listingStandard.CheckboxLabeled(
                hediffDef.label.CapitalizeFirst() + " (" + hediffDef.defName + ")",
                ref _legsWhitelistMap[hediffDef.defName][0],
                hediffDef.description
            );
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