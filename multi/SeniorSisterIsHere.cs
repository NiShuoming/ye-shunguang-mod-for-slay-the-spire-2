using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 师姐在此 2 所有玩家获得2=3层以太帷幕
    /// </summary>
    public class SeniorSisterIsHere:YeCardModel
    {
        public SeniorSisterIsHere() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies) { }

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<EtherVeil>(2m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EtherVeil>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<EtherVeil>(choiceContext, Owner.Creature,
                DynamicVars.Power<EtherVeil>().BaseValue,
                Owner.Creature, this);
            foreach(Creature c in CombatState.Allies)
            {
                await PowerCmd.Apply<EtherVeil>(choiceContext, c,
                DynamicVars.Power<EtherVeil>().BaseValue,
                Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<EtherVeil>().UpgradeValueBy(1m);
        }
    }
}
