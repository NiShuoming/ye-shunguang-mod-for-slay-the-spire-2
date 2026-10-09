using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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
    /// 斩流光极 2 【记忆】对所有敌人造成13->17点伤害，若有，消耗2层【青溟剑势】获得6-8伤害 完成
    /// </summary>
    public class SunderlightMaximum : YeCardModel
    {
        public SunderlightMaximum() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CalculationBaseVar(13m),new ExtraDamageVar(6m), new PowerVar<QingmingSwordForce>(2),
            new CalculatedDamageVar(ValueProp.Move).
            WithMultiplier((card, _) =>
            {
                if(ZZZCmd.GetSecondEnergy(card.Owner).Energy2>=2)
                {
                    return 1;
                }
                else return 0;
            }
            )];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [YeKeywords.Memory];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override HashSet<CardTag> CanonicalTags =>
            [YeTags.Sunderlight];

        protected override bool ShouldGlowGoldInternal =>
                ZZZCmd.GetSecondEnergy(Owner).Energy2 >= DynamicVars.Power<QingmingSwordForce>().IntValue
                    || Owner.Creature.GetPowerAmount<EnlightenedMind>() > 0;


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int n = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            if (n >= DynamicVars.Power<QingmingSwordForce>().IntValue)
            {
                await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this, cardPlay).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

                await ZZZCmd.AddSecondEnergy(Owner, -DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);
            }
            else
            {
                await DamageCmd.Attack(DynamicVars.CalculationBase.BaseValue)
                .FromCard(this, cardPlay).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(4m);
            DynamicVars.ExtraDamage.UpgradeValueBy(2m);
        }
    }
}
