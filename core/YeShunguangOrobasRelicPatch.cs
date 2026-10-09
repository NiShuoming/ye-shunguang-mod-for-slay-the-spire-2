using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using System.Collections.Generic;
using System.Linq;
using YeShunguang;

[HarmonyPatch(typeof(TouchOfOrobas), "get_RefinementUpgrades")]
    public static class YeShunguangOrobasRelicPatch
    {
        static void Postfix(ref Dictionary<ModelId, RelicModel> __result)
        {
        if (!__result.ContainsKey(ModelDb.Relic<BurningClartity>().Id))
            __result.Add(ModelDb.Relic<BurningClartity>().Id, 
                ModelDb.Relic<CaseFullOfLight>());
                
        }
    }


