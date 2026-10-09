using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 燕袭 1 造成8->11点伤害，如果上一张是技能牌，获得4活力
    /// </summary>
    public class SwallowStrike:YeCardModel
    {
        public SwallowStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(8m,ValueProp.Move),new PowerVar<VigorPower>(4m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<VigorPower>()];

        protected override HashSet<CardTag> CanonicalTags => 
            [CardTag.Strike];

        bool isSkill = false;

        protected override bool ShouldGlowGoldInternal => isSkill;

        public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if(cardPlay.Card.Owner!=Owner) return Task.CompletedTask;
            if(cardPlay.Card.Type == CardType.Skill)
            {
                isSkill = true;
            }
            else isSkill = false;
            return Task.CompletedTask;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if(isSkill) await PowerCmd.Apply<VigorPower>(choiceContext,Owner.Creature,
                DynamicVars.Power<VigorPower>().BaseValue, Owner.Creature,
                this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3);
            //DynamicVars.Power<VigorPower>().UpgradeValueBy(1);
        }
    }
}
