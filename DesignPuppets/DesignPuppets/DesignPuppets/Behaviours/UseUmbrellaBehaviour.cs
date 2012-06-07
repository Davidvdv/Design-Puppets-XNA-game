using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets
{
	/// <summary>
	/// Behaviour om de Puppet zijn paraplu te laten gebruiken.
	/// Deze wordt gebruikt wanneer Puppet.HasUmbrella == true.
	/// </summary>
	class UseUmbrellaBehaviour : IPuppetBehaviour
	{
		// Snelheid waarme de Puppet daalt.
		private float _glideSpeed = 0.6f;

		public void DoTask(Puppet puppet)
		{
			// Verander het aantal frames die bij de animatie hoort van graven.
			puppet.TotalFrames = 8;

			// Haal uit de singleton de juiste texture en laadt die in.
			TextureLoader t = TextureLoader.GetInstance(puppet.Game);
			puppet.Texture = t.GameObjectTextures["lemming_float_r"];

			// Verplaats de Puppet met de snelheid.
			puppet.setPosY(puppet.getPosY() + _glideSpeed);

			// Verander de state van de puppet, zodat de pupppet weet wat hij aan het doen is.
			puppet.State = Puppet.PuppetState.UseUmbrella;
		}
	}
}
