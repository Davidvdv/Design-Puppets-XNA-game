using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets
{
	/// <summary>
	/// Speler class waarin allerlei gegevens van de speler in opgeslagen kan worden.
	/// Dat is nu alleen de score.
	/// </summary>
	public class Player
	{
		public Player(string name)
		{
			Score = 0;
		}

		public int Score
		{
			get;
			set;
		}

		public int HighestLevel
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
	}
}
