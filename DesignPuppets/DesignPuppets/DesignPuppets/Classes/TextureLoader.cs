using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DesignPuppets
{
	/// <summary>
	/// Een Singleton waarmee ik de textures alvast preload zodat ik die in de game kan laten wisselen.
	/// </summary>
	class TextureLoader
	{
		private static TextureLoader theInstance;
		private Dictionary<string, Texture2D> gameObjectTextures;

		public Dictionary<string, Texture2D> GameObjectTextures
		{
			get { return gameObjectTextures; }
		}

		/// <summary>
		/// Hier worden de sprites ingeladen waar tuse
		/// </summary>
		/// <param name="game">De game wordt meegestuurd om de sprite te laden.</param>
		private TextureLoader(Game game) 
		{
			gameObjectTextures = new Dictionary<string, Texture2D>();
			gameObjectTextures.Add("theTile",			game.Content.Load<Texture2D>("theTile"));
			gameObjectTextures.Add("finish",			game.Content.Load<Texture2D>("finish"));
			gameObjectTextures.Add("BtnDig",			game.Content.Load<Texture2D>("BtnDig"));
			gameObjectTextures.Add("BtnUmbrella", game.Content.Load<Texture2D>("BtnUmbrella"));
			gameObjectTextures.Add("lemming_walk_r",	game.Content.Load<Texture2D>("lemming_walk_r"));
			gameObjectTextures.Add("lemming_walk_l",	game.Content.Load<Texture2D>("lemming_walk_l"));
			gameObjectTextures.Add("lemming_dig_r",		game.Content.Load<Texture2D>("lemming_dig_r"));
			gameObjectTextures.Add("lemming_dig_l",		game.Content.Load<Texture2D>("lemming_dig_l"));
			gameObjectTextures.Add("lemming_fall_r",	game.Content.Load<Texture2D>("lemming_fall_r"));
			gameObjectTextures.Add("lemming_fall_l",	game.Content.Load<Texture2D>("lemming_fall_l"));
			gameObjectTextures.Add("lemming_float_r",	game.Content.Load<Texture2D>("lemming_float_r"));
			gameObjectTextures.Add("lemming_float_l",	game.Content.Load<Texture2D>("lemming_float_l"));
			gameObjectTextures.Add("lemming_explode_s", game.Content.Load<Texture2D>("lemming_explode_s"));
		}

		/// <summary>
		/// Een getter om de instantie te op vragen. De static TextureLoader theInstance.
		/// </summary>
		/// <param name="game"></param>
		/// <returns>Een instantie van zichzelf (de Singleton).</returns>
		public static TextureLoader GetInstance(Game game)
		{
			if (theInstance == null)
			{
				theInstance = new TextureLoader(game);
			} 

			return theInstance;
		}
	}
}
