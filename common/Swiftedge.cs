using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 快剑 1 造成8-10伤害，如果有以太帷幕，多造成3伤害,抽1=>2
    /// </summary>
    public class Swiftedge:YeCardModel
    {
        public Swiftedge() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CalculationBaseVar(8m),  new ExtraDamageVar(2m), new CardsVar(1),
            new CalculatedDamageVar(ValueProp.Move).
            WithMultiplier((card, _) => 
            (card.Owner.Creature.GetPower<EtherVeil>()==null?0:1))];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EtherVeil>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            if (Owner.Creature.GetPower<EtherVeil>() != null)
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(2m);
            DynamicVars.ExtraDamage.UpgradeValueBy(1m);
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}
