using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace YeShunguang
{
    /// <summary>
    /// 白发模式
    /// </summary>
    public class WhiteHairFormPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;
    }
}
