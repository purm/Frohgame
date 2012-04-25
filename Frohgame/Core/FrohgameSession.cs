using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FROHGAME.Http;
using System.Xml.XPath;

/*
 * 
 * Author(s): Purm & cannap
 * 
 */

namespace FROHGAME.Core
{
	public class FrohgameSession
	{
        #region Private Fields

		StringManager _stringManager;//test

        #endregion

        #region Properties

		HttpHandler _httpHandler = new HttpHandler ("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko/20100101 Firefox/11.0");
		/// <summary>
		/// Http-Handler, zum abbonieren des Navigate Events
		/// </summary>
		public HttpHandler HttpHandler {
			get { return _httpHandler; }
			set { _httpHandler = value; }
		}

		Logger _logger = new Logger ();
		/// <summary>
		/// Logging Klasse, Log-Ereignis kann abbonniert werden
		/// </summary>
		public Logger Logger {
			get { return _logger; }
			set { _logger = value; }
		}

		HtmlAgilityPack.HtmlDocument htmlParser = new HtmlAgilityPack.HtmlDocument ();
		/// <summary>
		/// Zum Html parsen
		/// </summary>
		public HtmlAgilityPack.HtmlDocument HtmlParser {
			get { return htmlParser; }
			set { htmlParser = value; }
		}

		HttpResult __lastResult;

		/// <summary>
		/// Bitte NUR bei Abfragen Festlegen, welche im content Metall, erz, links etc enthalten => keine ajax abfragen etc hier storen, sondern nur INDEX-PAGES
		/// </summary>
		HttpResult _lastResult {
			get {
				return __lastResult;
			}
			set {
				this.__lastResult = value;

				htmlParser.LoadHtml (value.ResponseContent);
			}
		}
		/// <summary>
		/// Das zuletzt zurückbekommene Http Ergebnis
		/// </summary>
		internal HttpResult LastResult {
			get { return _lastResult; }
		}

		string _userPassword;
		/// <summary>
		/// Passwort des Benutzers
		/// </summary>
		public string UserPassword {
			get { return _userPassword; }
		}

		string _userName;
		/// <summary>
		/// Name des Benutzers
		/// </summary>
		public string UserName {
			get { return _userName; }
		}

		string _server;
		/// <summary>
		/// Server auf dem der Benutzer spielt
		/// </summary>
		public string Server {
			get { return _server; }
		}

		/// <summary>
		/// Liest die 'Ogame'-Version aus den Meta Daten aus
		/// </summary>
		public string Version {
			get {
				string version = Utils.SimpleRegex (_lastResult.ResponseContent, _stringManager.VersionRegex);
				Logger.Log (LoggingCategories.Parse, "Version: " + version);
				return version;
			}
		}

		/// <summary>
		/// Token, welches unter anderem beim Bau von Gebäuden benötigt wird
		/// Gibt nur etwas zurück, wenn _lastResult == der resourcenPage oder der stationPage ist
		/// </summary>
		public string Token {
			get {
				string token = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.TokenXPath).Attributes ["value"].Value;
				Logger.Log (LoggingCategories.Parse, "Token: " + (!string.IsNullOrEmpty (token) ? token : "None"));
				return token;
			}
		}

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
		/// Dunkle Materie
		/// </summary>
		public int DarkMatter {
			get {
				string tmp = HtmlParser.DocumentNode.SelectSingleNode (_stringManager.DarkMatterXPath).InnerText;
				int Result = Utils.StringReplaceToInt32 (tmp);
				Logger.Log (LoggingCategories.Parse, "DarkMatter: " + Result.ToString ());
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

        #endregion

        #region Contructors

		/// <summary>
		/// Erstellt eine neue FrohgameSession
		/// </summary>
		/// <param name="name">Username</param>
		/// <param name="password">Userpass</param>
		/// <param name="server">Server des users. example: "uni42.ogame.de"</param>
		/// <param name="userAgent">Useragent, der beim Browser-Simulator verwendet werden soll</param>
		public FrohgameSession (string name, string password, string server, string userAgent)
            : this(name, password, server)
		{
			HttpHandler.UserAgent = userAgent;
		}

		/// <summary>
		/// Erstellt eine neue FrohgameSession und versucht sich einzuloggen
		/// </summary>
		/// <param name="name">Username</param>
		/// <param name="password">Userpass</param>
		/// <param name="server">Server des users. example: "uni42.ogame.de"</param>
		public FrohgameSession (string name, string password, string server)
		{
			if (string.IsNullOrEmpty (name))
				throw new ArgumentNullException ("name");

			if (string.IsNullOrEmpty (password))
				throw new ArgumentNullException ("password");

			if (string.IsNullOrEmpty (server))
				throw new ArgumentNullException ("server");

			this._server = server;
			this._userName = name;
			this._userPassword = password;

			_stringManager = new StringManager (this._userName, this._userPassword, this._server);
		}

        #endregion

        #region Public Methods

		/// <summary>
		/// Navigiert zu einer Ogame-Standard Seite
		/// </summary>
		/// <param name="page">Ogame-Standard Seite</param>
		public HttpResult NagivateToIndexPage (IndexPages page)
		{
			Logger.Log (LoggingCategories.NavigationAction, "NagivateToIndexPage(" + _stringManager.IndexPageNames [page] + ")");
			return this._lastResult = HttpHandler.Get (this._stringManager.GetIndexPageUrl (page));
		}

		/// <summary>
		/// Versucht sich einzuloggen
		/// </summary>
		public void Login ()
		{
			Logger.Log (LoggingCategories.NavigationAction, "Login");
			//Zur Ogame Startseite navigieren
			Logger.Log (LoggingCategories.NavigationAction, "Navigate to Startpage");

			HttpHandler.Get (_stringManager.StartUrl);

			//Logindaten senden^^
			Logger.Log (LoggingCategories.NavigationAction, "Sending Login Data");
			this._lastResult = HttpHandler.Post (_stringManager.LoginUrl, _stringManager.LoginParameter);

			//Nach Logout Link suchen... falls vorhanden => login war erfolgreich, sonst nicht
			if (!Regex.IsMatch (LastResult.ResponseContent, _stringManager.LogoutRegex))
				throw new LoginFailedException ("Login failed (LogoutRegex) not found");

			Logger.Log (LoggingCategories.NavigationAction, "Login was successfull");

			//@CANNAP: DEIN PLANET AUSLESEN KRAM HAT BEI MIR GECRASHT

			//Todo Prüfen ob ein Gebäude im Bau ist
		}

		/// <summary>
		/// Versucht ein Gebäude auf dem aktuell-angewählten Planeten auszubauen
		/// </summary>
		/// <param name="building">gebäude id</param>
		public void UpgradeBuilding (SupplyBuildings building)
		{
			Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding(" + building.ToString () + ")");
			//Falls Token nicht gefunden wird zur entsprechenden Seite navigieren
			if (this._lastResult.ResponseUrl.ToString () != _stringManager.GetIndexPageUrl (IndexPages.Resources)) {
				Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding: Wir sind noch nicht auf der Bau-Seite");
				NagivateToIndexPage (IndexPages.Resources);
			} else {
				Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding: Wir sind bereits auf der Bau-Seite");
			}

			HttpResult tmp = NavigateBuildingAjax (1, building);

			HtmlAgilityPack.HtmlDocument tmpDoc = new HtmlAgilityPack.HtmlDocument ();
			tmpDoc.LoadHtml (tmp.ResponseContent);

			int neededMetal = GetMetalFromAjax (tmp.ResponseContent);
			int neededCrystal = GetCrystalFromAjax (tmp.ResponseContent);
			int neededDeuterium = GetDeuteriumFromAjax (tmp.ResponseContent);

			int currentLevel = GetBuildingLevelFromAjax (tmpDoc);

			//abgleichen mit aktuellen ressies
			if (this.Metal < neededMetal) {
				throw new NotEnoughMetalException ("Nicht genug Metall zum bau von " + building.ToString ());
			} else if (this.Crystal < neededCrystal) {
				Mathemathics.CalcMaxTimeForRes (this.Metal, this.Crystal, this.Deuterium, neededMetal, neededCrystal, neededDeuterium, MetalPerHour, CrystalPerHour, DeuteriumPerHour);
				throw new NotEnoughCrystalException ("Nicht genug Kristall zum bau von " + building.ToString ());
			} else if (this.Deuterium < neededDeuterium) {
				throw new NotEnoughDeuteriumException ("Nicht genug Deuterium zum bau von " + building.ToString ());
			}
			HttpHandler.Post (_stringManager.GetIndexPageUrl (IndexPages.Resources), _stringManager.GetUpgradeBuildingSubmitParameter (this.Token, building));
		}

		/// <summary>
		/// Versucht ein Gebäude auf dem aktuell-angewählten Planeten auszubauen
		/// </summary>
		/// <param name="building">gebäude id</param>
		public void UpgradeBuilding (StationBuildings building)
		{
			Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding(" + building.ToString () + ")");
			//Falls Token nicht gefunden wird zur entsprechenden Seite navigieren
			if (this._lastResult.ResponseUrl.ToString () != _stringManager.GetIndexPageUrl (IndexPages.Station)) {
				Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding: Wir sind noch nicht auf der Bau-Seite");
				NagivateToIndexPage (IndexPages.Station);
			} else {
				Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding: Wir sind bereits auf der Bau-Seite");
			}

			HttpResult tmp = NavigateBuildingAjax (1, building);

			HtmlAgilityPack.HtmlDocument tmpDoc = new HtmlAgilityPack.HtmlDocument ();
			tmpDoc.LoadHtml (tmp.ResponseContent);

			int neededMetal = GetMetalFromAjax (tmp.ResponseContent);
			int neededCrystal = GetCrystalFromAjax (tmp.ResponseContent);
			int neededDeuterium = GetDeuteriumFromAjax (tmp.ResponseContent);
			int currentLevel = GetBuildingLevelFromAjax (tmpDoc);

			//abgleichen mit aktuellen ressies
			if (this.Metal < neededMetal) {
				throw new NotEnoughMetalException ("Nicht genug Metall zum bau von " + building.ToString ());
			} else if (this.Crystal < neededCrystal) {
				throw new NotEnoughCrystalException ("Nicht genug Kristall zum bau von " + building.ToString ());
			} else if (this.Deuterium < neededDeuterium) {
				throw new NotEnoughDeuteriumException ("Nicht genug Deuterium zum bau von " + building.ToString ());
			}
			HttpHandler.Post (_stringManager.GetIndexPageUrl (IndexPages.Station), _stringManager.GetUpgradeBuildingSubmitParameter (this.Token, building));
		}

        #endregion

        #region Private Methods

		/// <summary>
		/// Öffnet den Ajax Javascript kram vom Gebäude-Baucenter
		/// </summary>
		/// <param name="ajaxIndex">immer 1 setzen vorerst (nochnicht herrausgefunden, was der parameter bringt)</param>
		/// <param name="ajaxParam">gebäude id</param>
		/// <returns>Http Ergebnis</returns>
		HttpResult NavigateBuildingAjax (int ajaxIndex, SupplyBuildings ajaxParam)
		{
			Logger.Log (LoggingCategories.NavigationAction, "NavigateBuildingAjax(" + ajaxIndex.ToString () + ", " + ajaxParam.ToString () + ")");
			return this._httpHandler.Post (_stringManager.GetAjaxUrl (_stringManager.IndexPageNames [IndexPages.Resources], ajaxIndex), _stringManager.GetAjaxParameter (ajaxParam));
		}

		/// <summary>
		/// Öffnet den Ajax Javascript kram vom Gebäude-Baucenter
		/// </summary>
		/// <param name="ajaxIndex">immer 1 setzen vorerst (nochnicht herrausgefunden, was der parameter bringt)</param>
		/// <param name="ajaxParam">gebäude id</param>
		/// <returns>Http Ergebnis</returns>
		HttpResult NavigateBuildingAjax (int ajaxIndex, StationBuildings ajaxParam)
		{
			Logger.Log (LoggingCategories.NavigationAction, "NavigateBuildingAjax(" + ajaxIndex.ToString () + ", " + ajaxParam.ToString () + ")");
			return this._httpHandler.Post (_stringManager.GetAjaxUrl (_stringManager.IndexPageNames [IndexPages.Station], ajaxIndex), _stringManager.GetAjaxParameter (ajaxParam));
		}

		/// <summary>
		/// Liest Level eines Gebäudes aus 
		/// </summary>
		/// <param name="ajaxHTML">Quelltext</param>
		/// <returns>Level</returns>
		private int GetBuildingLevelFromAjax (HtmlAgilityPack.HtmlDocument ajaxHTML)
		{
			int Result = Utils.StringReplaceToInt32 (ajaxHTML.DocumentNode.SelectSingleNode (_stringManager.BuildCurLevelXPath).InnerText);
			Logger.Log (LoggingCategories.Parse, "GetBuildingLevelFromAjax: " + Result.ToString ());
			return Result;
		}

		/// <summary>
		/// Liest Metal aus was gebraucht wird 
		/// </summary>
		/// <param name="ajaxHTML">Quelltext</param>
		/// <returns>Metal or 0 </returns>
		private int GetMetalFromAjax (string ajaxHTML)
		{
			string tmp = Utils.SimpleRegex (ajaxHTML, _stringManager.NeededMetalRegex);
			int Result = 0;
			if (!string.IsNullOrEmpty (tmp)) {
				Result = Utils.StringReplaceToInt32 (tmp);
			}

			Logger.Log (LoggingCategories.Parse, "GetMetalFromAjax: " + Result.ToString ());
			return Result;
		}

		/// <summary>
		/// Liest Kristall aus was gebraucht wird 
		/// </summary>
		/// <param name="ajaxHTML">Quelltext</param>
		/// <returns>Crystal or 0 </returns>
		private int GetCrystalFromAjax (string ajaxHTML)
		{
			string tmp = Utils.SimpleRegex (ajaxHTML, _stringManager.NeededCrystalRegex);
			int Result = 0;
			if (!string.IsNullOrEmpty (tmp)) {
				Result = Utils.StringReplaceToInt32 (tmp);
			}

			Logger.Log (LoggingCategories.Parse, "GetCrystalFromAjax: " + Result.ToString ());
			return Result;
		}

		/// <summary>
		/// Liest Deuterium aus was gebraucht wird 
		/// </summary>
		/// <param name="ajaxHTML">Quelltext</param>
		/// <returns>Deuterium or 0 </returns>
		private int GetDeuteriumFromAjax (string ajaxHTML)
		{
			string tmp = Utils.SimpleRegex (ajaxHTML, _stringManager.NeededDeuteriumRegex);
			int Result = 0;
			if (!string.IsNullOrEmpty (tmp)) {
				Result = Utils.StringReplaceToInt32 (tmp);
			}

			Logger.Log (LoggingCategories.Parse, "GetDeuteriumFromAjax: " + Result.ToString ());
			return Result;
		}
        #endregion

        #region Planet Switcher

		/// <summary>
		/// Lese Planeten aus und füge Sie in planetList ein 
		/// </summary>
		public List<Planet> PlanetList {
			get {
				List<Planet> ret = new List<Planet> ();
				HtmlAgilityPack.HtmlNodeCollection planetNodes = HtmlParser.DocumentNode.SelectNodes (_stringManager.PlanetListXPath);

				foreach (HtmlAgilityPack.HtmlNode planetNode in planetNodes) {
					ret.Add (new Planet (planetNode, _stringManager, Logger));
				}

				return ret;
			}
		}

		/// <summary>
		/// Wechselt auf einen anderen Planet ist gültig für die ganze session
		/// </summary>
		/// <param name="planetListID">Die Nummer des Planeten</param>
		/// <returns>Planetname auf dem man sich nun Befindet</returns>
		public string PlanetChanger (Planet planet)
		{ //ToDo
			return "Planetname";
		}

        #endregion
	}
}