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
    /// 斩妄开天 x 对随机敌人造成14-17点伤害x次，
    /// 每有1层【青溟剑势】伤害-1 完成
    /// </summary>
    public class CleavingHeavens : YeCardModel
    {
        public CleavingHeavens() : base(0, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy) { }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new CalculationBaseVar(14m),new ExtraDamageVar(-1),
            new CalculatedDamageVar(ValueProp.Move).
            WithMultiplier((card,_)=>(ZZZCmd.GetSecondEnergy(card.Owner).Energy2))];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int num = ResolveEnergyXValue();
            await DamageCmd.Attack(base.DynamicVars.CalculatedDamage)
                .WithHitCount(num)
                .FromCard(this,cardPlay).TargetingRandomOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(3m);
        }
    }
}
