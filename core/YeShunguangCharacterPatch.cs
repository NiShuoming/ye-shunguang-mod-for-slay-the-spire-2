using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace YeShunguang
{
    /*
    [HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllCharacters), MethodType.Getter)]
    public static class YeShunguangCharactersPatch
    {
        static void Postfix(ref IEnumerable<CharacterModel> __result)
        {
            __result = __result
                .Append(ModelDb.Character<YeShunguang>())
                .Distinct();
        }
    }*/
}
