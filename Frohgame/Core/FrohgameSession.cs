using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Frohgame.Http;
using System.Xml.XPath;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

/*
 * 
 * Author(s): Purm & cannap
 * 
 */

namespace Frohgame.Core
{
	[Serializable()]
	public class FrohgameSession
	{
        #region Private Fields

		StringManager _stringManager;//test
		
        #endregion

        #region Properties
		
		/// <summary>
		/// Überprüft ob die session noch gültig ist
		/// </summary>
		/// <returns>
		/// <c>True</c> falls gültig, sonst <c>falsch</c>.
		/// </returns>
		/// <param name='refresh'>
		/// Wenn <c>true</c> wird die Overview Seite neu geladen, sonst wird aus dem cache geladen.
		/// </param>
		public bool IsLoggedIn(bool refresh) {
			if(refresh)
				NagivateToIndexPage(IndexPages.Overview);
			
			HtmlAgilityPack.HtmlNode node = AccountCache.LastIndexPageParser.DocumentNode.SelectSingleNode(_stringManager.IsLoggedinXPath);
			if(node == null)
				return false;
				
			if(node.Attributes["content"] == null) {
				return false;	
			}
			
			return true;
		}
		
		public Frohgame.Core.Mathemathics.Calculator Calculator = new Frohgame.Core.Mathemathics.Calculator();
		
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
		
		Frohgame.FrohgameCache _accountCache = new Frohgame.FrohgameCache();
		public Frohgame.FrohgameCache AccountCache {
			get {
				return this._accountCache;
			}
			set {
				_accountCache = value;
			}
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
				string version = AccountCache.LastIndexPageParser.DocumentNode.SelectSingleNode(_stringManager.VersionRegex).Attributes["content"].Value;
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
				string token = this.CurrentPlanet.Cache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.TokenXPath).Attributes ["value"].Value;
				Logger.Log (LoggingCategories.Parse, "Token: " + (!string.IsNullOrEmpty (token) ? token : "None"));
				return token;
			}
		}
		
		/// <summary>
		/// Dunkle Materie
		/// </summary>
		public int DarkMatter {
			get {
				string tmp = AccountCache.LastIndexPageParser.DocumentNode.SelectSingleNode (_stringManager.DarkMatterXPath).InnerText;
				int Result = Utils.StringReplaceToInt32(tmp);
				Logger.Log (LoggingCategories.Parse, "DarkMatter: " + Result.ToString ());
				return Result;
			}
		}
		
		/// <summary>
		/// ID (aus den metadaten) vom aktuellen planeten
		/// </summary>
		public int CurrentPlanetId {
			get {
				return Utils.StringReplaceToInt32(
					AccountCache.LastIndexPageParser.DocumentNode.SelectSingleNode(_stringManager.CurrentPlanetIdXPath).Attributes["content"].Value);
			}
		}
		
		/// <summary>
		/// Der aktuell angewählte Planet
		/// </summary>
		public Planet CurrentPlanet {
		 	get {
				int currentPlanetId  = CurrentPlanetId;
				foreach(Planet p in this.CachedPlanetList) {
					if(p.Id == currentPlanetId)	{
						return p;	
					}
				}
				
				return null;
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
		
		public void SetLastIndexPage(IndexPages page, HttpResult res) {
			if(res == null) {
				throw new ArgumentException("null", "res");	
			}
			
			
		}
		
		/// <summary>
		/// Navigiert zu einer Ogame-Standard Seite
		/// </summary>
		/// <param name="page">Ogame-Standard Seite</param>
		public HttpResult NagivateToIndexPage (IndexPages page)
		{
			Logger.Log (LoggingCategories.NavigationAction, "NagivateToIndexPage(" + _stringManager.IndexPageNames [page] + ")");
			HttpResult tmp = HttpHandler.Get(this._stringManager.GetIndexPageUrl (page));
			this.AccountCache.LastIndexPageResult = tmp;
			this.AccountCache.LastPageResult = tmp;
			this.AccountCache.LastIndexPagesResults[(int)page] = tmp;
			this.CurrentPlanet.Cache.LastPageResult = tmp;
			this.CurrentPlanet.Cache.LastIndexPagesResults[(int)page] =  tmp;
			this.CurrentPlanet.Cache.LastIndexPageResult =  tmp;
			return tmp;
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
			HttpResult tmp = HttpHandler.Post (_stringManager.LoginUrl, _stringManager.LoginParameter);
			
			this.AccountCache.LastIndexPageResult = tmp;
			this.AccountCache.LastPageResult = tmp;
			this.AccountCache.LastIndexPagesResults[(int)IndexPages.Overview] = tmp;
			this.CurrentPlanet.Cache.LastPageResult = tmp;
			this.CurrentPlanet.Cache.LastIndexPagesResults[(int)IndexPages.Overview] =  tmp;
			this.CurrentPlanet.Cache.LastIndexPageResult =  tmp;
			
			//Nach Logout Link suchen... falls vorhanden => login war erfolgreich, sonst nicht
			if(!IsLoggedIn(false))
				throw new LoginFailedException ("Login failed (LogoutRegex) not found");
			
			Logger.Log (LoggingCategories.NavigationAction, "Login was successfull");
		}

		/// <summary>
		/// Versucht ein Gebäude auf dem aktuell-angewählten Planeten auszubauen
		/// </summary>
		/// <param name="building">gebäude id</param>
		public void UpgradeBuilding (SupplyBuildings building)
		{
			Logger.Log (LoggingCategories.NavigationAction, "UpgradeBuilding(" + building.ToString () + ")");
			//Falls Token nicht gefunden wird zur entsprechenden Seite navigieren
			if (this.AccountCache.LastIndexPageResult.ResponseUrl.ToString () != _stringManager.GetIndexPageUrl (IndexPages.Resources)) {
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
			if (this.CurrentPlanet.Metal < neededMetal) {
				throw new NotEnoughMetalException ("Nicht genug Metall zum bau von " + building.ToString ());
			} else if (this.CurrentPlanet.Crystal < neededCrystal) {
				Mathemathics.CalcMaxTimeForRes (this.CurrentPlanet.Metal, this.CurrentPlanet.Crystal, this.CurrentPlanet.Deuterium, neededMetal, neededCrystal, neededDeuterium, this.CurrentPlanet.MetalPerHour, this.CurrentPlanet.CrystalPerHour, this.CurrentPlanet.DeuteriumPerHour);
				throw new NotEnoughCrystalException ("Nicht genug Kristall zum bau von " + building.ToString ());
			} else if (this.CurrentPlanet.Deuterium < neededDeuterium) {
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
			if (this.AccountCache.LastIndexPageResult.ResponseUrl.ToString () != _stringManager.GetIndexPageUrl (IndexPages.Station)) {
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
			if (this.CurrentPlanet.Metal < neededMetal) {
				throw new NotEnoughMetalException ("Nicht genug Metall zum bau von " + building.ToString ());
			} else if (this.CurrentPlanet.Crystal < neededCrystal) {
				Mathemathics.CalcMaxTimeForRes (this.CurrentPlanet.Metal, this.CurrentPlanet.Crystal, this.CurrentPlanet.Deuterium, neededMetal, neededCrystal, neededDeuterium, this.CurrentPlanet.MetalPerHour, this.CurrentPlanet.CrystalPerHour, this.CurrentPlanet.DeuteriumPerHour);
				throw new NotEnoughCrystalException ("Nicht genug Kristall zum bau von " + building.ToString ());
			} else if (this.CurrentPlanet.Deuterium < neededDeuterium) {
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
		
		public List<Planet> _cachedPlanetList = null;
			
		public List<Planet> CachedPlanetList {
			get {
				if( _cachedPlanetList == null)
					_cachedPlanetList = PlanetList;
				
				return _cachedPlanetList;
			}
		}
		
		/// <summary>
		/// Lese Planeten aus und füge Sie in planetList ein 
		/// </summary>
		public List<Planet> PlanetList {
			get {
				List<Planet> ret = new List<Planet> ();
				HtmlAgilityPack.HtmlNodeCollection planetNodes = this.AccountCache.LastIndexPageParser.DocumentNode.SelectNodes (_stringManager.PlanetListXPath);

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
		public void ChangeToPlanet (IndexPages page, Planet planet)
		{
			HttpResult tmp = this.HttpHandler.Get(_stringManager.GetIndexPageUrl(page) + "&cp=" + planet.Id.ToString());
			this.AccountCache.LastPageResult = tmp;
			this.AccountCache.LastIndexPageResult = tmp;
			this.AccountCache.LastIndexPagesResults[(int)page] = tmp;
			this.CurrentPlanet.Cache.LastPageResult =  tmp;
			this.CurrentPlanet.Cache.LastIndexPagesResults[(int)page] =  tmp;
			this.CurrentPlanet.Cache.LastIndexPageResult =  tmp;
		}

        #endregion
		
		#region Serialization
		
		/// <summary>
		/// Speichert die Session
		/// </summary>
		/// <param name='path'>
		/// Dateipfad
		/// </param>
		public void Serialize(string path) {
			FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
			DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();
			string hwID = Utils.Truncate(Utils.HardwareID, 8);
			cryptic.Key = ASCIIEncoding.ASCII.GetBytes(hwID);
			cryptic.IV = ASCIIEncoding.ASCII.GetBytes(hwID);

			CryptoStream crStream = new CryptoStream(stream,
   				cryptic.CreateEncryptor(), CryptoStreamMode.Write);

			BinaryFormatter formatter =  new BinaryFormatter();
			formatter.Serialize(crStream, this);

			crStream.Close();
			stream.Close();
		}
		
		/// <summary>
		/// Lädt eine Session aus einer Datei
		/// </summary>
		public static FrohgameSession Deserialize(string path) {
			FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

			DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();
			string hwID = Utils.Truncate(Utils.HardwareID, 8);
			cryptic.Key = ASCIIEncoding.ASCII.GetBytes(hwID);
			cryptic.IV = ASCIIEncoding.ASCII.GetBytes(hwID);
			
			CryptoStream crStream = new CryptoStream(stream,
			    cryptic.CreateDecryptor(), CryptoStreamMode.Read);
			
			BinaryFormatter formatter =  new BinaryFormatter();
			object obj = formatter.Deserialize(crStream);
			
			stream.Close();
			
			return (FrohgameSession)obj;
		}
		
		#endregion
	}
}