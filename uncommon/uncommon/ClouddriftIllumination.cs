using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 流云照影 1=0 【消耗】将1记忆或伙伴从消耗堆加入手牌
    /// </summary>
    public class ClouddriftIllumination : YeCardModel
    {
        public ClouddriftIllumination() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(ZZZKeyWord.Partner),
            HoverTipFactory.FromKeyword(YeKeywords.Memory)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CardsVar(3)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardSelectorPrefs prefs = new(base.SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue);
            CardPile pile = PileType.Exhaust.GetPile(Owner);
            List<CardModel> cardModels = [.. (await CardSelectCmd.FromSimpleGrid(choiceContext,
                    [.. pile.Cards.Where(a=>(a.Keywords.Contains(ZZZKeyWord.Partner)||
                    a.Keywords.Contains(YeKeywords.Memory)))], base.Owner, prefs))];
            if (cardModels != null)
            {
                await CardPileCmd.Add(cardModels, PileType.Hand);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
