using BaseLib.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZZZLib.Cmds;
using ZZZLib.Other;

namespace YeShunguang
{
    /// <summary>
    /// 五感尽失 0 消耗 消耗任意张张牌，每张获得1->2点【青溟剑势】，若【青溟剑势】已满
    /// ，获得1层【明心境】
    /// </summary>
    public class LossOfAllFiveSenses : YeCardModel
    {
        public LossOfAllFiveSenses() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<QingmingSwordForce>(1)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
            HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(
                choiceContext, Owner,
                new CardSelectorPrefs(base.SelectionScreenPrompt, 0, 10),
                null, this);
            int n = enumerable.Count();

            foreach (CardModel card in enumerable.ToList())
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars.Power<QingmingSwordForce>().IntValue * n, Owner.Creature, this);

            ZZZSecondEnergy energy = ZZZCmd.GetSecondEnergy(Owner);
            if (energy.Energy2 >= 6)
            {
                await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature,
                    1, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<QingmingSwordForce>().UpgradeValueBy(1m);
        }
    }
}
