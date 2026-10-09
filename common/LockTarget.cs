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
using System.Threading.Tasks;
using ZZZLib.Cmds;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 锁敌 造成9->12点伤害，若有1剑势消耗，获得1帷幕，抽1
    /// </summary>
    public class LockTarget : YeCardModel
    {
        public LockTarget() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(9m, ValueProp.Move), new CardsVar(1),
            new PowerVar<EtherVeil>(1m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<EtherVeil>(), 
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= 1;


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            int n = ZZZCmd.GetSecondEnergy(Owner).Energy2;

            if (n >= 1)
            {
                await ZZZCmd.AddSecondEnergy(Owner, -1, Owner.Creature, this);
                await PowerCmd.Apply<EtherVeil>(choiceContext, Owner.Creature,
                DynamicVars.Power<EtherVeil>().BaseValue, Owner.Creature,
                this);
                await CardPileCmd.Draw(choiceContext,Owner);
            }

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
