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
    /// <summary>
    /// 逍遥云踪 2 12=15格挡。2=3青溟剑势
    /// </summary>
    public class WanderingCloud : YeCardModel
    {
        public WanderingCloud() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new BlockVar(12m, ValueProp.Move), new PowerVar<QingmingSwordForce>(2m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block,
                cardPlay);
            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
            DynamicVars.Power<QingmingSwordForce>().UpgradeValueBy(1m);

        }
    }
}
