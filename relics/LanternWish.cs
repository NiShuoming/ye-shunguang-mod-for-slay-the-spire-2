using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
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
    /// 明灯愿 敌人获得格挡时，获得2格挡
    /// </summary>
    public class LanternWish : YeRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new BlockVar(2, ValueProp.Unpowered)];

        public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel cardSource)
        {
            if (creature.IsEnemy)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
            }
        }
    }
}
