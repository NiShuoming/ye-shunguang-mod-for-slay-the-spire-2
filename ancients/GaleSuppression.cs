using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;

namespace YeShunguang
{
    /// <summary>
    /// 定风波 1 随机造成7=10伤害3次，获得1=2青溟剑势,获得3张飞剑，
    /// </summary>
    public class GaleSuppression : YeCardModel
    {
        public GaleSuppression() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.RandomEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(7m,ValueProp.Move), new CardsVar(3),
            new DynamicVar("Force", 1m),new RepeatVar(3)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromCard<FlyingSword>(),
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).
                WithHitCount(DynamicVars.Repeat.IntValue).
                FromCard(this, cardPlay).TargetingRandomOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars["Force"].IntValue, Owner.Creature, this);

            await FlyingSword.CreateInHand(Owner, DynamicVars.Cards.IntValue, (MegaCrit.Sts2.Core.Combat.CombatState)CombatState);

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
            DynamicVars["Force"].UpgradeValueBy(1m);
        }
    }
}
