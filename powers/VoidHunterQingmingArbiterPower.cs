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

namespace YeShunguang
{
    /// <summary>
    /// 虚狩buff 给所有敌人+6格挡，x费+x次数
    /// </summary>
    public class VoidHunterQingmingArbiterPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override int ModifyXValue(CardModel card, int originalValue)
        {
            if(card !=null && card.Owner.Creature == Owner)
                return originalValue + Amount;
            return originalValue;
        }


    }
}
