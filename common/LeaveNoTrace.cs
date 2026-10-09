using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 不留行 1 【消耗】获得7->10格挡，施加2=3层虚弱 完成
    /// </summary>
    public class LeaveNoTrace : YeCardModel
    {
        public LeaveNoTrace() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy){}

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust];

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new BlockVar(7m,ValueProp.Move), new PowerVar<WeakPower>(2m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<WeakPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, 
                DynamicVars.Power<WeakPower>().BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
            DynamicVars.Power<WeakPower>().UpgradeValueBy(1m);
        }
    }
}
