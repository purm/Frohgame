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
	[Serializable()]
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
	[Serializable()]
	public class Planet
	{
		/// <summary>
		/// Metall auf dem aktuellen Planeten
		/// </summary>
		public int Metal {
			get {
				string metalString = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.MetalXPath).InnerText;
				int Result = Utils.StringReplaceToInt32 (metalString);
				Logger.Log (LoggingCategories.Parse, "Metal: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Metall/Stunde auf dem aktuellen Planeten
		/// </summary>
		public int MetalPerHour {
			get {
				string tmp = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.MetalPerHourXPath).Attributes ["title"].Value;
				string tmp1 = Utils.SimpleRegex (tmp, _stringManager.MetalPerHourRegex);
				int Result = 0;
				if (!string.IsNullOrEmpty (tmp1))
					Result = Utils.StringReplaceToInt32 (tmp1);

				Logger.Log (LoggingCategories.Parse, "Metal per Hour: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Kristall auf dem aktuellen Planeten
		/// </summary>
		public int Crystal {
			get {
				string crystalString = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.CrystalXPath).InnerText;
				int Result = Utils.StringReplaceToInt32 (crystalString);
				Logger.Log (LoggingCategories.Parse, "Crystal: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Kristall/Stunde auf dem aktuellen Planeten
		/// </summary>
		public int CrystalPerHour {
			get {
				string tmp = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.CrystalPerHourXPath).Attributes ["title"].Value;
				string tmp1 = Utils.SimpleRegex (tmp, _stringManager.CrystalPerHourRegex);
				int Result = 0;
				if (!string.IsNullOrEmpty (tmp1))
					Result = Utils.StringReplaceToInt32 (tmp1);

				Logger.Log (LoggingCategories.Parse, "Crystal per Hour: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Deuterium auf dem aktuellen Planeten
		/// </summary>
		public int Deuterium {
			get {
				string tmp = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.DeuteriumXPath).InnerText;
				int Result = Utils.StringReplaceToInt32 (tmp);
				Logger.Log (LoggingCategories.Parse, "Deuterium: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Deuterium/Stunde auf dem aktuellen Planeten
		/// </summary>
		public int DeuteriumPerHour {
			get {
				string tmp = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.DeuteriumPerHourXPath).Attributes ["title"].Value;
				string tmp1 = Utils.SimpleRegex (tmp, _stringManager.DeuteriumPerHourRegex);
				int Result = 0;
				if (!string.IsNullOrEmpty (tmp1))
					Result = Utils.StringReplaceToInt32 (tmp1);

				Logger.Log (LoggingCategories.Parse, "Deuterium per Hour: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Energie auf dem aktuellen Planeten
		/// </summary>
		public int Energy {
			get {
				string tmp = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.EnergyXPath).InnerText;
				int Result = Utils.StringReplaceToInt32 (tmp);
				Logger.Log (LoggingCategories.Parse, "Energy: " + Result.ToString ());
				return Result;
			}
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
	}
}
