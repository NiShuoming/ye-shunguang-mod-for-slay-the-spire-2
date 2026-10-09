using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZZZLib;
using ZZZLib.Model;

namespace YeShunguang
{
    /// <summary>
    /// 芽芽 【伙伴】【消耗】造成3点伤害，获得3张【飞剑】->【飞剑+】 完成
    /// </summary>
    [Pool(typeof(YeShunguangCardPool))]
    public class Sprout: ZZZPartnerCardModel
    {
        public Sprout():base(1,CardType.Attack,CardRarity.Common,TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DamageVar(3m,ValueProp.Move),new CardsVar(3)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust, ZZZKeyWord.Partner];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(ZZZKeyWord.Partner),HoverTipFactory.FromCard<FlyingSword>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            IEnumerable<CardModel> enumerable2 = await FlyingSword.CreateInHand(Owner, DynamicVars.Cards.IntValue, (MegaCrit.Sts2.Core.Combat.CombatState)CombatState);
            if (!base.IsUpgraded)
            {
                return;
            }
            foreach (CardModel item in enumerable2)
            {
                CardCmd.Upgrade(item);
            }
        }
    }
}
