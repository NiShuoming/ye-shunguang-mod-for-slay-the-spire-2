using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseLib.Utils.BetaMainCompatibility;

namespace YeShunguang
{
    /// <summary>
    /// 旧梦回溯于此刻p 每回合第一次消耗卡牌时，向弃牌堆中添加一张复制品
    /// </summary>
    public class WhenOldDreamsResurfacePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        private bool _reset = true;

        public override Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
        {
            _reset = true;
            return Task.CompletedTask;
        }
     
        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if(card.Owner.Creature == Owner && _reset && Owner.IsPlayer)
            {
                _reset = false;
                CardModel created = card.CreateClone();
                await CardPileCmd.AddGeneratedCardToCombat(created, PileType.Discard, Owner.Player);
            }
        }
    }
}
