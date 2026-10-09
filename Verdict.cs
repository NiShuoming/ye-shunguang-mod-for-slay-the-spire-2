using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;
using ZZZLib.Other;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 决裁 开启2=3层以太帷幕，获得2层剑势，已满开启2明心境
    /// </summary>
    public class Verdict : YeCardModel
    {
        public Verdict() : base(2, CardType.Skill, CardRarity.Basic, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("EtherVeil", 2m), new DynamicVar("Force", 2m), new DynamicVar("Mind", 2m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<EtherVeil>(), HoverTipFactory.FromPower<QingmingSwordForce>(),
            HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= ZZZCmd.GetSecondEnergy(Owner).MaxEnergy2;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<EtherVeil>(choiceContext, Owner.Creature, DynamicVars["EtherVeil"].BaseValue, Owner.Creature, this);
            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars["Force"].IntValue, Owner.Creature, this);

            ZZZSecondEnergy zz = ZZZCmd.GetSecondEnergy(Owner);
            if (zz.Energy2 >= 6)
            {
                await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature, DynamicVars["Mind"].BaseValue, Owner.Creature, this);

            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["EtherVeil"].UpgradeValueBy(1m);
        }
    }


}
