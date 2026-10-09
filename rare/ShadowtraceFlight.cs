using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Powers;

namespace YeShunguang
{
    /// <summary>
    /// 溯影惊鸿 1 开启【以太帷幕】会获得2点【青溟剑势】，升级后开启1层【以太帷幕】 完成
    /// </summary>
    public class ShadowtraceFlight:YeCardModel
    {
        public ShadowtraceFlight() : base(1,CardType.Power,CardRarity.Rare,TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<QingmingSwordForce>(2)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromPower<EtherVeil>(),
            HoverTipFactory.FromPower<QingmingSwordForce>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            
            await PowerCmd.Apply<ShadowtraceFlightPower>(choiceContext, Owner.Creature,
                DynamicVars.Power<QingmingSwordForce>().BaseValue, Owner.Creature, this);
            if (IsUpgraded) await PowerCmd.Apply<EtherVeil>(choiceContext, Owner.Creature,
                1m, Owner.Creature, this);
        }
    }
}
