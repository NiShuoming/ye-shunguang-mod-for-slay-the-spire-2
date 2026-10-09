using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 暖霞拾光 3=2 【明心境】状态下，【记忆】不再消耗，【伙伴】也可以再释放一次 完成
    /// </summary>
    public class TouchOfDawnlight:YeCardModel
    {
        public TouchOfDawnlight() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EnlightenedMind>(),
            HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromKeyword(ZZZKeyWord.Partner)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<TouchOfDawnlightPower>(choiceContext,Owner.Creature,
                1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
