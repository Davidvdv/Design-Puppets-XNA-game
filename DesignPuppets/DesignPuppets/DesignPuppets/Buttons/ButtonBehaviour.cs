using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DesignPuppets
{
	class ButtonBehaviour : GameObject
	{
		public enum BehaviourType {
			Dig,
			Block,
			UseUmbrella,
			None
		}

		public BehaviourType ButtonBehaviourType { get; set; }

		public ButtonBehaviour(Game game, string textureName, Vector2 vector, BehaviourType buttonBehaviour)
			: base(game, textureName, 1, vector)
		{
			ButtonBehaviourType = buttonBehaviour;
		}
	}
}
