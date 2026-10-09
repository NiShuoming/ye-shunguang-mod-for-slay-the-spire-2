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
    /// 流云萦绕 1 明心境期间，每回合获得5-7活力
    /// </summary>
    public class HaloOfFlowingLight:YeCardModel
    {
        public HaloOfFlowingLight() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<VigorPower>(5m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<VigorPower>(),
            HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<HaloOfFlowingLightPower>(choiceContext, Owner.Creature,
                DynamicVars.Power<VigorPower>().BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<VigorPower>().UpgradeValueBy(2m);
        }
    }
}
