using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 抱一p  使伙伴对目标伤害+35%defbuff翻倍 完成
    /// </summary>
    public class UnificationPower : PowerModel
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
        {
            if (target == Owner && cardSource != null && cardSource.Keywords.Contains(ZZZKeyWord.Partner))
            {
                return 1.35m;
            }
            return 1;
        }

        public override decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, Creature target, CardModel cardSource)
        {
            if (target == Owner && cardSource != null && cardSource.Keywords.Contains(ZZZKeyWord.Partner))
            {
                return 2;
            }
            return 1;
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Enemy)
            {
                await PowerCmd.TickDownDuration(this);
            }
        }
    }
}
