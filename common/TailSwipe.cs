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
    /// 横扫 1 对所有敌人造成8->11点伤害，施加1层脆弱 完成
    /// </summary>
    public class TailSwipe:YeCardModel
    {
        public TailSwipe() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(8m,ValueProp.Move),new PowerVar<FrailPower>(1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<FrailPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).
             FromCard(this, cardPlay).
             TargetingAllOpponents(base.CombatState).
             WithHitFx("vfx/vfx_attack_slash").
             Execute(choiceContext);

            await PowerCmd.Apply<FrailPower>(choiceContext,CombatState.Enemies, DynamicVars.Power<FrailPower>().BaseValue,
                Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }


    }
}
