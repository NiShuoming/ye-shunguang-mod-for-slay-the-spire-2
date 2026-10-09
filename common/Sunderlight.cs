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

namespace YeShunguang
{
    /// <summary>
    /// 斩流光 2 记忆 造成7=10点伤害2次，4格挡 完成
    /// </summary>
    public class Sunderlight:YeCardModel
    {
        public override bool GainsBlock => true;

        public Sunderlight() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(7m,ValueProp.Move),new RepeatVar(2),
            new BlockVar(4m,ValueProp.Move)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [YeKeywords.Memory];

        protected override HashSet<CardTag> CanonicalTags => 
            [YeTags.Sunderlight];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(YeKeywords.Memory),];
        protected override bool ShouldGlowGoldInternal =>
    Owner.Creature.GetPowerAmount<EnlightenedMind>() > 0;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);

        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
