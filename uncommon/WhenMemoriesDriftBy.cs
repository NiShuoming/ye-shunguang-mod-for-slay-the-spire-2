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

namespace YeShunguang
{
    /// <summary>
    /// 记忆流光时 1 每消耗1点剑势，获得3-》4格挡 完成
    /// </summary>
    public class WhenMemoriesDriftBy:YeCardModel
    {
        public WhenMemoriesDriftBy() : base(1,CardType.Power,CardRarity.Uncommon,TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new EnergyVar(1),new DynamicVar("Count",3m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
            HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<WhenMemoriesDriftByPower>(choiceContext, Owner.Creature,
                DynamicVars["Count"].BaseValue, Owner.Creature,this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Count"].UpgradeValueBy(1m);
        }
    }
}
