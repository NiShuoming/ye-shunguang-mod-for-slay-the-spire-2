using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using System;
using System.Reflection;
using System.Threading.Tasks;
using ZZZLib.Other;

namespace YeShunguang
{
    [GlobalClass]
    public partial class EnlightenedMindCounter : ZZZSecondEnergyCounter
    {
        [Export]
        private NParticlesContainer? _backVfx;
        [Export]
        private NParticlesContainer? _frontVfx;
        [Export]
        private Control? _frontE;

        private Color OutlineColor => _pl.Character.EnergyLabelOutlineColor;

        public override void _Ready()
        {
            base._Ready();
            ChangeModFvx(false);
            Visible = true;
        }

        public void ChangeModFvx(bool changeToMind)
        {
            if (_frontE != null)
                _frontE.Visible = !changeToMind;
            _pl.Creature.GetCreatureNode().SpineAnimation.SetAnimation("FlyingSword", true, 1);
            //白发动画
            if (changeToMind)
                _pl.Creature.GetCreatureNode().SpineAnimation.SetAnimation("WhiteMode", false, 2);
            else
                _pl.Creature.GetCreatureNode().SpineAnimation.SetAnimation("NormalMode", false, 2);
        }
        //特效
        private void Parti()
        {
            _backVfx?.Restart();
            _frontVfx?.Restart();
        }

        protected override void Reflash()
        {
            if (_label == null || _pl == null) return;
            ZZZSecondEnergy energy2 = _pl.PlayerCombatState.GetSecondEnergy(_pl);
            _label.SetTextAutoSize($"{energy2.Energy2}/{energy2.MaxEnergy2}");
            _label.AddThemeColorOverride(ThemeConstants.Label.FontColor, (energy2.Energy2 == 0) ? StsColors.red : StsColors.cream);
            _label.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, (energy2.Energy2 == 0) ? StsColors.unplayableEnergyCostOutline : OutlineColor);
        }

        protected override void OnEnergyChanged(int oldEnergy, int newEnergy)
        {
            if (oldEnergy != newEnergy)
            {
                Parti();
            }
        }
    }
}
