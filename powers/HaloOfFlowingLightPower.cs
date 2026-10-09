using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 流光萦绕p 明心境期间每回合获得活力 完成
    /// </summary>
    public class HaloOfFlowingLightPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
        {
            if(Owner.GetPower<EnlightenedMind>()!=null && Owner.Player == player)
            {
                Flash();
                await PowerCmd.Apply<VigorPower>(choiceContext, Owner, Amount, Owner, null);
            }
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            if (power is EnlightenedMind && power.Owner == Owner && amount > 0)
            {
                Flash();
                await PowerCmd.Apply<VigorPower>(choiceContext, Owner, Amount, Owner, null);
            }
        }
    }
}
