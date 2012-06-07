using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DesignPuppets
{
	class Finish : GameObject
	{
		/// <summary>
		/// Het eindpunt waar de Puppets kunnen finishen.
		/// </summary>
		/// <param name="game">De game voor GameObject</param>
		/// <param name="vector">De positie van de finish</param>
		public Finish(Game game, Vector2 vector) : base(game, "finish", 1, vector)
		{

		}
	}
}
