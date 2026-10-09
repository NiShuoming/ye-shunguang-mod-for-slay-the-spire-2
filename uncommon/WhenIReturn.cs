using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 归去时 1 获得7->10点格挡，本回合受到脆弱敌人的伤害-3=4
    /// </summary>
    public class WhenIReturn:YeCardModel
    {
        public WhenIReturn() : base(1,CardType.Skill,CardRarity.Uncommon,TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new BlockVar(7m, ValueProp.Move), new DynamicVar("Decrease",3m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<FrailPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<WhenIReturnPower>(choiceContext, Owner.Creature,
                DynamicVars["Decrease"].BaseValue,Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
            DynamicVars["Decrease"].UpgradeValueBy(1m);
        }
    }
}
