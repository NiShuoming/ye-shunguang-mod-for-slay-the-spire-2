using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 献给自己的花束 1 消耗 【消耗】给予目标4格挡，1=2覆甲，给予自己6=8覆甲。 完成
    /// </summary>
    public class ABouquetForMyself:YeCardModel
    {
        public ABouquetForMyself() : base(1,CardType.Power, CardRarity.Rare,TargetType.AnyEnemy) { }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<PlatingPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DynamicVar("EnemyBlock", 4), new PowerVar<PlatingPower>(6m),
            new DynamicVar("PlatingEnemy",2)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await CreatureCmd.GainBlock(cardPlay.Target, DynamicVars["EnemyBlock"].BaseValue,
                ValueProp.Move, null);
            await PowerCmd.Apply<PlatingPower>( choiceContext,cardPlay.Target,
                DynamicVars["PlatingEnemy"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature,
                DynamicVars.Power<PlatingPower>().BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<PlatingPower>().UpgradeValueBy(2m);
            DynamicVars["PlatingEnemy"].UpgradeValueBy(1m);
        }
    }
}
