using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

public partial class PlayAni : Control
{
	public override void _Ready()
	{
		foreach (Node2D childSpineNode in GetChildSpineNodes())
		{
			MegaSprite megaSprite = new MegaSprite(childSpineNode);
			if (megaSprite.HasAnimation("animation"))
			{
				megaSprite.GetAnimationState().SetAnimation("animation", loop: true, 1);
			}
		}
	}
	private IEnumerable<Node2D> GetChildSpineNodes()
	{
		foreach (Node2D item in GetChildren().OfType<Node2D>())
		{
			if (!(item.GetClass() != "SpineSprite"))
			{
				yield return item;
			}
		}
	}
}
