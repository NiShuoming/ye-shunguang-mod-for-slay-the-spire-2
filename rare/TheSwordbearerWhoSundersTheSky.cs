using BaseLib.Extensions;
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
using ZZZLib;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 步溟仗剑开天光 1 【】->【固有】抽到【伙伴】牌时会开启1层【以太帷幕】，获得1格挡 完成
    /// </summary>
    public class TheSwordbearerWhoSundersTheSky:YeCardModel
    {
        public TheSwordbearerWhoSundersTheSky() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<EtherVeil>(1m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EtherVeil>(),
            HoverTipFactory.FromKeyword(ZZZKeyWord.Partner),
             HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<TheSwordbearerWhoSundersTheSkyPower>(choiceContext, Owner.Creature,
                DynamicVars.Power<EtherVeil>().BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}
