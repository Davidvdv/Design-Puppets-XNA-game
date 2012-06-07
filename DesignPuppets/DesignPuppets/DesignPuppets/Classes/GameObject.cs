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
	/// Een abstracte class GameObject.
	/// 
	/// Hierin staan fields en methods die universeel zijn voor een GameComponent in de game.
	/// </summary>
	public abstract class GameObject : Microsoft.Xna.Framework.DrawableGameComponent
	{
		private Texture2D _texture;

		public Texture2D Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}
		protected Vector2 _vector2;
		protected Rectangle _col;

		public Rectangle CollisionRectangle
		{
			get { return _col; }
			set { _col = value; }
		}
		private Rectangle _frame;
		private int _totalFrames;

		public int TotalFrames
		{
			get { return _totalFrames; }
			set { _totalFrames = value; }
		}
		private string _textureName;

		private TimeSpan elapsedTime = TimeSpan.Zero;

		/// <summary>
		/// Een abstracte class GameObject.
		/// Hierin staan fields en methods die universeel zijn voor een GameComponent in de game.
		/// 
		/// Parameters voor een GameObject opslaan in fields.
		/// </summary>
		/// <param name="game">Game meesturen voor DrawableGameComponent.</param>
		/// <param name="textureName">De texture van het GameObject.</param>
		/// <param name="totalFrames">Het aantal frames (animaties) waaruit een GameObject bestaat.</param>
		/// <param name="XYposition">De positie op de Viewport.</param>
		public GameObject(Game game, string textureName, int totalFrames, Vector2 XYposition)
			: base(game)
		{
			// TODO: Construct any child components here
			//_texture = Game.Content.Load<Texture2D>(texture);
			_vector2 = XYposition;
			_totalFrames = totalFrames;
			_textureName = textureName;

			game.Components.Add(this);
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			// Initialiseren van de texture.
			try
			{
				TextureLoader t = TextureLoader.GetInstance(Game);
				_texture = t.GameObjectTextures[_textureName];
				_frame = new Rectangle(0,0, _texture.Width / _totalFrames, _texture.Height);
				_col = new Rectangle((int)_vector2.X, (int)_vector2.Y, _texture.Width / _totalFrames, _texture.Height);
			}
			catch (Exception)
			{
				throw new Exception("Kan de texture niet laden!");
			}
			
			base.Initialize();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// Verschuif om de 100 milliseconden de rectangle op het plaatje zodat er een animatie ontstaat.
			elapsedTime += gameTime.ElapsedGameTime;
			if (elapsedTime > TimeSpan.FromMilliseconds(100))
			{
				elapsedTime -= TimeSpan.FromMilliseconds(100);

				if (_frame.X < (_texture.Width - _frame.Width)) _frame.X += _frame.Width;
				else _frame.X = 0;
			}

			// Verplaats ook collision rectangle met het GameObject mee.
			_col.X = (int)_vector2.X;
			_col.Y = (int)_vector2.Y;

			base.Update(gameTime);
		}

		/// <summary>
		/// Teken het GameObject.
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			Game1.spriteBatch.Begin();
			Game1.spriteBatch.Draw(_texture, _vector2, _frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Game1.spriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Stuur coordinaat terug van de vector.
		/// </summary>
		/// <returns>De x-coordinaat.</returns>
		public float getPosX()
		{
			return _vector2.X;
		}

		/// <summary>
		/// Verander de X positie
		/// </summary>
		/// <param name="value">Een nieuwe waarde voor de x-positie</param>
		public void setPosX(float value)
		{
			_vector2.X = value;
		}

		/// <summary>
		/// Stuur coordinaat terug van de vector.
		/// </summary>
		/// <returns>De y-coordinaat.</returns>
		public float getPosY()
		{
			return _vector2.Y;
		}

		/// <summary>
		/// Verander de Y positie
		/// </summary>
		/// <param name="value">Een nieuwe waarde voor de y-positie</param>
		public void setPosY(float value)
		{
			_vector2.Y = value;
		}
	}
}
