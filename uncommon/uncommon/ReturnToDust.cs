using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;

namespace YeShunguang
{
    /// <summary>
    /// 归尘 x 【记忆】只有在【明心境】状态下可打出。对所有敌人造成6->9点伤害x次，获得2剑势 完成
    /// </summary>
    public class ReturnToDust : YeCardModel
    {
        public ReturnToDust() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(6m,ValueProp.Move),
            new PowerVar<QingmingSwordForce>(2m)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [YeKeywords.Memory];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromPower<EnlightenedMind>(),
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override bool ShouldGlowGoldInternal => IsPlayable;

        protected override bool IsPlayable => Owner?.Creature.GetPower<EnlightenedMind>() != null;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int xValue = ResolveEnergyXValue();
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).
               WithHitCount(xValue).FromCard(this, cardPlay).
               TargetingAllOpponents(base.CombatState).
               WithHitFx("vfx/vfx_attack_slash").
               Execute(choiceContext);

            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
