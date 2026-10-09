using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 千万次初见 击破格挡时，打出抽牌堆中的一张【伙伴】
    /// </summary>
    public class AThousandFirstMeetingsPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(ZZZKeyWord.Partner)];

        private bool _did = false;

        public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            _did = false;
            return Task.CompletedTask;
        }

        public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature dealer, CardModel cardSource)
        {
            if (result.WasBlockBroken && !_did && dealer==Owner && Owner.IsPlayer)
            {
                CardPile cardPile = PileType.Draw.GetPile(Owner.Player);
                var list = cardPile.Cards.ToList();
                for(int i = 0; i < Amount; i++)
                {
                    if (list != null)
                    {
                        CardModel card = list.Find(a => a.Keywords.ToList().Contains(ZZZKeyWord.Partner));
                        if (card != null)
                        {
                            Flash();
                            await CardCmd.AutoPlay(choiceContext, card, null);
                            _did = true;
                        }

                    }
                }

            }
        }
    }
}
