using BaseLib.Extensions;
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
    public class PerfectDodge : YeCardModel
    {
        public PerfectDodge() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer) { }

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<QingmingSwordForce>(1), new CalculationBaseVar(0),
            new CalculationExtraVar(2), new CalculatedBlockVar(ValueProp.Unpowered).
            WithMultiplier((card, _) => ZZZCmd.GetSecondEnergy(card.Owner).Energy2)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);

            await CreatureCmd.GainBlock(cardPlay.Target, new BlockVar(DynamicVars.CalculatedBlock.Calculate(Owner.Creature)
                , ValueProp.Unpowered), cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<QingmingSwordForce>().UpgradeValueBy(1);
        }
    }
}
