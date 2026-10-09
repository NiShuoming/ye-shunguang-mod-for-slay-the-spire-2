using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 执剑 1 消耗 =>保留 生成1飞剑，去除手牌中【飞剑】的消耗，并使其伤害+4=7 完成
    /// </summary>
    public class SwordMastery:YeCardModel
    {
        public SwordMastery() : base(1,CardType.Skill,CardRarity.Uncommon,TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new DynamicVar("PowerUp",4m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromCard<FlyingSword>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

            await FlyingSword.CreateInHand(Owner, 1,
                (MegaCrit.Sts2.Core.Combat.CombatState)CombatState);

            var enumerable = PileType.Hand.GetPile(base.Owner).Cards.Where(a => (a.Tags.Contains(CardTag.Shiv) ));
            foreach(var card in enumerable)
            {
                if(card.Keywords.Contains(CardKeyword.Exhaust))
                    card.RemoveKeyword(CardKeyword.Exhaust);
                card.DynamicVars.Damage.BaseValue += DynamicVars["PowerUp"].BaseValue;
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["PowerUp"].UpgradeValueBy(3m);
            AddKeyword(CardKeyword.Retain);
        }
    }
}
