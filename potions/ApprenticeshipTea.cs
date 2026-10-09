using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cards.Partner;

namespace YeShunguang
{
    /// <summary>
    /// 将一张 仪玄 加入手牌
    /// </summary>
    public class ApprenticeshipTea : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Rare;

        public override PotionUsage Usage => PotionUsage.CombatOnly;

        public override TargetType TargetType => TargetType.Self;

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
        {
            if (!target.IsPlayer || target.Player == null) return;
            await CardPileCmd.AddGeneratedCardToCombat(target.CombatState.CreateCard<Yixuan>(target.Player),
                 PileType.Hand, Owner);

        }
    }
}
