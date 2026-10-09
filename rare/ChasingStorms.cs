using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZZZLib;
using ZZZLib.Cmds;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 逐云惊霆 3 消耗 回满3=4【青溟剑势】，获得2层【明心境】状态，并对所有敌人造成14->20伤
    /// </summary>
    public class ChasingStorms : YeCardModel
    {
        public ChasingStorms() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DamageVar(14m,ValueProp.Move), new PowerVar<EtherVeil>(1),
            new PowerVar<EnlightenedMind>(2),new PowerVar<QingmingSwordForce>(3m)];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust, ZZZKeyWord.Ultimate];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
            HoverTipFactory.FromPower<EnlightenedMind>(),
            HoverTipFactory.FromKeyword(ZZZKeyWord.Ultimate)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await ZZZCmd.AddSecondEnergy(Owner, DynamicVars.Power<QingmingSwordForce>().IntValue, Owner.Creature, this);

            await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature,
                DynamicVars.Power<EnlightenedMind>().BaseValue,
                Owner.Creature, this);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                     .FromCard(this, cardPlay).TargetingAllOpponents(CombatState)
                     .WithHitFx("vfx/vfx_attack_slash")
                     .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(6m);
            DynamicVars.Power<QingmingSwordForce>().UpgradeValueBy(1m);
        }
    }
}
