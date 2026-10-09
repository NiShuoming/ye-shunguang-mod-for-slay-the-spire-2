using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 合道 给予一位玩家8=》11活力
    /// </summary>
    public class Unity:YeCardModel
    {
        public Unity() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer) { }

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<VigorPower>(8)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<VigorPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<VigorPower>(choiceContext, cardPlay.Target, DynamicVars.Power<VigorPower>().BaseValue,
                Owner.Creature, this);
        }
        protected override void OnUpgrade()
        {
            DynamicVars.Power<VigorPower>().UpgradeValueBy(3);
        }
    }
}
