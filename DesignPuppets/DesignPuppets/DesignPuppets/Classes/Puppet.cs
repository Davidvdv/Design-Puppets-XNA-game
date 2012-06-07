using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace DesignPuppets
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class Puppet : GameObject
	{
		
		/// <summary>
		/// Property voor het hebben van een paraplu. Wanneer de speler een puppet een paraplu geeft
		/// </summary>
		public bool HasUmbrella { get; set; }

		/// <summary>
		/// Property waarmee de behaviour van de puppet kan getten en setten.
		/// </summary>
		public IPuppetBehaviour Behaviour { get; set; }

		/// <summary>
		/// De verschillende states waarin een puppet zich kan bevinden.
		/// </summary>
		public enum PuppetState
		{
			Walking,
			Digging,
			Falling,
			UseUmbrella,
			Dead
		}

		/// <summary>
		/// Property om de state van de puppet in op te slaan.
		/// Puppet moet weten wat die aan het doen is.
		/// </summary>
		public PuppetState State { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game">Game meesturen aan de parent GameObject</param>
		/// <param name="textureName">De texture die puppet kan krijgen</param>
		/// <param name="totalFrames">Het aantal frames (animaties) die de puppet heeft.</param>
		/// <param name="vector">De positie van de puppet.</param>
		public Puppet(Game game, string textureName, int totalFrames, Vector2 vector)
			: base(game, textureName, totalFrames, vector)
		{
			Behaviour = new WalkBehaviour();
			HasUmbrella = false;
		}

		public override void Update(GameTime gameTime)
		{
			// Voer de behaviour uit via de method DoTask die in de interface staat.
			Behaviour.DoTask(this);

			base.Update(gameTime);
		}
	}
}
