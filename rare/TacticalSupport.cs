using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseLib.Utils.BetaMainCompatibility;

namespace YeShunguang
{
    /// <summary>
    /// 策应 1 【记忆】获得8->11格挡，抽1，获得【明心境】时，召唤到手中 完成
    /// </summary> 
    public class TacticalSupport:YeCardModel
    {
        public TacticalSupport():base(1,CardType.Skill,CardRarity.Rare,TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new BlockVar(8m,ValueProp.Move), new CardsVar(1)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [YeKeywords.Memory];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromPower<EnlightenedMind>()];
        protected override bool ShouldGlowGoldInternal =>
    Owner.Creature.GetPowerAmount<EnlightenedMind>() > 0;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue,Owner);
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            if(power.Owner == Owner.Creature && power is EnlightenedMind && amount > 0)
            {
                await CardPileCmd.Add(this, PileType.Hand);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}
