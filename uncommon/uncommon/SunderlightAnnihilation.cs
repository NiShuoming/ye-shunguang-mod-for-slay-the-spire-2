using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
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
    /// 斩流光灭 3 【记忆】对目标造成28->36点伤害,若有，消耗3层【青溟剑势】，再次造成伤害 完成
    /// </summary>
    public class SunderlightAnnihilation : YeCardModel
    {
        public SunderlightAnnihilation() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(28m, ValueProp.Move), new PowerVar<QingmingSwordForce>(3m)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [YeKeywords.Memory];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override HashSet<CardTag> CanonicalTags =>
            [YeTags.Sunderlight];

        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= DynamicVars.Power<QingmingSwordForce>().IntValue
            || Owner.Creature.GetPowerAmount<EnlightenedMind>()>0;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            AttackCommand a = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this,cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (a.Results.Any(b => b.Any(c => c.WasTargetKilled))) return;
            int n = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            if (n >= DynamicVars.Power<QingmingSwordForce>().IntValue)
            {
                await ZZZCmd.AddSecondEnergy(Owner, -DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);
                await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this,cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(8m);
        }
    }
}
