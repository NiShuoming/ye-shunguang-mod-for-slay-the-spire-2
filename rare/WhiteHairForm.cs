using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeShunguang
{
    /// <summary>
    /// 白发形态 3 【虚无】获得99层【明心境】，能量归零时明心境不再退出 完成
    /// </summary>
    public class WhiteHairForm:YeCardModel
    {
        public WhiteHairForm():base(3,CardType.Power,CardRarity.Rare,TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            [CardKeyword.Ethereal];

        protected override IEnumerable<DynamicVar> CanonicalVars => 
            [new PowerVar<EnlightenedMind>(99)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => 
            [ HoverTipFactory.FromPower<EnlightenedMind>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<WhiteHairFormPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);

            await PowerCmd.Apply<EnlightenedMind>(choiceContext, Owner.Creature,
                DynamicVars.Power<EnlightenedMind>().BaseValue, Owner.Creature, this);

        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }
    }
}
