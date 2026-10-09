using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using System.Collections.Generic;

namespace YeShunguang
{
    [Pool(typeof(YeShunguangCardPool))]
    public abstract class YeCardModel
        (int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true, bool autoAdd = true) : 
        CustomCardModel(baseCost, type, rarity, target, showInCardLibrary, autoAdd)
    {
        protected virtual string PortraitName => Id.Entry.RemovePrefix().ToSnakeCase();

        public override string? CustomPortraitPath => 
            PathHelper.GetCardImageBigPath(PortraitName);

        public override string PortraitPath =>
            PathHelper.GetCardImageBigPath(PortraitName);
    }
}
