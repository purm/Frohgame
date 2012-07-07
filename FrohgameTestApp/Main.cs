using System;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace FrohgameTestApp
{
	class MainClass
	{			
		static Frohgame.Core.FrohgameSession session = null;
		public static void Main (string[] args)
		{
			//SCHROTT, nicht nachmachen :D
			//Ja NIEMALS
			
			string path = Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location) + @"\AccountSchrott.xml";
			if (!File.Exists (path)) {
				string XmlText = (
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + 
                "<accounts>" + Environment.NewLine + 
                "<account name=\"UserName\" password=\"Password\" server=\"uni###.ogame.de\" />" + Environment.NewLine + 
                "</accounts>");   
				File.WriteAllText (path, XmlText);
				Console.WriteLine ("Bitte zuerst deine " + path + " bearbeiten! Drücke eine Taste zum fortfahren...");
				Console.ReadKey ();
			}
			
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument ();
			xmlDoc.Load ("AccountSchrott.xml");
			System.Xml.XmlNode accNode = xmlDoc.SelectSingleNode ("//account[@name]");
			
			
			
			bool loadedSessionFromFile = false;
			
			if(File.Exists("session.dat")) {
				Console.WriteLine("Session von Datei laden? (Yes/No)");
				if(Console.ReadLine().ToUpper() == "YES") {
					session = Frohgame.Core.FrohgameSession.Deserialize("session.dat", "a&md/1jA", "&mPmD8c!");
					loadedSessionFromFile = true;
				}
			}
			
			if(session == null) {
				session = new Frohgame.Core.FrohgameSession ();	
			}
			else {
				Console.WriteLine("Session erfolgreich deserialisiert");
			}
			
			//session.HttpHandler.Proxy = "127.0.0.1:8888";
			session.Logger.OnStringLogged += new Frohgame.Logger.OnLoggedStringDelegate (Logger_OnStringLogged);
			session.HttpHandler.OnNavigating += new Frohgame.Http.HttpHandler.OnNavigatingDelegate (HttpHandler_OnNavigating);
			session.HttpHandler.OnNavigated += new Frohgame.Http.HttpHandler.OnNavigatedDelegate (HttpHandler_OnNavigated);	
			
			//session.Calculator.CalculateNeeds((int)FROHGAME.Core.SupplyBuildings.Metalmine,12);
			
			if(loadedSessionFromFile == true) {
				if(!session.IsLoggedIn(true)) {
                    session.Login(accNode.Attributes["name"].Value, accNode.Attributes["password"].Value, accNode.Attributes["server"].Value);
				}
			} else {
                session.Login(accNode.Attributes["name"].Value, accNode.Attributes["password"].Value, accNode.Attributes["server"].Value);	
			}
			
			try {
				Console.WriteLine("VERSION: " + session.Version);
				
				string str = (
	                "METALL: " + session.CurrentPlanet.Metal + " - " + session.CurrentPlanet.MetalPerHour + "/h" +
	                " - KRISTALL: " + session.CurrentPlanet.Crystal + " - " + session.CurrentPlanet.CrystalPerHour + "/h" +
	                " - DEUTERIUM: " + session.CurrentPlanet.Deuterium + " - " + session.CurrentPlanet.DeuteriumPerHour + "/h" + 
	                " - DUNKLE MATERIE: " + session.DarkMatter + 
	                " - ENERGIE: " + session.CurrentPlanet.Energy);
	
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine (str);
	
				Console.WriteLine ("PLANETEN:");
				foreach (Frohgame.Core.Planet p in session.PlanetList) {
					Console.WriteLine ("PLANET: " + p.Name + " - " + p.Id + " - [" + p.Coords.Galaxy.ToString () + ":" + p.Coords.SunSystem.ToString () + ":" + p.Coords.Place.ToString () + "]");
				}
	
				//int time = FROHGAME.Core.Mathemathics.CalcTimeForRes(100000, session.Metal, session.MetalPerHour);
	
				//TEST:
//				foreach(Frohgame.Core.Planet p in session.PlanetList) {
//					if(session.CurrentPlanet != p) {
//						session.ChangeToPlanet(Frohgame.Core.IndexPages.Overview, p);
//					}
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Trader);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Alliance);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Changelog);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Defense);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Fleet1);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Galaxy);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Research);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Premium);
//					session.NagivateToIndexPage(Frohgame.Core.IndexPages.Shipyard);
//					
//				}
	
				Console.WriteLine("UNgelesene Nachrichten: " + session.UnreadMessagesCount);
	        
				System.Collections.Generic.Dictionary<Frohgame.Core.StationBuildings, int> levels; 
				
				try {
					levels = session.CurrentPlanet.StationBuildingLevels;	
				} catch(Frohgame.Core.NoCacheDataException ex) {
					Console.WriteLine("NoCacheDataException: " + ex.Message);
					session.NavigateToIndexPage(Frohgame.Core.IndexPages.Station);
					levels = session.CurrentPlanet.StationBuildingLevels;
				}
				
				Console.WriteLine("RobotorFactory level: " + levels[Frohgame.Core.StationBuildings.RobotorFactory]);
				
				System.Collections.Generic.Dictionary<Frohgame.Core.Researches, int> levels2; 
				
				try {
					levels2 = session.CurrentPlanet.ResearchLevels;	
				} catch(Frohgame.Core.NoCacheDataException ex) {
					Console.WriteLine("NoCacheDataException: " + ex.Message);
					session.NavigateToIndexPage(Frohgame.Core.IndexPages.Research);
					levels2 = session.CurrentPlanet.ResearchLevels;
				}
				
				Console.WriteLine("HyperRoomTech level: " + levels2[Frohgame.Core.Researches.HyperRoomTech]);
				
				} 
			catch(Frohgame.Core.InvalidSessionException ex) {
				Console.WriteLine("InvalidSessionException: " + ex.Message);

                //SCHROTT ENDE
			}
			
			
			Console.WriteLine("Ende Gelände");
			Console.ReadKey();
            session.Serialize("session.dat", "a&md/1jA", "&mPmD8c!");
		}

		static void HttpHandler_OnNavigating (string targetUrl, string post)
		{
			string toLog = String.Format ("[DEBUG] [NAVIGATING] [{0}]: {1}", DateTime.Now.ToString (), targetUrl);
			toLog = !string.IsNullOrEmpty (post) ? (String.Format ("{0} POST: {1}", toLog, post)) : toLog;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine (toLog);
			System.IO.File.AppendAllText ("log.txt", toLog + Environment.NewLine);
		}

		static void HttpHandler_OnNavigated (Frohgame.Http.HttpResult res)
		{
			string toLog = String.Format ("[DEBUG] [NAVIGATED] [{0}]: {1}", DateTime.Now.ToString (), res.ResponseUrl.ToString ());
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine (toLog);
			System.IO.File.AppendAllText ("log.txt", toLog + Environment.NewLine);

			int sleep = Frohgame.Utils.Random (1000, 3000);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine ("Schlafen: " + sleep.ToString () + "ms");
			Thread.Sleep (sleep);
		}

		static void Logger_OnStringLogged (Frohgame.Core.LoggingCategories category, string log)
		{
			string toLog = String.Format ("[DEBUG] [{0}]: {1}", DateTime.Now.ToString (), log);

			switch (category) {
			case Frohgame.Core.LoggingCategories.Combat:
				Console.ForegroundColor = ConsoleColor.Magenta;
				break;
			case Frohgame.Core.LoggingCategories.NavigationAction:
				Console.ForegroundColor = ConsoleColor.Gray;
				break;
			case Frohgame.Core.LoggingCategories.Parse:
				Console.ForegroundColor = ConsoleColor.Cyan;
				break;
			default:
				Console.ForegroundColor = ConsoleColor.White;
				break;
			}

			Console.WriteLine (toLog);
			System.IO.File.AppendAllText (category.ToString () + ".log.txt", toLog + Environment.NewLine);
		}


		/// <summary>
		/// Liest aus der Console
		/// </summary>
		static public void CommandReader ()
		{
			while (true) {
				string input = Console.ReadLine ();

				//prüfe auf "!" prefix
				if (input.Contains ("!")) {
					string[] command = input.Split (' ');

					switch (command [0]) {

					case "!login":
						Console.WriteLine ("test " + command [1]);
						break;
					case "!build":

						break;
					case "!cp":

						break;
					default:
						Console.WriteLine ("Command nicht gefunden");
						break;
					}
				}
			}
		}
	}
}
