using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
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
    /// 每消耗1点第二能量，获得格挡
    /// </summary>
    public class WhenMemoriesDriftByPower : PowerModel, IZZZSecondEnergyHook
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
        /*
        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            if(power is QingmingSwordForce && amount <0 && power.Owner == Owner)
            {
                Flash();
                await CreatureCmd.GainBlock(Owner, new BlockVar(-amount * Amount, ValueProp.Unpowered)
                    , null);
            }
        }*/

        public async Task AfterSecondEnergyChanged(Player player, int before, int after, CardModel cardModel)
        {
            if (after < before)
            {
                Flash();

                await CreatureCmd.GainBlock(Owner, new BlockVar((before - after) * Amount, ValueProp.Unpowered), null);

            }
        }

        public int ModifyMaxSecondEnergy(Player player, int amount)
        {
            return amount;
        }

        public int ModifySecondEnergyGain(Player player, int amount, Creature giver, CardModel cardModel)
        {
            return amount;
        }
    }
}
