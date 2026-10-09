using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 剑引青霜p 明心境结束获得能量 完成
    /// </summary>
    public class FrostedBladePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext,PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            if(power.Owner == Owner && amount<0 && 
                power is EnlightenedMind && power.Amount <= 0 
                && Owner.IsPlayer)
            {
                await PlayerCmd.GainEnergy(Amount, Owner.Player);
            }
        }
    }
}
