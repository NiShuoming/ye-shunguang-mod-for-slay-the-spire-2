using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
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
    /// 流云剑意 造成伤害7-10伤害x次
    /// </summary>
    public class CloudstreamSwordWill:YeCardModel
    {
        protected override bool HasEnergyCostX => true;

        public CloudstreamSwordWill() : base(0,CardType.Attack,CardRarity.Common,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(7m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            int num = ResolveEnergyXValue();
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).
                WithHitCount(num).FromCard(this, cardPlay).
                Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").
                Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
