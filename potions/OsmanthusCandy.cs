using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 桂花糖 从消耗堆中选择一张牌加入手牌 卧槽
    /// </summary>
    public class OsmanthusCandy : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Uncommon;

        public override PotionUsage Usage => PotionUsage.CombatOnly;

        public override TargetType TargetType => TargetType.Self;

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
        {
            CardSelectorPrefs prefs = new(base.SelectionScreenPrompt, 1);
            CardPile pile = PileType.Exhaust.GetPile(Owner);
            CardModel cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs)).FirstOrDefault();
            if (cardModel != null) 
            {
                await CardPileCmd.Add(cardModel, PileType.Hand);
            }

        }
    }
}
