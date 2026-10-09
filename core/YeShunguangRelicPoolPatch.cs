using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace YeShunguang
{
    /*
    [HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllPotionPools), MethodType.Getter)]
    public static class YeShunguangRelicPoolPatch
    {
        static void Postfix(ref IEnumerable<PotionPoolModel> __result)
        {
            __result = __result
                .Append(ModelDb.PotionPool<YeShunguangPotionPool>())
                .Distinct();
        }
    }*/
}
