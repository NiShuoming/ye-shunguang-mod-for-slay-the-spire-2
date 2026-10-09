using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;
using ZZZLib.Model;

namespace YeShunguang
{
    [Pool(typeof(YeShunguangRelicPool))]
    public abstract class YeRelicModel : CustomRelicModel, IZZZSecondEnergyHook
    {
        protected override string IconBaseName =>
            base.Id.Entry.RemovePrefix().ToLowerInvariant();
        public virtual Task AfterSecondEnergyChanged(Player player, int before, int after, CardModel cardModel)
        {
            return Task.CompletedTask;
        }

        public virtual decimal ModifyAnomalyDamageAdd(ICombatState combatState, Creature target, Creature dealer, decimal damage, CardModel cardSource, CardPlay cardPlay)
        {
            return 0;
        }

        public virtual decimal ModifyAnomalyDamageMulti(ICombatState combatState, Creature target, Creature dealer, decimal damage, CardModel cardSource, CardPlay cardPlay)
        {
            return 1;
        }

        public virtual int ModifyMaxSecondEnergy(Player player, int amount)
        {
            return amount;
        }

        public virtual int ModifySecondEnergyGain(Player player, int amount, Creature giver, CardModel cardModel)
        {
            return amount;
        }

    }
}
