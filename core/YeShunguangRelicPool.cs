using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

namespace YeShunguang
{
    /// <summary>
    /// 将遗物添加到遗物池（未完成）
    /// </summary>
    public sealed class YeShunguangRelicPool : CustomRelicPoolModel
    {
        public override string EnergyColorName => "ye_shunguang";

        public override Color LabOutlineColor => Colors.Pink;
        /*
        /// <summary>
        /// 添加遗物
        /// </summary>
        /// <returns></returns>
        protected override List<RelicModel> GenerateAllRelics()
        {
            return new List<RelicModel>([
                ModelDb.Relic<BurningClartity>(),
                ModelDb.Relic<CaseFullOfLight>(),
                ModelDb.Relic<ClangOfJade>(),
                ModelDb.Relic<DreamboundSelf>(),
                ModelDb.Relic<GuidingGlimmer>(),
                ModelDb.Relic<LanternWish>(),
                ModelDb.Relic<LightShadow>(),
                ModelDb.Relic<Swordswoman>(),
                ModelDb.Relic<TogetherIntoTheDust>(),
            ]);
        }*/
    }
}
