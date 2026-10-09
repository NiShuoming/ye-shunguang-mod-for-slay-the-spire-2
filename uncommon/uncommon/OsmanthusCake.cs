using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib.Cmds;

namespace YeShunguang
{
    /// <summary>
    /// 桂花糕 1=0 【消耗】将【青溟剑势】直接转化为能量 完成
    /// </summary>
    public class OsmanthusCake : YeCardModel
    {
        public OsmanthusCake() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            [CardKeyword.Exhaust];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
            EnergyHoverTip];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int n = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            if (n > 0)
            {
                await ZZZCmd.AddSecondEnergy(Owner, -n, Owner.Creature, this);
                await PlayerCmd.GainEnergy(n, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
