using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets.Behaviours
{
	/// <summary>
	/// Behaviour om de Puppet anderen te blokkeren.
	/// Helaas nog niet toegepast in de game.
	/// </summary>
	class BlockBehaviour : IPuppetBehaviour
	{
		public void DoTask(Puppet puppet)
		{
			puppet.TotalFrames = 16;
			TextureLoader t = TextureLoader.GetInstance(puppet.Game);
			puppet.Texture = t.GameObjectTextures["lemming_block_r"];
		}
	}
}
