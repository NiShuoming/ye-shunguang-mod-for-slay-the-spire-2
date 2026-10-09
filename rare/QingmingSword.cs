using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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

namespace YeShunguang
{
    /// <summary>
    /// 青溟剑 2 消耗1张手牌，造成15点伤害，消耗堆中每有1张【记忆】卡，伤害+3=5 完成
    /// </summary>
    public class QingmingSword:YeCardModel
    {
        public QingmingSword() : base(2,CardType.Attack,CardRarity.Rare,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new CalculationBaseVar(15m), new CardsVar(1),
            new ExtraDamageVar(3m), new CalculatedDamageVar(ValueProp.Move)
            .WithMultiplier((card,_)
            =>(PileType.Exhaust.GetPile(card.Owner).Cards
            .Count(a=>a.Keywords.Contains(YeKeywords.Memory))))];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(
                choiceContext, Owner,
                new CardSelectorPrefs(base.SelectionScreenPrompt, 1),
                null, this);

            foreach (CardModel c in enumerable.ToList())
            {
                await CardCmd.Exhaust(choiceContext, c);
            }

            await DamageCmd.Attack(base.DynamicVars.CalculatedDamage)
                .FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(2m);
        }
    }
}
