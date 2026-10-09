using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 日记 1 消耗 【】=【保留】复制1张卡
    /// </summary>
    public class Diary:YeCardModel
    {
        public Diary():base(1,CardType.Skill,CardRarity.Uncommon,TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardModel cardModel = (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 1),
                context: choiceContext, player: base.Owner,
                filter: null,
                source: this)).FirstOrDefault();

            if (cardModel != null)
            {
                await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
                CardModel created = cardModel.CreateClone();
                await CardPileCmd.AddGeneratedCardToCombat(created, PileType.Hand, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            this.AddKeyword(CardKeyword.Retain);
        }
    }
}
