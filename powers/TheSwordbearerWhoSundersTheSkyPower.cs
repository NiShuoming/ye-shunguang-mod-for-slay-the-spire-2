using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
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
    /// 步溟仗剑开天光p 抽到伙伴获得帷幕 和格挡
    /// </summary>
    public class TheSwordbearerWhoSundersTheSkyPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel clonedBy)
        {
            if (card.Owner.Creature == Owner && card.Keywords.Contains(ZZZKeyWord.Partner) 
                && card.Pile.Type == PileType.Hand && oldPileType != PileType.Hand)
            {
                await PowerCmd.Apply<EtherVeil>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
                await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
            }
        }
        /*
        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.Keywords.Contains(YeKeywords.Partner))
            {
                await PowerCmd.Apply<EtherVeil>(context,Owner, Amount, Owner, null);
            }
        }*/
    }
}
