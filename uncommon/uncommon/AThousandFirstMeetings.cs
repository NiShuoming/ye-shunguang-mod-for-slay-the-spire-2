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
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 千万次初见 2 击破格挡时，打出抽牌堆中的一张【伙伴】
    /// </summary>
    public class AThousandFirstMeetings : YeCardModel
    {
        public AThousandFirstMeetings():base(2,CardType.Power,CardRarity.Uncommon,TargetType.Self){}

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(ZZZKeyWord.Partner),
            HoverTipFactory.Static(StaticHoverTip.Block)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<AThousandFirstMeetingsPower>(choiceContext,Owner.Creature,
                1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
