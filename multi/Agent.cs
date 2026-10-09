using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 代理人 2 【消耗】所有玩家减少1=2层debuff
    /// </summary>
    public class Agent:YeCardModel
    {
        public Agent() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies) { }

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
                [new DynamicVar("Count", 2m)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var ps = Owner.Creature.Powers.ToList();
            foreach (PowerModel p in ps)
            {
                if (p.Type == PowerType.Debuff)
                    await PowerCmd.ModifyAmount(choiceContext,p, -DynamicVars["Count"].BaseValue,
                        Owner.Creature, this);
            }
            foreach(Creature c in CombatState.Allies)
            {
                ps = c.Powers.ToList();
                foreach (PowerModel p in ps)
                {
                    if (p.Type == PowerType.Debuff)
                        await PowerCmd.ModifyAmount(choiceContext, p, -DynamicVars["Count"].BaseValue,
                            Owner.Creature, this);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Count"].UpgradeValueBy(1m);
        }
    }
}
