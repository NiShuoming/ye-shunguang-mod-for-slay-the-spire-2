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
using ZZZLib.Cmds;

namespace YeShunguang
{
    /// <summary>
    /// 宠物与责任 0 获得1=2能量，消耗1剑势多1能量 完成
    /// </summary>
    public class PetsAndResponsibilities : YeCardModel
    {
        public PetsAndResponsibilities() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new EnergyVar(1), new EnergyVar("Extra", 1)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>(),
            EnergyHoverTip];
        protected override bool ShouldGlowGoldInternal =>
            ZZZCmd.GetSecondEnergy(Owner).Energy2 >= 1;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            int num = ZZZCmd.GetSecondEnergy(Owner).Energy2;
            if (num > 0)
            {
                await ZZZCmd.AddSecondEnergy(Owner, -1, Owner.Creature, this);
                await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue
                    + DynamicVars["Extra"].IntValue, Owner);
            }
            else
            {
                await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
            }
        }
        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1);
        }
    }
}
