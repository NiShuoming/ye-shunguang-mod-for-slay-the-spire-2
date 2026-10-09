using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 拂衣去 1 记忆 获得7=10格挡，给予2脆弱
    /// </summary>
    public class CleanExit:YeCardModel
    {
        public CleanExit() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        public override bool GainsBlock => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [YeKeywords.Memory];

        protected override bool ShouldGlowGoldInternal => 
            Owner.Creature.GetPowerAmount<EnlightenedMind>()>0;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new BlockVar(7m,ValueProp.Move),new PowerVar<FrailPower>(2m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromPower<FrailPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<FrailPower>(choiceContext,cardPlay.Target,
                DynamicVars.Power<FrailPower>().BaseValue, Owner.Creature,
                this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}
