using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;
using ZZZLib.Other;

namespace YeShunguang
{
    /// <summary>
    /// 生命的奇迹 2 【消耗】【】=》【保留】下回合获得12格挡，若剑势已满，获得2明心境 完成
    /// </summary>
    public class MiracleOfLife : YeCardModel
    {
        public MiracleOfLife() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("NextBlock", 12)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
             HoverTipFactory.FromPower<EnlightenedMind>(),
            HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= ZZZCmd.GetSecondEnergy(Owner).MaxEnergy2;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature,
                DynamicVars["NextBlock"].BaseValue,
                Owner.Creature, this);
            ZZZSecondEnergy zz = ZZZCmd.GetSecondEnergy(Owner);
            if (zz.Energy2 >= 6)
            {
                await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature,
                    2, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
            DynamicVars["NextBlock"].UpgradeValueBy(4);
        }
    }
}
