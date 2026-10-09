using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 飞剑 消耗 伤害3=5
    /// </summary>
    public class FlyingSword : YeCardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [YeKeywords.Memory, CardKeyword.Exhaust];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m,ValueProp.Move)];

        public FlyingSword():base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy) { }

        protected override HashSet<CardTag> CanonicalTags => [CardTag.Shiv];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [HoverTipFactory.FromKeyword(YeKeywords.Memory)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(2m);
        }

        public static async Task<CardModel?> CreateInHand(Player owner, CombatState combatState)
        {
            return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
        }

        public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, CombatState combatState)
        {
            if (count == 0)
            {
                return [];
            }
            if (CombatManager.Instance.IsOverOrEnding)
            {
                return [];
            }
            List<CardModel> shivs = [];
            for (int i = 0; i < count; i++)
            {
                shivs.Add(combatState.CreateCard<FlyingSword>(owner));
            }
            await CardPileCmd.AddGeneratedCardsToCombat(shivs, PileType.Hand, owner);
            return shivs;
        }

        
    }
}
