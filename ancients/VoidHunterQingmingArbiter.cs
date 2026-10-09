using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 虚狩 3 x费效果+2-3，每回合为所有敌人施加4格挡
    /// </summary>
    public class VoidHunterQingmingArbiter : YeCardModel
    {
        public VoidHunterQingmingArbiter():base(3, CardType.Power, CardRarity.Ancient,TargetType.Self)
        { 

        }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DynamicVar("Count", 2m), new DynamicVar("EnemyBlock", 4m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<VoidHunterQingmingArbiterPower>(choiceContext,
                Owner.Creature, DynamicVars["Count"].BaseValue, Owner.Creature, this);
            
            await PowerCmd.Apply<VoidHunterQingmingArbiterPower2>(choiceContext,
                Owner.Creature, DynamicVars["EnemyBlock"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Count"].UpgradeValueBy(1m);
        }
    }
}
