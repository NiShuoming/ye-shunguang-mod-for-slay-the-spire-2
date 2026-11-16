using MegaCrit.Sts2.Core.Combat;
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
    /// 青溟剑剑主p 消耗牌获得力量 完成
    /// </summary>
    public class TheMasterOfQingmingSwordPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private decimal _power;

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if(card.Owner.Creature == Owner )
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner,Amount,Owner,null);
                _power += Amount;
            }
        }
        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (_power > 0 && side == CombatSide.Player)
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -_power, Owner, null);
                _power = 0;
            }
        }
        /*
        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (_power > 0)
            {
                await PowerCmd.Apply<StrengthPower>(Owner,-_power,Owner,null);
                _power = 0;
            }
        }*/
    }
}
