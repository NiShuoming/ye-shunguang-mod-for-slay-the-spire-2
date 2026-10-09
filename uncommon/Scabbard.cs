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

namespace YeShunguang
{
    /// <summary>
    /// 剑鞘 0 每生成过1把飞剑，获得2->3格挡，最多获得12->18格挡 完成
    /// </summary>
    public class Scabbard:YeCardModel
    {
        public Scabbard():base(0,CardType.Skill,CardRarity.Uncommon,TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CalculationBaseVar(0m),new CalculationExtraVar(2m),
             new CalculatedBlockVar(ValueProp.Move).
            WithMultiplier((card, _) => {
                int temp = card.Owner.PlayerCombatState.AllCards.
            Count(c => c.Tags.Contains(CardTag.Shiv));
                return Math.Clamp(temp,0,6);})];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromCard<FlyingSword>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await FlyingSword.CreateInHand(Owner, 1,
                (MegaCrit.Sts2.Core.Combat.CombatState)CombatState);

            await CreatureCmd.GainBlock(base.Owner.Creature, 
                new BlockVar(DynamicVars.CalculatedBlock.Calculate(Owner.Creature)
                ,ValueProp.Move),cardPlay);
            
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationExtra.UpgradeValueBy(1m);
        }
    }
}
