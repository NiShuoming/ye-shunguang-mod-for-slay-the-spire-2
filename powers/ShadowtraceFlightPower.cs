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
using ZZZLib.Cmds;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 溯影惊鸿p 开启【以太帷幕】会获得1点【青溟剑势】
    /// </summary>
    public class ShadowtraceFlightPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            if (power is EtherVeil && power.Owner == Owner && amount > 0 && Owner.IsPlayer)
            {
                Flash();
                await ZZZCmd.AddSecondEnergy(Owner.Player, Amount, Owner, null);

            }
        }
    }
}
