using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 锵鸣琳琅 获得明心境时，多获得1层 完成
    /// </summary>
    public class ClangOfJade : YeRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Shop;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<EnlightenedMind>()];

        public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature target, CardModel cardSource)
        {
            if (power is EnlightenedMind && Owner.Creature == target)
            {
                return 1;
            }
            return 0;
        }/*
        public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
        {
            if(power is  EnlightenedMind && Owner.Creature == target)
            {
                return amount + 1;
            }
            return amount;
        }*/
    }
}
