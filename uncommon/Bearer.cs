using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;

namespace YeShunguang
{
    /// <summary>
    /// 载物 1 消耗手牌中能耗小于等于0和状态的卡，每消耗1张牌，获得1青溟剑势，抽1张牌
    /// </summary>
    public class Bearer : YeCardModel
    {
        public Bearer() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CardsVar(1), new PowerVar<QingmingSwordForce>(1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardPile pile = PileType.Hand.GetPile(Owner);
            List<CardModel> enumerable2 = pile.Cards.Where(a =>
            (a.EnergyCost.Canonical <= 0) || a.Type == CardType.Status).ToList();

            int n = enumerable2.Count;
            foreach (CardModel card in enumerable2)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
            if (n > 0)
            {
                await ZZZCmd.AddSecondEnergy(Owner, n * DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);
                await CardPileCmd.Draw(choiceContext, n * DynamicVars.Cards.IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
