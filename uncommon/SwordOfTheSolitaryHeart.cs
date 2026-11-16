using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 剑映孤心 1 给予3=4脆弱，并造成双（3）量伤害 完成
    /// </summary>
    public class SwordOfTheSolitaryHeart:YeCardModel
    {
        public SwordOfTheSolitaryHeart() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<FrailPower>(2m), new DynamicVar("multi", 2)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<FrailPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            VoiceManager.PlayAttackVoice(Owner.Creature.HasPower<EnlightenedMind>());
            await PowerCmd.Apply<FrailPower>(choiceContext, cardPlay.Target, DynamicVars.Power<FrailPower>().BaseValue
                , Owner.Creature, this);

            FrailPower power = cardPlay.Target.GetPower<FrailPower>();
            if (power != null)
            {
                await DamageCmd.Attack(power.Amount * DynamicVars["multi"].BaseValue)
                    .FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash", tmpSfx: "打击音效.mp3")
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<FrailPower>().UpgradeValueBy(1m);
            DynamicVars["multi"].UpgradeValueBy(1);
        }
    }
 }