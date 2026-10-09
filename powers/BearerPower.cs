using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
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
    /// 载物p 本回合获得的剑势翻倍 已禁用
    /// </summary>
    public class BearerPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, Creature target, CardModel cardSource)
        {
            if (power is QingmingSwordForce && power.Owner == target)
            {
                Flash();
                return 2;
            }
            return 1;
        }

        /*
        public override decimal aModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature target, CardModel cardSource)
        {
            if(power is QingmingSwordForce && power.Owner == target)
            {
                Flash();
                return amount * 2;
            }
            return amount;
        }*/

        //减少层数
        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Enemy)
            {
                await PowerCmd.TickDownDuration(this);
            }
        }
    }
}
