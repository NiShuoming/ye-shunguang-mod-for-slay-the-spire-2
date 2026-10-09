using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 斩邪祟 造成7->9点伤害，若帷幕超过1，每层多攻击一次，消耗1层
    /// </summary>
    public class SmiteTheWicked:YeCardModel
    {
        public SmiteTheWicked() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(7m, ValueProp.Move), new PowerVar<EtherVeil>(1),
            new RepeatVar(1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
            HoverTipFactory.FromPower<EtherVeil>()];

        protected override bool ShouldGlowGoldInternal => 
            Owner.Creature.GetPowerAmount<EtherVeil>()>=2;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            int count = DynamicVars.Repeat.IntValue;
            if (Owner.Creature.GetPowerAmount<EtherVeil>() > 0)
            {
                await PowerCmd.Apply<EtherVeil>(choiceContext,
                    Owner.Creature, -1, Owner.Creature, this);
                count += Owner.Creature.GetPowerAmount<EtherVeil>();
            }
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(count)
                .FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
