using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using BaseLib;
using System.Reflection;
using MegaCrit.Sts2.Core.Audio;
using BaseLib.Utils;

namespace YeShunguang
{
	[ModInitializer(nameof(Initialize))]
	public static class YeCustomModInitializer
	{
		public static string VoicePatch = "res://YeShunguang/voice/";

        public const string ModId = "YeShunguang";
		public static void Initialize()
		{
			GD.Print("YeCustomModInitializer - 加载中!");
			Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
			var harmony = new Harmony("YeShunguang");
            harmony.PatchAll();
            GD.Print("YeCustomModInitializer - 加载成功!");
        }
    }
}
