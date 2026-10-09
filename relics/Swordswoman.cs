using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
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
    /// 剑客行 开局获得一张飞光 完成
    /// </summary>
    public class Swordswoman : YeRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        bool on = true;

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature) && combatState.RoundNumber <= 1 && on)
            {
                on = false;
                Flash();
                SoaringLight soaringLight = Owner.Creature.CombatState?.CreateCard<SoaringLight>(Owner);
                if (soaringLight != null)
                    await CardPileCmd.AddGeneratedCardToCombat(soaringLight, PileType.Hand, Owner);
            }
        }
        /*
        public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
        {
            if (side == base.Owner.Creature.Side && combatState.RoundNumber <= 1)
            {
                Flash();
                SoaringLight soaringLight = Owner.Creature.CombatState?.CreateCard<SoaringLight>(Owner);
                if(soaringLight != null) 
                    await CardPileCmd.AddGeneratedCardToCombat(soaringLight,PileType.Hand, true);
            }
        }*/
    }
}
