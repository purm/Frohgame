using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 
 * Author(s): Purm & cannap
 * 
 */

namespace FROHGAME.Core
{
	/// <summary>
	/// Stellt die Position eines Planeten dar
	/// </summary>
	public class PlanetPosition
	{
		int _place;
		/// <summary>
		/// Platz im sonnensystem
		/// </summary>
		public int Place {
			get { return _place; }
			set { _place = value; }
		}

		int _sunSystem;
		/// <summary>
		/// Sonnensystem
		/// </summary>
		public int SunSystem {
			get { return _sunSystem; }
			set { _sunSystem = value; }
		}

		int _galaxy;
		/// <summary>
		/// GAlaxie
		/// </summary>
		public int Galaxy {
			get { return _galaxy; }
			set { _galaxy = value; }
		}
	}

	/// <summary>
	/// Stellt einen Planeten dar
	/// </summary>
	public class Planet
	{
		/// <summary>
		/// parset planetinformationen
		/// </summary>
		/// <param name="planetNode">Html Knoten, der die Planetinformationen enthält</param>
		public Planet (HtmlAgilityPack.HtmlNode planetNode, StringManager strings, Logger logger)
		{
			logger.Log (LoggingCategories.Parse, "Planet Constructor");
			this._name = planetNode.SelectSingleNode (strings.PlanetNameXPath).InnerText;

			//Koordinaten auslesen:
			string coordsString = planetNode.SelectSingleNode (strings.PlanetCoordsXPath).InnerText;
			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match (coordsString, strings.PlanetCoordsRegex);

			//Koordinaten Parsen
			this._coords.Galaxy = Convert.ToInt32 (match.Groups [1].Value);
			this._coords.SunSystem = Convert.ToInt32 (match.Groups [2].Value);
			this._coords.Place = Convert.ToInt32 (match.Groups [3].Value);

			//Link auslesen:
			string linktToPlanet = planetNode.SelectSingleNode (strings.PlanetLinkXPath).Attributes ["href"].Value;
			this._id = Utils.StringReplaceToInt32 (Utils.SimpleRegex (linktToPlanet, strings.PlanetIDRegex));
		}

		int _id;
		/// <summary>
		/// Ogamespezifische ID, wird unter anderem zum wechseln zum planeten benutzt
		/// </summary>
		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		PlanetPosition _coords = new PlanetPosition ();
		/// <summary>
		/// Koordinaten des Planeten
		/// </summary>
		public PlanetPosition Coords {
			get { return _coords; }
			set { _coords = value; }
		}

		private string _name;
		/// <summary>
		/// Name des Planeten
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
	}
}
