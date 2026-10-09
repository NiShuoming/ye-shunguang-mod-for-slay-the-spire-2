using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZZZLib;
using ZZZLib.Cmds;
using ZZZLib.Other;

namespace YeShunguang
{
    /// <summary>
    /// 照破无明 完成 攻击牌加剑势
    /// </summary>

    public class BurningClartity : YeRelicModel
    {
        private int _counts = 0;

        private bool _isActivating = false;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            [HoverTipFactory.FromPower<QingmingSwordForce>()];

        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("Count", 2m), new DynamicVar("Force", 1m)];

        private bool on = true;

        public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side == base.Owner.Creature.Side && combatState.RoundNumber <= 1 && on)
            {
                on = false;
                Owner.PlayerCombatState.GetSecondEnergy(Owner);
            }
            return Task.CompletedTask;

        }

        public override bool ShowCounter => true;

        public override int DisplayAmount
        {
            get
            {
                if (!IsActivating)
                {
                    return _counts;
                }
                return base.DynamicVars["Count"].IntValue;
            }
        }

        private bool IsActivating
        {
            get
            {
                return _isActivating;
            }
            set
            {
                AssertMutable();
                _isActivating = value;
                InvokeDisplayAmountChanged();
            }
        }

        [SavedProperty]
        public int TurnsSeen
        {
            get
            {
                return _counts;
            }
            set
            {
                AssertMutable();
                _counts = value;
                InvokeDisplayAmountChanged();
            }
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Type == CardType.Attack && cardPlay
                .Card.Owner == Owner)
            {
                await AddCount(choiceContext);
            }
        }

        public async Task AddCount(PlayerChoiceContext choiceContext)
        {
            //处于明心境时不添加
            if (Owner.Creature.GetPower<EnlightenedMind>() != null && Owner.Creature.GetPower<WhiteHairFormPower>()==null)
                return;
            _counts++;
            InvokeDisplayAmountChanged();
            //满次数添加青溟剑势
            if (_counts >= DynamicVars["Count"].IntValue)
            {
                await ZZZCmd.AddSecondEnergy(Owner, DynamicVars["Force"].IntValue, Owner.Creature, null);
                /*
                await PowerCmd.Apply<QingmingSwordForce>(choiceContext,
                    Owner.Creature, DynamicVars["Force"].IntValue, Owner.Creature, null);*/

                _counts %= DynamicVars["Count"].IntValue;
                await DoActivateVisuals();
            }
        }

        private async Task DoActivateVisuals()
        {
            IsActivating = true;    // 切换到激活显示
            Flash();                // 闪烁效果
            await Cmd.Wait(.1f);     // 等待0.1秒
            IsActivating = false;   // 恢复正常显示
        }

        public override async Task AfterSecondEnergyChanged(Player player, int before, int after, CardModel cardModel)
        {
            decimal num = after - Owner.Creature.GetPowerAmount<QingmingSwordForce>();
            if (num != 0)
                await PowerCmd.Apply<QingmingSwordForce>(new ThrowingPlayerChoiceContext(), Owner.Creature, num, Owner.Creature, cardModel);
        }

    }
}
