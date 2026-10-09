using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

namespace YeShunguang
{
    public sealed class YeShunguangPotionPool : CustomPotionPoolModel
    {
        public override string EnergyColorName => "ye_shunguang";

        public override Color LabOutlineColor => Colors.Pink;

        protected override List<PotionModel> GenerateAllPotions()
        {
            return [
                ModelDb.Potion<ApprenticeshipTea>(),
                ModelDb.Potion<OsmanthusCandy>(),
                ModelDb.Potion<SwordForceGourd>()
            ];
        }
    }
}
