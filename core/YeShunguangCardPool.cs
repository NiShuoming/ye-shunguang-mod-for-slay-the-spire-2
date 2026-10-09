using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using ZZZLib.Cards.Equips.DriveDiscs;
using ZZZLib.Cards.Equips.Other;
using ZZZLib.Cards.Equips.WEngine;
using ZZZLib.Cards.Partner;


namespace YeShunguang
{
    /// <summary>
    /// 叶瞬光卡池（未完成）
    /// </summary>
    public class YeShunguangCardPool : CustomCardPoolModel
    {
        public override string Title => "YeShunguang";

        public override string EnergyColorName => "ye_shunguang";

        public override string CardFrameMaterialPath => "card_frame_pink";

        public override Color DeckEntryCardColor => Colors.Pink;

        public override Color EnergyOutlineColor => Colors.Red;

        public override bool IsColorless => false;
        
        /// <summary>
        /// 记录角色所有卡牌
        /// </summary>
        /// <returns></returns>
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<JuFufu>(),
				ModelDb.Card<PanYinhu>(),
				ModelDb.Card<Zhao>(),
				ModelDb.Card<CloudcleaveRadiance>(),
				ModelDb.Card<YeShiyuan>(),
				ModelDb.Card<Yixuan>(),
				ModelDb.Card<YunkuiSummit>(),
				 ModelDb.Card<JadeSoulFrozenHeart>(),
				 ModelDb.Card<WhiteWaterBallad>(),
				 ModelDb.Card<Wise>(),
				 ModelDb.Card<Dialyn>(),
				 ModelDb.Card<Banyue>(),
				 ModelDb.Card<Belle>(),
				 ModelDb.Card<ChainAttack>()
			};
		}
    }

}
