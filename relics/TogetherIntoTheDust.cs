using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 共黯尘 开启以太帷幕对所有敌人施加1易伤 完成
    /// </summary>
    public class TogetherIntoTheDust : YeRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
        { new PowerVar<VulnerablePower>(1m)};

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
        { HoverTipFactory.FromPower<EtherVeil>(),HoverTipFactory.FromPower<VulnerablePower>()};

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power is EtherVeil && amount > 0 && power.Owner == Owner.Creature )
            {
                Flash();
                await PowerCmd.Apply<VulnerablePower>(choiceContext,Owner.Creature.CombatState?.HittableEnemies, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, null);
            }
        }
    }
}
