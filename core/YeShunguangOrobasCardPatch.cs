using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.GameInfo.Objects;
using System.Collections.Generic;
using YeShunguang;

[HarmonyPatch(typeof(ArchaicTooth), "get_TranscendenceUpgrades")]
public static class YeShunguangOrobasCardPatch
{
    static void Postfix(ref Dictionary<ModelId, CardModel> __result)
    {
        if (!__result.ContainsKey(ModelDb.Relic<BurningClartity>().Id))
            __result.Add(ModelDb.Card<GuidingTides>().Id,
            ModelDb.Card<GaleSuppression>());

    }
}
