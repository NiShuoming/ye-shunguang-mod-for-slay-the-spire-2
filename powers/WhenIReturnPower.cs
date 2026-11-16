using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 归去时p 本回合脆弱敌人伤减少
    /// </summary>
    public class WhenIReturnPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource,CardPlay cardPlay)
        {
            if(target==Owner && dealer.GetPower<FrailPower>() != null 
                && props == ValueProp.Move)
            {
                if (amount - Amount < 0) return -amount;
                return -Amount;
            }
            return 0;
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player == Owner.Player)
                await PowerCmd.Remove<WhenIReturnPower>(Owner);
        }
    }
}
