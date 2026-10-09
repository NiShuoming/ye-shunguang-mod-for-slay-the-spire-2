using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 妙趣扭蛋 0 【】->【固有】【消耗】获得1张随机x费牌，添加消耗 完成
    /// </summary>
    public class GachaFun:YeCardModel
    {
        public GachaFun() : base(0,CardType.Skill,CardRarity.Rare,TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            List<CardPoolModel> list = base.Owner.UnlockState.CharacterCardPools.ToList();

            IEnumerable<CardModel> cards = from c in list.SelectMany((CardPoolModel c) => 
                c.GetUnlockedCards(base.Owner.UnlockState, 
                base.Owner.RunState.CardMultiplayerConstraint))
                where c.EnergyCost.CostsX select c;

            CardModel card = CardFactory.GetDistinctForCombat(base.Owner, cards, 1, 
                base.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();

            if(card != null )
            {
                card.AddKeyword(CardKeyword.Exhaust);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}
