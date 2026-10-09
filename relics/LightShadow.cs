using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZZZLib.Cmds;


namespace YeShunguang
{
    /// <summary>
    /// 光与影 青溟剑势上限增加 完成
    /// </summary>
    public class LightShadow : YeRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("Max", 3m)];

        public override int ModifyMaxSecondEnergy(Player player, int amount)
        {
            return amount + DynamicVars["Max"].IntValue;
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature) && combatState.RoundNumber <= 1)
            {
                Flash();
                await ZZZCmd.AddSecondEnergy(Owner, DynamicVars["Max"].IntValue, Owner.Creature, null);

            }
        }
    }
}
