using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 纳剑 1 为目标施加4格挡，抽3=4牌 完成
    /// </summary>
    public class SwordSheathe:YeCardModel
    {
        public SwordSheathe() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("EnemyBlock", 4), new CardsVar(3)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
            await CreatureCmd.GainBlock(cardPlay.Target, base.DynamicVars["EnemyBlock"].BaseValue,
                ValueProp.Move, null);
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}
