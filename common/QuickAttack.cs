using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 速攻 1 造成8->10点伤害，如果造成伤害，给予1易伤，抽1=>2张牌 完成
    /// </summary>
    public class QuickAttack:YeCardModel
    {
        public QuickAttack():base(1,CardType.Attack, CardRarity.Common,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(8m,ValueProp.Move), new PowerVar<VulnerablePower>(1m),
            new CardsVar(1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<VulnerablePower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            AttackCommand temp = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            bool dam = temp.Results.Any((List<DamageResult> a) => a.Any((DamageResult a) => (a.UnblockedDamage > 0)));
            if (dam)
            {
                await PowerCmd.Apply<VulnerablePower>(choiceContext,cardPlay.Target,
                    DynamicVars.Power<VulnerablePower>().BaseValue,
                    Owner.Creature, this);

                await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.IntValue ,Owner);
            }
                
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}
