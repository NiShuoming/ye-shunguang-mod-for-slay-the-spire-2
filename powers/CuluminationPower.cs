using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;
using ZZZLib.Model;

namespace YeShunguang
{
    /// <summary>
    /// 观止p 少消耗剑势 完成
    /// </summary>
    public class CuluminationPower : PowerModel, IZZZSecondEnergyHook
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public Task AfterSecondEnergyChanged(Player player, int before, int after, CardModel cardModel)
        {
            return Task.CompletedTask;
        }

        public int ModifyMaxSecondEnergy(Player player, int amount)
        {
            return amount;
        }

        public int ModifySecondEnergyGain(Player player, int amount, Creature giver, CardModel cardModel)
        {
            if (amount < 0)
            {
                amount = Math.Clamp(amount + Amount, amount, 0);
            }
            return amount;
        }
        /*
        public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature target, CardModel cardSource)
        {
            if (power is QingmingSwordForce && target == Owner && amount < 0)
            {
                Flash();
                return Amount;
            }
            return 0;
        }
        
        public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature target, CardModel cardSource)
        {
            if(power is QingmingSwordForce && target==Owner && amount < 0)
            {
                Flash();
                return Math.Clamp(amount + Amount, amount, 0);
            }
            return amount;
        }*/
    }
}
