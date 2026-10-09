using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 向着阳光灿烂的明天 1=0 清除目标的脆弱，获得等量飞剑
    /// </summary>
    public class TowardABrightAndSunnyTomorrow:YeCardModel
    {
        public TowardABrightAndSunnyTomorrow() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<FrailPower>(),
            HoverTipFactory.FromCard<FlyingSword>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            int n = cardPlay.Target.GetPowerAmount<FrailPower>();

            if(n<=0)return;

            await PowerCmd.Apply<FrailPower>(choiceContext, cardPlay.Target,
                -n, Owner.Creature, this);

            await FlyingSword.CreateInHand(Owner, n,
                (MegaCrit.Sts2.Core.Combat.CombatState)CombatState);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
