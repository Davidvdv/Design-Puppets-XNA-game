using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace DesignPuppets
{
	/// <summary>
	/// In het Level wordt een bepaalde spelsituatie gemaakt.
	/// De speler speelt een level die door Game aangemaakt is.
	/// </summary>
	public class Level : DrawableGameComponent
	{
		private bool _isComplete; // Controleert of level klaar is.

		public bool IsComplete
		{
			get { return _isComplete; }
			set { _isComplete = value; }
		}

		// Lijsten met knoppen en puppets
		private List<Puppet> _listOfPuppets = new List<Puppet>();
		private List<ButtonBehaviour> _listOfButtons = new List<ButtonBehaviour>();
		
		/*
		 * Specifieke fields waarin wordt bijgehouden:
		 * 
		 * Hoeveel Puppets er mogen spawnen in het Level.
		 * Hoeveel Puppets er minimaal moeten finishen.
		 * Hoeveel Puppets er gefinisht zijn.
		 * Hoeveel Puppets er al door het Level zijn aangemaakt.
		 * 
		 */
		private int _maxRespawnPuppets;
		private int _minRequiredFinishedPuppets;
		private int _fisnishedPuppets;
		private int _totalCreatedPuppets;
		
		private int _levelNr = 1;
		
		private Texture2D _levelBg;
		private Finish _finish; // Het eindpunt voor de Puppets.

		// Het aantal blokjes op de x-as en y-as voor het grid.
		private int _blocksX;
		private int _blocksY;

		// Hoeveel tijd er al voorbij is sinds het Level is gestart.
		private TimeSpan elapsedTime = TimeSpan.Zero;

		private MouseState _prevMouseState;

		// Level weet welke knop er is geklikt en krijgt een enum van de Button
		private ButtonBehaviour.BehaviourType _newPuppetBehaviour;

		/*
		 * Grid: Een array met daarin ints.
		 * Elk vakje in de array staat voor een blokje. Op basis hiervan wordt het level getekent.
		 * 
		 * Betekenis van de vakjes:
		 * 0 = lucht
		 * 1 = grond
		 * 2 = afgrond rechts
		 * 4 = onder de grond
		 * 
		 * 10 = het eindpunt (finish)
		 * 
		 */
		int[,] theGrid = {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},       
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
						  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0},
						  {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
						  {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4}};

		/// <summary>
		/// In de constructor van Level worden variablen gedefinieerd die van invloed zijn op de gameplay.
		/// </summary>
		/// <param name="game">Game meesturen voor de parent DrawableGameComponent.</param>
		/// <param name="minRequiredFinishedPuppets"></param>
		/// <param name="maxPuppets"></param>
		public Level(Game game, int minRequiredFinishedPuppets, int maxPuppets):base(game)
		{
			// Level voegt zichzelf toe aan de Componets list.
			game.Components.Add(this);
			
			_isComplete = false;
			_maxRespawnPuppets = maxPuppets;
			_fisnishedPuppets = 0;
			_totalCreatedPuppets = 0;

			// Uitrekenen uit hoeveel blokjes het level bestaat.
			_blocksX = Game.GraphicsDevice.Viewport.Width / 32;
			_blocksY = Game.GraphicsDevice.Viewport.Height / 32;

			// Controle of parameters wel gelden. Het minimaal aantal mag niet groter zijn dan de maximaal aantal Puppets
			if (minRequiredFinishedPuppets > maxPuppets) _minRequiredFinishedPuppets = maxPuppets;
			else _minRequiredFinishedPuppets = minRequiredFinishedPuppets;

			// Maak een finish
			this.CreateFinish();
		}

		/// <summary>
		/// Controleert waar in het grid staat waar de finish moet komen en Finish zet zichzelf die op de positie.
		/// </summary>
		private void CreateFinish()
		{
			for (int y = 0; y < _blocksY; y++)
			{
				for (int x = 0; x < _blocksX; x++)
				{
					if (theGrid[y, x] == 10)
					{
						_finish = new Finish(Game, new Vector2(x * 32, y * 32));
					}
				}
			}
		}

		/// <summary>
		/// Het vullen van de lijsten met knoppen en puppets.
		/// </summary>
		public override void Initialize()
		{
			_listOfButtons.Add(new ButtonBehaviour(Game, "BtnDig", new Vector2(250, 10), ButtonBehaviour.BehaviourType.Dig));
			_listOfButtons.Add(new ButtonBehaviour(Game, "BtnUmbrella", new Vector2(200, 10), ButtonBehaviour.BehaviourType.UseUmbrella));
			_prevMouseState = Mouse.GetState();

			base.Initialize();
		}

		/// <summary>
		/// Inladen van de tile (een groot plaatje) met de blokjes erop.
		/// </summary>
		protected override void LoadContent()
		{
			// De singleton haalt een plaatje op die daarin geladen staat.
			TextureLoader t = TextureLoader.GetInstance(Game);
			_levelBg = t.GameObjectTextures["theTile"];

			base.LoadContent();
		}

		/// <summary>
		/// Level kan zelf een puppet aanmaken.
		/// </summary>
		private void CreatePuppet()
		{			
			_listOfPuppets.Add(new Puppet(Game, "lemming_walk_r", 8, new Vector2(100, 0)));
			_totalCreatedPuppets++;
		}
	
		public int LevelNr { get; set; }

		public void NextLevel()
		{
			_levelNr++;
		}

		public override void Update(GameTime gameTime)
		{
			/** Zet elke seconde een nieuwe Puppet op het scherm zolang het maximum van het aantal Puppets in het level is bereikt. **/
			elapsedTime += gameTime.ElapsedGameTime;
			if (elapsedTime > TimeSpan.FromMilliseconds(1000) && _totalCreatedPuppets < _maxRespawnPuppets )
			{
				elapsedTime -= TimeSpan.FromMilliseconds(1000);
				
				this.CreatePuppet();
			}

			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRec = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

			// Voor elk knop kijken of er opgeklikt is.
			foreach (ButtonBehaviour button in _listOfButtons)
			{
				if (mouseRec.Intersects(button.CollisionRectangle))
				{
					// Zo ja, sla het behaviour type op, zodat level weet welke actie die moet toekennen aan puppet wannneer er op geklikt wordt.
					if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
					{
						_newPuppetBehaviour = button.ButtonBehaviourType;
					}

					_prevMouseState = mouseState;
				}
			}
			
			// Voor elke Puppet in de lijst doet die het volgende. ToArray() om tussendoor puppets om de lijst te muteren.
			foreach (Puppet puppet in _listOfPuppets.ToArray<Puppet>())
			{
				/* De coordinaat uitrekenen op basis van de x- en y-as van de puppet.
				 * Door te delen door 32 weet ik in welke hokje de puppet zich bevindt in theGrid.
				 */
				int puppetGridCorX = (int) Math.Floor(puppet.getPosX() / 32);
				int puppetGridCorY = (int) Math.Floor((puppet.getPosY() + puppet.Texture.Height) / 32);

				// Als de puppet de finish raakt...
				if (puppet.CollisionRectangle.Intersects(_finish.CollisionRectangle))
				{
					PuppetIsFinished(puppet); // De puppet is gefinisht.
				}

				// Controleren of er een knop is ingedrukt.
				if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
				{
					// Het selecteren van de Puppet.
 					if (puppet.CollisionRectangle.Contains(mouseState.X, mouseState.Y))
					{
						/* Het level houdt bij op welke knop er is geklikt in _newPuppetBehaviour. 
						 * Als die gelijk is aan een bepaald behaviour type en de puppet is in een bepaalde state,
						 * alleen dan mag die de actie uitvoeren.
						 */
						if (_newPuppetBehaviour == ButtonBehaviour.BehaviourType.Dig && puppet.State == Puppet.PuppetState.Walking)
						{
							// Puppet mag nu graven.
							puppet.Behaviour = new DigBehaviour();
						}
						else if (_newPuppetBehaviour == ButtonBehaviour.BehaviourType.UseUmbrella)
						{
							// Puppet krijgt een paraplu.
							puppet.HasUmbrella = true;
						}
					}
				}

				/* 
				 * De Puppet doet iets op basis van de state.
				 * 
				 * Controleren welke state de puppet zich bevindt en aan de hand daarvan controleren.
				 * Aan de hand daarvan controlren of die al moet stoppen met de huidige behaviour. De behaviour wordt dan aangepast.
				 */
				switch(puppet.State)
				{
					case Puppet.PuppetState.Falling:

						// Als de Puppet zicht op de grond bevindt, dan moet hij weer gaan lopen.
						if(theGrid[puppetGridCorY, puppetGridCorX] == 1) puppet.Behaviour = new WalkBehaviour();
						break;
					case Puppet.PuppetState.Walking:

						//Controleren of de Puppet niet doodgevallen is.
						if (puppet.State == Puppet.PuppetState.Dead) this.RemovePuppet(puppet);

						// Als de Puppet in de lucht bevindt en de puppet heeft een paraplu gebruik die dan, anders vallen.
						if (theGrid[puppetGridCorY, puppetGridCorX] == 0)
						{
							if (puppet.HasUmbrella) puppet.Behaviour = new UseUmbrellaBehaviour();
							else puppet.Behaviour = new FallBehaviour();
						}
						break;
					case Puppet.PuppetState.UseUmbrella:

						// Als de Puppet zicht op de grond bevindt, dan moet hij weer gaan lopen.
						if (theGrid[puppetGridCorY, puppetGridCorX] == 1) puppet.Behaviour = new WalkBehaviour();
						break;
					case Puppet.PuppetState.Digging:

						// Het vakje in de theGrid aanpassen zodat de grond verdwijnt.
						theGrid[puppetGridCorY - 1, puppetGridCorX] = 0; 

						// Als de Puppet door de grond heen is moet hij gaan vallen
						if (theGrid[puppetGridCorY, puppetGridCorX] == 0)
						{
							if (puppet.HasUmbrella) puppet.Behaviour = new UseUmbrellaBehaviour();
							else puppet.Behaviour = new FallBehaviour();
						}
						break;
				}
			}

			// Controleren of het level klaar is.
			this.CheckLevelComplete();

			base.Update(gameTime);
		}

		/// <summary>
		/// Het laten finishen van een Pupppet.
		/// </summary>
		/// <param name="puppet">De puppet die verwijdert moet worden, omdat die gefinisht is.</param>
		private void PuppetIsFinished(Puppet puppet)
		{
			this.RemovePuppet(puppet);
			_fisnishedPuppets++;
		}

		/// <summary>
		/// Verwijderen van de puppet
		/// </summary>
		/// <param name="puppet">De puppet die verwijdert moet worden.</param>
		private void RemovePuppet(Puppet puppet)
		{
			_listOfPuppets.Remove(puppet);
			puppet.Dispose();
			Game.Components.Remove(puppet);
		}

		/// <summary>
		/// Controleren of het level al klaar is.
		/// </summary>
		private void CheckLevelComplete()
		{
			if (_fisnishedPuppets >= _minRequiredFinishedPuppets)
			{
				_isComplete = true;
			}
			else
			{
				_isComplete = false;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			Game1.spriteBatch.Begin();

			for (int y = 0; y < _blocksY; y++)
			{
				for (int x = 0; x < _blocksX; x++)
				{
					switch (theGrid[y,x])
					{
						/*
						 * - Betekenis van de getal in het grid -
						 * 
						 * 0 = lucht
						 * 1 = grond
						 * 2 = afgrond rechts
						 * 4 = onder de grond
						 * 
						 * 10 = finish
						 */

						case 0:
							Game1.spriteBatch.Draw(_levelBg, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(101, 101, 32, 32), Color.White);
						break;
						case 1:
							Game1.spriteBatch.Draw(_levelBg, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(68, 34, 32, 32), Color.White);
						break;
						case 2:
							Game1.spriteBatch.Draw(_levelBg, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(68, 34, 32, 32), Color.White);
						break;

						case 4:
							Game1.spriteBatch.Draw(_levelBg, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(68, 68, 32, 32), Color.White);
						break;
					}
				}
			}
			
			Game1.spriteBatch.DrawString(Game.Content.Load<SpriteFont>("PuppetFont"), "Gefinishte Puppets " + _fisnishedPuppets, new Vector2( 32, GraphicsDevice.Viewport.Height - 32), Color.White);
			Game1.spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
