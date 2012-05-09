using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 
 * Author(s): Purm & cannap
 * 
 */

namespace Frohgame.Core
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
				if(Cache.LastIndexPageParser == null) {
					throw new NoCacheDataException("Cache.LastIndexPageParser == null");	
				}
				
				HtmlAgilityPack.HtmlNode spanNode = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (this._stringManager.MetalXPath);
				if(spanNode == null) {
					throw new ParsingException("Metal: span-Knoten nicht gefunden");
				}
				
				string metalString = spanNode.InnerText;
				if(string.IsNullOrEmpty(metalString)) {
					throw new ParsingException("Metal: span-Knoten Inhalt ist leer");	
				}
				
				int Result;
				try {
					Result = Utils.StringReplaceToInt32WithoutPlusAndMinus (metalString);
				} catch (FormatException) {
					throw new ParsingException("Metal: span-Knoten Inhalt ist keine Zahl");	
				}
				
				_logger.Log (LoggingCategories.Parse, "Metal: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Metall/Stunde auf dem aktuellen Planeten
		/// </summary>
		public int MetalPerHour {
			get {
				if(Cache.LastIndexPageParser == null) {
					throw new NoCacheDataException("Cache.LastIndexPageParser == null");
				}
				
				HtmlAgilityPack.HtmlNode liNode = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.MetalPerHourXPath);
				if(liNode == null) {
					throw new ParsingException("MetalPerHour: li-Knoten konnte nicht gefunden werden");	
				}
				
				HtmlAgilityPack.HtmlAttribute titleAttribute = liNode.Attributes ["title"];
				if(titleAttribute == null) {
					throw new ParsingException("MetalPerHour: title-Attribut vom li-Knoten konnte nicht gefunden werden");	
				}
				
				string tmp = titleAttribute.Value;
				if(string.IsNullOrEmpty(tmp)) {
					throw new ParsingException("MetalPerHour: title-Attribut vom li-Knoten ist leer");	
				}
				
				string tmp1 = Utils.SimpleRegex (tmp, _stringManager.MetalPerHourRegex);
				int Result = 0;
				
				try {
				if (!string.IsNullOrEmpty (tmp1))
					Result = Utils.StringReplaceToInt32WithoutPlusAndMinus (tmp1);
				} catch(FormatException) {
					throw new ParsingException("MetalPerHour: RegexResult ist keine Zahl");	
				}

				_logger.Log (LoggingCategories.Parse, "Metal per Hour: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Kristall auf dem aktuellen Planeten
		/// </summary>
		public int Crystal {
			get {
				string crystalString = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.CrystalXPath).InnerText;
				int Result = Utils.StringReplaceToInt32WithoutPlusAndMinus (crystalString);
				_logger.Log (LoggingCategories.Parse, "Crystal: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Kristall/Stunde auf dem aktuellen Planeten
		/// </summary>
		public int CrystalPerHour {
			get {
				string tmp = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.CrystalPerHourXPath).Attributes ["title"].Value;
				string tmp1 = Utils.SimpleRegex (tmp, _stringManager.CrystalPerHourRegex);
				int Result = 0;
				if (!string.IsNullOrEmpty (tmp1))
					Result = Utils.StringReplaceToInt32WithoutPlusAndMinus (tmp1);

				_logger.Log (LoggingCategories.Parse, "Crystal per Hour: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Deuterium auf dem aktuellen Planeten
		/// </summary>
		public int Deuterium {
			get {
				string tmp = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.DeuteriumXPath).InnerText;
				int Result = Utils.StringReplaceToInt32WithoutPlusAndMinus (tmp);
				_logger.Log (LoggingCategories.Parse, "Deuterium: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Deuterium/Stunde auf dem aktuellen Planeten
		/// </summary>
		public int DeuteriumPerHour {
			get {
				string tmp = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.DeuteriumPerHourXPath).Attributes ["title"].Value;
				string tmp1 = Utils.SimpleRegex (tmp, _stringManager.DeuteriumPerHourRegex);
				int Result = 0;
				if (!string.IsNullOrEmpty (tmp1))
					Result = Utils.StringReplaceToInt32WithoutPlusAndMinus (tmp1);

				_logger.Log (LoggingCategories.Parse, "Deuterium per Hour: " + Result.ToString ());
				return Result;
			}
		}

		/// <summary>
		/// Energie auf dem aktuellen Planeten
		/// </summary>
		public int Energy {
			get {
				string tmp = Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.EnergyXPath).InnerText;
				int Result = Utils.StringReplaceToInt32 (tmp);
				_logger.Log (LoggingCategories.Parse, "Energy: " + Result.ToString ());
				return Result;
			}
		}
		
		string _id;
		/// <summary>
		/// Ogamespezifische ID, wird unter anderem zum wechseln zum planeten benutzt
		/// </summary>
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		
		string _moonId;
		/// <summary>
		/// ID vom Mond, falls kein Mond vorhanden => empty string
		/// </summary>
		public string MoonId {
			get {
				return this._moonId;
			}
			set {
				_moonId = value;
			}
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
		
		FrohgameCache _cache = new FrohgameCache();
		public FrohgameCache Cache {
			get {
				return this._cache;
			}
			private set {
				_cache = value;
			}
		}		
	
		/// <summary>
		/// Liest die Level der Ressourcen-Gebäude aus.
		/// </summary>
		public Dictionary<SupplyBuildings, int> SupplyBuildingLevels {
			get {
				Dictionary<SupplyBuildings, int> buildingLevels = new Dictionary<SupplyBuildings, int>();
				
				if(Cache.LastIndexPagesParsers[(int)IndexPages.Resources] == null) {
					throw new NoCacheDataException("Keine Cachedaten für IndexPages.Resources gefunden");
				}				
				
				HtmlAgilityPack.HtmlNodeCollection col = Cache.LastIndexPagesParsers[(int)IndexPages.Resources].DocumentNode.SelectNodes(_stringManager.BuildingResearchXpath);
				
				foreach(HtmlAgilityPack.HtmlNode building in col) {
					int type = Convert.ToInt32(building.Attributes["ref"].Value);
					int level = Utils.StringReplaceToInt32WithoutPlusAndMinus(building.SelectSingleNode(_stringManager.BuildingResearchLevelXPath).InnerText);
					switch(type) {
					case (int)SupplyBuildings.CrystalBox:
						buildingLevels.Add(SupplyBuildings.CrystalBox, level);
						break;
					case (int)SupplyBuildings.Crystalmine:
						buildingLevels.Add(SupplyBuildings.Crystalmine, level);
						break;
					case (int)SupplyBuildings.DeuteriumBox:
						buildingLevels.Add(SupplyBuildings.DeuteriumBox, level);
						break;
					case (int)SupplyBuildings.DeuteriumSynthesizer:
						buildingLevels.Add(SupplyBuildings.DeuteriumSynthesizer, level);
						break;
					case (int)SupplyBuildings.FusionPowerStation:
						buildingLevels.Add(SupplyBuildings.FusionPowerStation, level);
						break;
					case (int)SupplyBuildings.HiddenCrystalBox:
						buildingLevels.Add(SupplyBuildings.HiddenCrystalBox, level);
						break;
					case (int)SupplyBuildings.HiddenDeuteriumBox:
						buildingLevels.Add(SupplyBuildings.HiddenDeuteriumBox, level);
						break;
					case (int)SupplyBuildings.HiddenMetalBox:
						buildingLevels.Add(SupplyBuildings.HiddenMetalBox, level);
						break;
					case (int)SupplyBuildings.MetalBox:
						buildingLevels.Add(SupplyBuildings.MetalBox, level);
						break;
					case (int)SupplyBuildings.Metalmine:
						buildingLevels.Add(SupplyBuildings.Metalmine, level);
						break;
					case (int)SupplyBuildings.SolarPowerPlant:
						buildingLevels.Add(SupplyBuildings.SolarPowerPlant, level);
						break;
					default: 
						break;
					}
				}
				
				return buildingLevels;
			}
		}
		
		/// <summary>
		/// Liest die Level der Station-Gebäude aus.
		/// </summary>
		public Dictionary<StationBuildings, int> StationBuildingLevels {
			get {
				Dictionary<StationBuildings, int> buildingLevels = new Dictionary<StationBuildings, int>();
				
				if(Cache.LastIndexPagesParsers[(int)IndexPages.Station] == null) {
					throw new NoCacheDataException("Keine Cachedaten für IndexPages.Station gefunden");
				}				
				
				HtmlAgilityPack.HtmlNodeCollection col = Cache.LastIndexPagesParsers[(int)IndexPages.Station].DocumentNode.SelectNodes(_stringManager.BuildingResearchXpath);
				
				foreach(HtmlAgilityPack.HtmlNode building in col) {
					int type = Convert.ToInt32(building.Attributes["ref"].Value);
					int level = Utils.StringReplaceToInt32WithoutPlusAndMinus(building.SelectSingleNode(_stringManager.BuildingResearchLevelXPath).InnerText);
					switch(type) {
					case (int)StationBuildings.AllianceDepository:
						buildingLevels.Add(StationBuildings.AllianceDepository, level);
						break;
					case (int)StationBuildings.MissileSilo:
						buildingLevels.Add(StationBuildings.MissileSilo, level);
						break;
					case (int)StationBuildings.NaniFactory:
						buildingLevels.Add(StationBuildings.NaniFactory, level);
						break;
					case (int)StationBuildings.ResearchLaboratory:
						buildingLevels.Add(StationBuildings.ResearchLaboratory, level);
						break;
					case (int)StationBuildings.RobotorFactory:
						buildingLevels.Add(StationBuildings.RobotorFactory, level);
						break;
					case (int)StationBuildings.SpaceShipYard:
						buildingLevels.Add(StationBuildings.SpaceShipYard, level);
						break;
					case (int)StationBuildings.TerraFormer:
						buildingLevels.Add(StationBuildings.TerraFormer, level);
						break;
					default:
						break;
					}
				}
				
				return buildingLevels;
			}
		}
		
		/// <summary>
		/// Liest die Level der Forschungen aus
		/// </summary>
		public Dictionary<Researches, int> ResearchLevels {
			get {
				Dictionary<Researches, int> buildingLevels = new Dictionary<Researches, int>();
				
				if(Cache.LastIndexPagesParsers[(int)IndexPages.Research] == null) {
					throw new NoCacheDataException("Keine Cachedaten für IndexPages.Research gefunden");
				}				
				
				HtmlAgilityPack.HtmlNodeCollection col = Cache.LastIndexPagesParsers[(int)IndexPages.Research].DocumentNode.SelectNodes(_stringManager.BuildingResearchXpath);
				
				foreach(HtmlAgilityPack.HtmlNode building in col) {
					int type = Convert.ToInt32(building.Attributes["ref"].Value);
					int level = Utils.StringReplaceToInt32WithoutPlusAndMinus(building.SelectSingleNode(_stringManager.BuildingResearchLevelXPath).InnerText);
					switch(type) {
					case (int)Researches.ArmorTech:
						buildingLevels.Add(Researches.ArmorTech, level);
						break;
					case (int)Researches.AstroPhysics:
						buildingLevels.Add(Researches.AstroPhysics, level);
						break;
					case (int)Researches.BurningEngine:
						buildingLevels.Add(Researches.BurningEngine, level);
						break;
					case (int)Researches.ComputerTech:
						buildingLevels.Add(Researches.ComputerTech, level);
						break;
					case (int)Researches.EnergyTech:
						buildingLevels.Add(Researches.EnergyTech, level);
						break;
					case (int)Researches.GravitonResearch:
						buildingLevels.Add(Researches.GravitonResearch, level);
						break;
					case (int)Researches.HyperRoomEngine:
						buildingLevels.Add(Researches.HyperRoomEngine, level);
						break;
					case (int)Researches.HyperRoomTech:
						buildingLevels.Add(Researches.HyperRoomTech, level);
						break;
					case (int)Researches.ImpulsEngine:
						buildingLevels.Add(Researches.ImpulsEngine, level);
						break;
					case (int)Researches.IntergalacticNetwork:
						buildingLevels.Add(Researches.IntergalacticNetwork, level);
						break;
					case (int)Researches.IonenTech:
						buildingLevels.Add(Researches.IonenTech, level);
						break;
					case (int)Researches.LaserTech:
						buildingLevels.Add(Researches.LaserTech, level);
						break;
					case (int)Researches.PlasmaTech:
						buildingLevels.Add(Researches.PlasmaTech, level);
						break;
					case (int)Researches.ShieldTech:
						buildingLevels.Add(Researches.ShieldTech, level);
						break;
					case (int)Researches.SpyingTech:
						buildingLevels.Add(Researches.SpyingTech, level);
						break;
					case (int)Researches.WeaponTech:
						buildingLevels.Add(Researches.WeaponTech, level);
						break;
					default:
						break;
					}
				}
				
				return buildingLevels;
			}
		}
		
		StringManager _stringManager;
		Logger _logger;
		
		/// <summary>
		/// parset planetinformationen
		/// </summary>
		/// <param name="planetNode">Html Knoten, der die Planetinformationen enthält</param>
		public Planet (HtmlAgilityPack.HtmlNode planetNode, StringManager strings, Logger logger)
		{
			logger.Log (LoggingCategories.Parse, "Planet Constructor");
			this._stringManager = strings;
			this._logger = logger;
			
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
			this._id = Utils.ReplaceEverythingsExceptNumbers (Utils.SimpleRegex (linktToPlanet, strings.MoonOrPlanetIDRegex));
			
			//Mond?
			HtmlAgilityPack.HtmlNode moonNode = planetNode.SelectSingleNode(_stringManager.MoonXPath);
			if(moonNode != null) {
				string linkToMoon = moonNode.Attributes["href"].Value;
				this._moonId = Utils.ReplaceEverythingsExceptNumbers(Utils.SimpleRegex(linkToMoon, strings.MoonOrPlanetIDRegex));
				Console.WriteLine("There is a moon, buddy and his id is: " + _moonId.ToString());
			}
			else {
				this._moonId = string.Empty;	
			}
		}
	}
}
