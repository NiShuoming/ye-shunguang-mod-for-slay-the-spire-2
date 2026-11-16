using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZLib;

namespace YeShunguang
{
    /// <summary>
    /// 暖霞拾光 【明心境】状态下，【记忆】不再消耗，【伙伴】也可以再释放一次 完成
    /// </summary>
    public class TouchOfDawnlightPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [ HoverTipFactory.FromPower<EnlightenedMind>(),  HoverTipFactory.FromKeyword(YeKeywords.Memory),
        HoverTipFactory.FromKeyword(ZZZKeyWord.Partner)];


    }
}
