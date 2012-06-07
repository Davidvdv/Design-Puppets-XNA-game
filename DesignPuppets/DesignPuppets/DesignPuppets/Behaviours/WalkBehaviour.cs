using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets
{
	/// <summary>
	/// Behaviour om de Puppet te laten lopen.
	/// </summary>
	class WalkBehaviour : IPuppetBehaviour
	{
		// De loopsnelheid van de Puppet.
		private float _walkSpeed = 0.5f;

		public void DoTask(Puppet puppet)
		{
			// Aantal frames van de animatie lopen.
			puppet.TotalFrames = 8;

			// Haal uit de singleton de juiste texture en laadt die in.
			TextureLoader t =  TextureLoader.GetInstance(puppet.Game);
			puppet.Texture = t.GameObjectTextures["lemming_walk_r"];

			// Verplaats de Puppet met loopsnelheid.
			puppet.setPosX(puppet.getPosX() + _walkSpeed);

			// Verander de state van de puppet, zodat de pupppet weet wat hij aan het doen is.
			puppet.State = Puppet.PuppetState.Walking;
		}
	}
}
