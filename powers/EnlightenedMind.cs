using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using ZZZLib;
using ZZZLib.Cmds;
using ZZZLib.Other;


namespace YeShunguang
{
    /// <summary>
    /// 明心境 完成
    /// </summary>
    public class EnlightenedMind : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
        /// <summary>
        /// 是否有 暖霞拾光 power
        /// </summary>
        private bool _haveP = false;

        private List<CardModel> _ExhaustCards = new List<CardModel>();

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageDecrease", 25m)];

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource,CardPlay cardPlay)
        {
            if (target != base.Owner)
            {
                return 1m;
            }

            if (!props.IsPoweredAttack())
            {
                return 1m;
            }

            return 1 - base.DynamicVars["DamageDecrease"].BaseValue / 100m;
        }

        //层数为0时获得层数
        public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (target != applier || !target.IsPlayer || target.Player == null)
                return;
            //判断是否有暖霞拾光
            _haveP = target.GetPower<TouchOfDawnlightPower>() != null;
            Flash();
            /*
            //增加1层御剑
            await PowerCmd.Apply<SwordRiding>(new ThrowingPlayerChoiceContext(),target, 1, target, null);*/
            //将记忆牌添加1次重放
            IEnumerable<CardModel> enumerable = target.Player?.PlayerCombatState?.AllCards ?? [];
            _ExhaustCards = new List<CardModel>();
            foreach (CardModel item in enumerable)
            {
                await TryAddReplays(item);
            }
        }

        /// <summary>
        /// 当战斗中有卡牌添加
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public override async Task AfterCardEnteredCombat(CardModel card)
        {
            if (card.IsClone || card.Owner.Creature != Owner) return;
            await TryAddReplays(card);
        }
        //获得重放
        private Task TryAddReplays(CardModel card)
        {
            if (card.Keywords.Contains(YeKeywords.Memory))
            {
                card.BaseReplayCount += 1;
                if (!card.Keywords.Contains(CardKeyword.Exhaust) && !_haveP)
                {
                    _ExhaustCards.Add(card);
                    card.AddKeyword(CardKeyword.Exhaust);
                }
            }
            else if (_haveP && card.Keywords.Contains(ZZZKeyWord.Partner))
            {
                card.BaseReplayCount += 1;
            }

            return Task.CompletedTask;
        }
        //消耗记忆卡
        /*
        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if(cardPlay.Card.Keywords.Contains(YeKeywords.Memory) && cardPlay.Card.Owner.Creature == Owner
                && !_haveP && !cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust))
            {
               await CardCmd.Exhaust(choiceContext, cardPlay.Card);
            }
        }*/
        //减少层数
        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Enemy)
            {
                await PowerCmd.TickDownDuration(this);
            }
        }

        //
        private async Task PowerOver()
        {
            if (Owner.GetPower<WhiteHairFormPower>() == null)
            {
                await PowerCmd.ModifyAmount(new
                   ThrowingPlayerChoiceContext(), this, -Amount, Owner, null);
            }
        }

        public override async Task AfterEnergySpent(CardModel card, int amount)
        {
            if (card.Owner.Creature == Owner && card.Owner.PlayerCombatState.Energy <= 0)
            {
                await PowerOver();
            }
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power.Owner != Owner)
                return;
            //添加和去掉暖霞拾光，处理
            if (power is TouchOfDawnlightPower)
            {
                if (_haveP && power.Amount <= 0)
                {
                    _haveP = false;
                    IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? [];
                    foreach (CardModel item in enumerable)
                    {
                        if (item.Keywords.Contains(ZZZKeyWord.Partner))
                        {
                            item.BaseReplayCount -= 1;
                            if (_ExhaustCards.Contains(item))
                            {
                                item.RemoveKeyword(CardKeyword.Exhaust);
                                _ExhaustCards.Remove(item);
                            }
                            else if (item.Keywords.Contains(YeKeywords.Memory))
                            {
                                if (!item.Keywords.Contains(CardKeyword.Exhaust))
                                {
                                    item.AddKeyword(CardKeyword.Exhaust);
                                    _ExhaustCards.Add(item);
                                }
                            }
                        }
                    }

                }
                else if (!_haveP && power.Amount > 0)
                {
                    _haveP = true;
                    IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? [];
                    foreach (CardModel item in enumerable)
                    {
                        if (item.Keywords.Contains(ZZZKeyWord.Partner))
                        {
                            item.BaseReplayCount += 1;

                        }
                        else if (item.Keywords.Contains(YeKeywords.Memory))
                        {
                            if (_ExhaustCards.Contains(item))
                            {
                                item.RemoveKeyword(CardKeyword.Exhaust);
                                _ExhaustCards.Remove(item);
                            }
                        }
                    }
                }
            }
            else if (power is EnlightenedMind && amount > 0)
            {
                await TurnForceToEnergy(choiceContext);
                if (Owner.Player.PlayerCombatState.Energy <= 0 && Amount > 0)
                {
                    await PowerOver();
                }
            }
        }
        /// <summary>
        /// 清空青溟剑势并转化为能量
        /// </summary>
        private async Task TurnForceToEnergy(PlayerChoiceContext choiceContext)
        {
            if (!Owner.IsPlayer) return;
            //清空青溟剑势并转化为能量
            ZZZSecondEnergy zz = ZZZCmd.GetSecondEnergy(Owner.Player);
            int n = zz.Energy2;
            if (n > 0)
            {
                await ZZZCmd.AddSecondEnergy(Owner.Player, -n, Owner, null);
                await PlayerCmd.SetEnergy(n, Owner.Player);
            }
            if (zz.Couter != null && zz.Couter is EnlightenedMindCounter counter)
                counter.ChangeModFvx(true);
        }

        public override Task AfterRemoved(Creature oldOwner)
        {
            //记忆牌去掉重放
            IEnumerable<CardModel> enumerable = oldOwner.Player?.PlayerCombatState?.AllCards ?? [];
            foreach (CardModel item in enumerable)
            {
                if (item.Keywords.Contains(YeKeywords.Memory))
                {
                    item.BaseReplayCount -= 1;
                    if (_ExhaustCards.Contains(item))
                    {
                        item.RemoveKeyword(CardKeyword.Exhaust);
                        _ExhaustCards.Remove(item);
                    }
                }
                else if (_haveP && item.Keywords.Contains(ZZZKeyWord.Partner))
                {
                    item.BaseReplayCount -= 1;
                }
            }
            //触发特效
            if (oldOwner.IsPlayer && oldOwner.Player != null)
            {
                ZZZSecondEnergy zz = ZZZCmd.GetSecondEnergy(Owner.Player);
                if (zz.Couter != null && zz.Couter is EnlightenedMindCounter counter)
                    counter.ChangeModFvx(false);

            }
            return Task.CompletedTask;
            /*await PowerCmd.Remove<SwordRiding>(oldOwner);*/
        }
    }
}
