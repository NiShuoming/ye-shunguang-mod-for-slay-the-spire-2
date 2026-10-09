using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 光年效应 消耗 保留 手牌飞剑=>打击1重放 完成
    /// </summary>
    public class LightYearPhenomenon:YeCardModel
    {
        public LightYearPhenomenon() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromCard<FlyingSword>(),
            HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust, CardKeyword.Retain];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            var enumerable = PileType.Hand.GetPile(base.Owner).Cards.Where(a => (a.Tags.Contains(CardTag.Shiv) || a.Tags.Contains(CardTag.Strike)));
            foreach (var card in enumerable)
            {
                card.BaseReplayCount++;
            }
        }
        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
