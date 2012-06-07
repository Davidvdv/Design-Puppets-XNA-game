using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets
{
	/// <summary>
	/// Behaviour om de puppet te laten graven.
	/// </summary>
	class DigBehaviour : IPuppetBehaviour
	{
		private float _digSpeed = 0.5f;

		public void DoTask(Puppet puppet)
		{
			// Verander het aantal frames die bij de animatie hoort van graven.
			puppet.TotalFrames = 16;

			// Haal uit de singleton de juiste texture en laadt die in.
			TextureLoader t = TextureLoader.GetInstance(puppet.Game);
			puppet.Texture = t.GameObjectTextures["lemming_dig_r"];
			
			// Verplaats de puppet.
			puppet.setPosY(puppet.getPosY() + _digSpeed);

			// Verander de state van de puppet, zodat de pupppet weet wat hij aan het doen is.
			puppet.State = Puppet.PuppetState.Digging;
		}
	}
}
