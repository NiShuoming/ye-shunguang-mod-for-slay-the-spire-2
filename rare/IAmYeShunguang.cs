using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 名为叶瞬光 2=1 消耗 结束明心境，获得6格挡，抽满手牌 完成
    /// </summary>
    public class IAmYeShunguang:YeCardModel
    {
        public IAmYeShunguang() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new CardsVar(5), new BlockVar(10m, ValueProp.Move)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature, 
                -Owner.Creature.GetPowerAmount<EnlightenedMind>(), Owner.Creature, this);

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            int num = 10 - base.Owner.PlayerCombatState.Hand.Cards.Count;
            await CardPileCmd.Draw(choiceContext, num, base.Owner);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
