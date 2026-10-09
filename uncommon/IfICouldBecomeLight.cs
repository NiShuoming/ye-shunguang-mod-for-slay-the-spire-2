using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;

namespace YeShunguang
{
    /// <summary>
    /// 若能化作光芒 0 消耗抽3=4牌，耗1剑势抽1 完成
    /// </summary>
    public class IfICouldBecomeLight : YeCardModel
    {
        public IfICouldBecomeLight() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CardsVar(3), new DynamicVar("Extra", 1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust];

        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= 1;


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            int num = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            if (num > 0)
            {
                await ZZZCmd.AddSecondEnergy(Owner, -1, Owner.Creature, this);
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue
                    + DynamicVars["Extra"].IntValue, Owner);
            }
            else
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}
