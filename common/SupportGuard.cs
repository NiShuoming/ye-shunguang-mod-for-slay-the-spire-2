using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.OpenXRCompositionLayer;

namespace YeShunguang
{
    /// <summary>
    /// 援守 1 获得8=11格挡，将弃牌堆中的1加入抽牌牌
    /// </summary>
    public class SupportGuard:YeCardModel
    {
        public SupportGuard(): base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }
        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new BlockVar(8m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            CardSelectorPrefs prefs = new(base.SelectionScreenPrompt, 1);
            CardModel cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, 
                PileType.Discard.GetPile(base.Owner).Cards, base.Owner, prefs)).FirstOrDefault();
            if(cardModel != null)
                await CardPileCmd.Add(cardModel, PileType.Draw,CardPilePosition.Random);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}
