using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 飞光 1费4伤，消耗所有剑势,每有1青溟剑势多打x+1->x+2次 完成
    /// </summary>
    public class SoaringLight : YeCardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [YeKeywords.Memory];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(4m, ValueProp.Move), new DynamicVar("Count", 1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];

        public SoaringLight() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override bool ShouldGlowGoldInternal =>
            Owner.Creature.GetPowerAmount<EnlightenedMind>() > 0;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int num = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            await ZZZCmd.AddSecondEnergy(Owner, -num, Owner.Creature, this);
            num = Hook.ModifyXValue(CombatState, this, num);
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).
                WithHitCount(num + DynamicVars["Count"].IntValue).
                FromCard(this, cardPlay).
                TargetingAllOpponents(base.CombatState).
                WithHitFx("vfx/vfx_attack_slash").
                Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["Count"].UpgradeValueBy(1m);
        }
    }
}
