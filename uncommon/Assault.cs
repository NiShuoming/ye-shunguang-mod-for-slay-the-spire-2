using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 攻势 1 造成8->11点伤害，获得1层【以太帷幕】，若突破格挡，获得1能量
    /// </summary>
    public class Assault:YeCardModel
    {
        public Assault() : base(1,CardType.Attack,CardRarity.Uncommon,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(8m,ValueProp.Move), new PowerVar<EtherVeil>(1m),
            new EnergyVar(1),new CardsVar(1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EtherVeil>(),EnergyHoverTip,
            HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await PowerCmd.Apply<EtherVeil>(choiceContext, Owner.Creature,
                DynamicVars.Power<EtherVeil>().BaseValue, Owner.Creature, this);

            AttackCommand a = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            if(a.Results.Count(a => a.Any(b=>b.WasBlockBroken)) > 0)
            {
                await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
                await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.IntValue
                    , Owner);
            }
            
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
