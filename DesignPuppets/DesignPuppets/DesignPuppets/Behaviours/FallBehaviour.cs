using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets
{
	/// <summary>
	/// Behaviour om de Puppet te laten vallen.
	/// </summary>
	class FallBehaviour : IPuppetBehaviour
	{
		private float _fallSpeed = 1.5f;
		private float _fallDistance = 0;

		public void DoTask(Puppet puppet)
		{
			// Verander het aantal frames die bij de animatie hoort van vallen.
			puppet.TotalFrames = 4;

			// Haal uit de singleton de juiste texture en laadt die in.
			TextureLoader t = TextureLoader.GetInstance(puppet.Game);
			puppet.Texture = t.GameObjectTextures["lemming_fall_r"];

			// Verplaats de puppet.
			puppet.setPosY(puppet.getPosY() + _fallSpeed);

			// Verander de state van de puppet, zodat de pupppet weet wat hij aan het doen is.
			puppet.State = Puppet.PuppetState.Falling;

			// Hou de valhoogte bij.
			_fallDistance += _fallSpeed;

			// Wanneer de valhoogte groter is dan 3 keer de hoogte van de puppet dan is de puppet dood.
			// Note: Dit kon ik helaas nog niet helemaal uitwerken. Vandaar nog even uitgeschakeld.
			//if (_fallDistance > puppet.Texture.Height * 3) puppet.State = Puppet.PuppetState.Dead;
		}
	}
}
