using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 剑影 0 造成7-10点伤害，手牌中每有1张0费牌，多攻击一次,生成1张飞剑
    /// </summary>
    public class SwordShadow:YeCardModel
    {
        public SwordShadow() : base(0,CardType.Attack,CardRarity.Rare,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(7m, ValueProp.Move), new RepeatVar(0)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardPile pile = PileType.Hand.GetPile(Owner);

            int n = pile.Cards.Count(a => a.EnergyCost.GetWithModifiers(CostModifiers.All) == 0)+1;

            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(n + DynamicVars.Repeat.IntValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await FlyingSword.CreateInHand(Owner, 1, (MegaCrit.Sts2.Core.Combat.CombatState)CombatState);

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
