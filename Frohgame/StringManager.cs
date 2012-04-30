using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FROHGAME.Core;

/*
 * 
 * Author(s): Purm & cannap
 * 
 */

namespace FROHGAME
{
	/// <summary>
	/// Stellt eine Reihe von Regex & Url Konstaten dar, sodass nach einem Ogame-Update nur diese Datei aktualisiert werden muss
	/// </summary>
	[Serializable()]
	public class StringManager
	{

        #region Properties
		
		/// <summary>
		/// Xpath zum auslesen der Gebäude/Forschungen
		/// </summary>
		internal string BuildingResearchXpath {
			get {
				return @"//div[@class='buildingimg']/a[@ref]";	
			}
		}
		
		/// <summary>
		/// Xpath zum auslesen der Gebäude/Forschungs Level
		/// </summary>
		internal string BuildingResearchLevelXPath {
			get {
				return @".//span[@class='level']";	
			}
		}
		
		/// <summary>
		/// Regex zum auslesen der 'Ogame'-Version aus den Metadaten
		/// </summary>
		internal string VersionRegex {
			get {
				return "//meta[@name='ogame-version']";
			}
		}

		string _server;
		/// <summary>
		/// Serverstring
		/// </summary>
		public string Server {
			get { return _server; }
			set {
				_server = value;

				//Topleveldomain setzen^^
				string[] serverSplitted = _server.Split ('.');
				this._tldDomain = serverSplitted [serverSplitted.Length - 1];
			}
		}

		string _name;
		/// <summary>
		/// Benutzername
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		string _password;
		/// <summary>
		/// Benutzerpasswort
		/// </summary>
		public string Password {
			get { return _password; }
			set { _password = value; }
		}

		string _tldDomain;
		/// <summary>
		/// Topleveldomain, z.B. "de" oder "com"
		/// </summary>
		public string TldDomain {
			get { return _tldDomain; }
		}

		/// <summary>
		/// String zum verifizieren, ob der Logout-Link gefunden werden kann
		/// </summary>
		internal string IsLoggedinXPath {
			get {
				return "//meta[@name='ogame-session']";
			}
		}

		/// <summary>
		/// Startpage Url
		/// </summary>
		internal string StartUrl {
			get {
				return String.Format (@"http://ogame.{0}", this._tldDomain);
			}
		}

		/// <summary>
		/// Login Url
		/// </summary>
		internal string LoginUrl {
			get {
				return String.Format (@"http://{0}/game/reg/login2.php", this._server);
			}
		}

		/// <summary>
		/// Login Post-Parameter
		/// </summary>
		internal string LoginParameter {
			get {
				return String.Format (@"v=2&is_utf8=0&uni_url={0}&login={1}&pass={2}", this._server, System.Web.HttpUtility.UrlEncode (this._name), System.Web.HttpUtility.UrlEncode (this._password));
			}
		}

		/// <summary>
		/// Regex zum finden des Tokens
		/// </summary>
		internal string TokenXPath {
			get {
				return "//input[@name='token']";
			}
		}

        #region  Ressourcen des aktuellen Planeten XPath & Regex
		
		/// <summary>
		/// XPath zum auslesen des Metalls -> InnerText
		/// </summary>
		internal string MetalXPath {
			get {
				return "//span[@id='resources_metal']";
			}
		}

		/// <summary>
		/// Regex für Metall in der stunde
		/// </summary>
		internal string MetalPerHourRegex {
			get {
				return "<span class='undermark'>([^<]*)</span>";
			}
		}

		/// <summary>
		/// Regex für Metall in der stunde
		/// </summary>
		internal string MetalPerHourXPath {
			get {
				return "//li[@id='metal_box']";
			}
		} 

		/// <summary>
		/// XPath zum auslesen des Kristalls -> InnerText
		/// </summary>
		internal string CrystalXPath {
			get {
				return "//span[@id='resources_crystal']";
			}
		}

		/// <summary>
		/// Regex für Kristall in  der Stunde
		/// </summary>
		internal string CrystalPerHourRegex {
			get {
				//bei jeder ressource gleich^^
				return MetalPerHourRegex;
			}
		}

		/// <summary>
		/// Regex für Kristall in der stunde
		/// </summary>
		internal string CrystalPerHourXPath {
			get {
				return "//li[@id='crystal_box']";
			}
		} 

		/// <summary>
		/// XPath zum auslesen des Deuteriums -> InnerText
		/// </summary>
		internal string DeuteriumXPath {
			get {
				return "//span[@id='resources_deuterium']";
			}
		}

		/// <summary>
		/// Regex für Kristall in  der Stunde
		/// </summary>
		internal string DeuteriumPerHourRegex {
			get {
				//bei jeder ressource gleich^^
				return MetalPerHourRegex;
			}
		}

		/// <summary>
		/// Regex für Deuterium in der stunde
		/// </summary>
		internal string DeuteriumPerHourXPath {
			get {
				return "//li[@id='deuterium_box']";
			}
		} 

		/// <summary>
		/// XPath zum auslesen der dunklen Materie
		/// </summary>
		internal string DarkMatterXPath {
			get {
				return "//span[@id='resources_darkmatter']";
			}
		}

		/// <summary>
		/// XPath zum auslesen der Energie
		/// </summary>
		internal string EnergyXPath {
			get {
				return "//span[@id='resources_energy']";
			}
		}

        #endregion

		/// <summary>
		/// XPath für Level eines Gebäudes
		/// </summary>
		internal string BuildCurLevelXPath {
			get {
				return ".//span[@class='level']";
			}
		}

		/// <summary>
		/// XPath für PlanetReader
		/// </summary>
		internal string PlanetListXPath {
			get {
				return "//div[@class='smallplanet']";
			}
		}

		/// <summary>
		/// XPath zum auslesen eines Planeten names
		/// </summary>
		internal string PlanetNameXPath {
			get {
				return ".//span[@class='planet-name']";
			}
		}

		/// <summary>
		/// XPath zum auslesen der Koordinaten eines planeten als string
		/// </summary>
		internal string PlanetCoordsXPath {
			get {
				return ".//span[@class='planet-koords']";
			}
		}

		/// <summary>
		/// Regex zum parsen der koordinaten eines planeten
		/// </summary>
		internal string PlanetCoordsRegex {
			get {
				return @"\[([0-9]+)\:([0-9]+)\:([0-9]+)\]";
			}
		}

		/// <summary>
		/// XPath zum auslesen des Planeten links
		/// </summary>
		internal string PlanetLinkXPath {
			get {
				return ".//a";
			}
		}

		/// <summary>
		/// Regex zum parsen der ID aus dem Link eine planeten
		/// </summary>
		internal string PlanetIDRegex {
			get {
				return "cp=([0-9]+)";
			}
		}

        #region Ressourcen der Gebäude Regex

		/// <summary>
		/// Liest das Metal aus was gebraucht wird
		/// </summary>
		internal string NeededMetalRegex {
			get {
				return @"<li class=""metal tipsStandard"" title=""\|(\S*) Metall"">";
			}
		}

		/// <summary>
		/// Liest das Kristall aus was gebraucht wird
		/// </summary>
		internal string NeededCrystalRegex {
			get {
				return @"<li class=""metal tipsStandard"" title=""\|(\S*) Kristall"">";
			}
		}

		/// <summary>
		/// Liest das Deuterium aus was gebraucht wird
		/// </summary>
		internal string NeededDeuteriumRegex {
			get {
				return @"<li class=""metal tipsStandard"" title=""\|(\S*) Deuterium"">";
			}
		}


        #endregion
        #endregion

		private Dictionary<IndexPages, string> _indexPageNames = new Dictionary<IndexPages, string> ();
		/// <summary>
		/// gibt den Namen einer index page zurück
		/// </summary>
		internal Dictionary<IndexPages, string> IndexPageNames {
			get { return _indexPageNames; }
			set { _indexPageNames = value; }
		}



        #region Constructors

		internal StringManager (string name, string password, string server)
		{
			this.Server = server;
			this._password = password;
			this._name = name;

			//Index-Page Namen festlegen
			_indexPageNames.Add (IndexPages.Alliance, "alliance");
			_indexPageNames.Add (IndexPages.Changelog, "changelog");
			_indexPageNames.Add (IndexPages.Defense, "defense");
			_indexPageNames.Add (IndexPages.Fleet1, "fleet1");
			_indexPageNames.Add (IndexPages.Galaxy, "galaxy");
			_indexPageNames.Add (IndexPages.Overview, "overview");
			_indexPageNames.Add (IndexPages.Premium, "premium");
			_indexPageNames.Add (IndexPages.Research, "research");
			_indexPageNames.Add (IndexPages.Resources, "resources");
			_indexPageNames.Add (IndexPages.Shipyard, "shipyard");
			_indexPageNames.Add (IndexPages.Station, "station");
			_indexPageNames.Add (IndexPages.Trader, "trader");
		}

        #endregion

        #region Internal Methods

		/// <summary>
		/// Url für Ajax Anfragen
		/// </summary>
		/// <param name="currentPage">Index-Page</param>
		/// <param name="ajaxIndex">Ajax id</param>
		/// <returns>Url für Ajax Anfragen</returns>
		internal string GetAjaxUrl (string currentPage, int ajaxIndex)
		{
			return String.Format (@"http://{0}/game/index.php?page={1}&ajax={2}", this._server, currentPage, ajaxIndex.ToString ());
		}

		/// <summary>
		/// Post Parameter für Ajax Anfragen
		/// </summary>
		/// <param name="type">Type (SupplyBuildings)</param>
		/// <returns>Post Parameter für Ajax Anfragen</returns>
		internal string GetAjaxParameter (SupplyBuildings type)
		{
			return String.Format ("type={0}", ((int)type).ToString ());
		}

		/// <summary>
		/// Post Parameter für Ajax Anfragen
		/// </summary>
		/// <param name="type">Type (StationBuildings)</param>
		/// <returns>Post Parameter für Ajax Anfragen</returns>
		internal string GetAjaxParameter (StationBuildings type)
		{
			return String.Format ("type={0}", ((int)type).ToString ());
		}

		/// <summary>
		/// Url einer Index-Page
		/// </summary>
		/// <param name="page">Index-Page</param>
		/// <returns>Url einer Index-Page</returns>
		internal string GetIndexPageUrl (IndexPages page)
		{
			return String.Format ("http://{0}/game/index.php?page={1}", Server, this._indexPageNames [page]);
		}

		/// <summary>
		/// Parameter zum absenden des AjaxFormulars zum gebäude upgrade/bau
		/// </summary>
		/// <param name="token">token</param>
		/// <param name="building">geböude</param>
		/// <returns></returns>
		internal string GetUpgradeBuildingSubmitParameter (string token, SupplyBuildings building)
		{
			return String.Format ("token={0}&modus=1&type={1}", token, ((int)building).ToString ());
		}

		/// <summary>
		/// Parameter zum absenden des AjaxFormulars zum gebäude upgrade/bau
		/// </summary>
		/// <param name="token">token</param>
		/// <param name="building">geböude</param>
		/// <returns></returns>
		internal string GetUpgradeBuildingSubmitParameter (string token, StationBuildings building)
		{
			return String.Format ("token={0}&modus=1&type={1}", token, ((int)building).ToString ());
		}

        #endregion
	}
}