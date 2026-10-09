using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 掣惊雷 1 【记忆】造成4点伤害2次，本场造成伤害，使伤害+3=6
    /// </summary>
    public class LureThunder:YeCardModel
    {
        public LureThunder() : base(1,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(4m,ValueProp.Move), new RepeatVar(2),
            new DynamicVar("Increase",3m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(YeKeywords.Memory)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [YeKeywords.Memory];

        protected override bool ShouldGlowGoldInternal =>
    Owner.Creature.GetPowerAmount<EnlightenedMind>() > 0;

        private decimal _extraDamageFromPlays;

        public decimal ExtraDamageFromPlays
        {
            get
            {
                return _extraDamageFromPlays;
            }
            set
            {
                AssertMutable();
                _extraDamageFromPlays = value;
            }
        }

        protected override void AfterDowngraded()
        {
            base.AfterDowngraded();
            base.DynamicVars.Damage.BaseValue += ExtraDamageFromPlays;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            AttackCommand att = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            int count = att.Results.Sum(b =>b.Count((DamageResult d) => d.UnblockedDamage > 0));
            if(count>0)
            {
                base.DynamicVars.Damage.BaseValue += base.DynamicVars["Increase"].BaseValue;
                ExtraDamageFromPlays += base.DynamicVars["Increase"].BaseValue;
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Increase"].UpgradeValueBy(3m);
        }
    }
}
