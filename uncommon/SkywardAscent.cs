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
    /// 扶摇势 1 记忆 9=11格挡，1剑势4=6格挡 完成
    /// </summary>
    public class SkywardAscent : YeCardModel
    {
        public SkywardAscent() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new BlockVar(9m, ValueProp.Move), new BlockVar("Extra", 4m, ValueProp.Move)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [YeKeywords.Memory];

        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= 1|| 
            Owner.Creature.GetPowerAmount<EnlightenedMind>()>0;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            int n = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            if (n > 0)
            {
                await ZZZCmd.AddSecondEnergy(Owner, -1, Owner.Creature, this);
                await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["Extra"],
                    cardPlay);
            }

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2m);
            DynamicVars["Extra"].UpgradeValueBy(2m);
        }
    }
}
