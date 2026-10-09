using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 微光引 伙伴牌费用-1 完成
    /// </summary>
    public class GuidingGlimmer : YeRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
        { HoverTipFactory.FromKeyword(ZZZKeyWord.Partner)};


        public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
        {
            modifiedCost = originalCost;
            if (card.Owner.Creature != base.Owner.Creature)
            {
                return false;
            }
            if (!card.Keywords.Contains(ZZZKeyWord.Partner))
            {
                return false;
            }

            var flag = (card.Pile?.Type) switch
            {
                PileType.Hand or PileType.Play => true,
                _ => false,
            };
            if (!flag)
            {
                return false;
            }
            modifiedCost = originalCost - 1;
            return true;
        }
    }
}
