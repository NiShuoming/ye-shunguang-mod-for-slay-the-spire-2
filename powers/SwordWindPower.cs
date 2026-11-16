using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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
    /// 剑风p 获得明心境时，打出带有【斩流光】的卡牌时，对所有敌人造成伤害,获得格挡 完成
    /// </summary>
    public class SwordWindPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if(Owner.IsPlayer && cardPlay.Card.Owner == Owner.Player 
                && cardPlay.Card.Tags.Contains(YeTags.Sunderlight))
            {
                await CreatureCmd.Damage(context, CombatState.HittableEnemies, 
                    Amount,ValueProp.Unpowered, base.Owner);

                await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);

            }
        }
    }
}
