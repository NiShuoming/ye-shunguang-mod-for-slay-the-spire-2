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
using ZZZLib.Other;

namespace YeShunguang
{
    /// <summary>
    /// 照影 2 对所有敌人造成12->16伤害，若青溟剑势已满，获得2层【明心境】 完成
    /// </summary>
    public class IlluminatingDarkness : YeCardModel
    {
        public IlluminatingDarkness() : base(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(12m, ValueProp.Move), new PowerVar<EnlightenedMind>(2m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()
            ,HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override bool ShouldGlowGoldInternal =>
    ZZZCmd.GetSecondEnergy(Owner).Energy2 >= ZZZCmd.GetSecondEnergy(Owner).MaxEnergy2;


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this,cardPlay).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            ZZZSecondEnergy energy2 = ZZZCmd.GetSecondEnergy(Owner);

            if (energy2.Energy2 >= 6)
            {
                await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature,
                    DynamicVars.Power<EnlightenedMind>().BaseValue,
                    Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}
