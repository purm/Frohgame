using System;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace FrohgameTestApp
{
	class MainClass
	{				
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
			//SCHROTT ENDE
			
			Frohgame.Core.FrohgameSession session = null;
			bool loadedSessionFromFile = false;
			
			if(File.Exists("session.dat")) {
				Console.WriteLine("Session von Datei laden? (Yes/No)");
				if(Console.ReadLine().ToUpper() == "YES") {
					session = Frohgame.Core.FrohgameSession.Deserialize("session.dat");
					loadedSessionFromFile = true;
				}
			}
			
			if(session == null) {
				session = new Frohgame.Core.FrohgameSession (accNode.Attributes ["name"].Value, accNode.Attributes ["password"].Value, accNode.Attributes ["server"].Value);	
			}
			else {
				Console.WriteLine("Session erfolgreich deserialisiert");
			}
			
			//session.HttpHandler.Proxy = "127.0.0.1:8888";
			session.Logger.OnStringLogged += new Frohgame.Logger.OnLoggedStringDelegate (Logger_OnStringLogged);
			session.HttpHandler.OnNavigating += new Frohgame.Http.HttpHandler.OnNavigatingDelegate (HttpHandler_OnNavigating);
			session.HttpHandler.OnNavigated += new Frohgame.Http.HttpHandler.OnNavigatedDelegate (HttpHandler_OnNavigated);	
			
			//session.Calculator.CalculateNeeds((int)FROHGAME.Core.SupplyBuildings.Metalmine,12);
			
			//macht natürlich erst sinn, wenn Loggedin fertig implementiert ist!
			if(loadedSessionFromFile == true) {
				if(!session.IsLoggedIn(true)) {
					session.Login();
				}
			} else {
				session.Login();	
			}
			
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

			//Zeit bis 100.000 Metall:
			string time = Frohgame.Core.Mathemathics.CalcMaxTimeForRes (
                session.CurrentPlanet.Metal,
                session.CurrentPlanet.Crystal,
                session.CurrentPlanet.Deuterium,
                2000, 0, 0,
                session.CurrentPlanet.MetalPerHour,
                session.CurrentPlanet.CrystalPerHour,
                session.CurrentPlanet.DeuteriumPerHour).ToString ();

			//int time = FROHGAME.Core.Mathemathics.CalcTimeForRes(100000, session.Metal, session.MetalPerHour);

			//test

			Console.WriteLine ("TIME: " + time);
        
			session.NagivateToIndexPage(Frohgame.Core.IndexPages.Resources);
			System.Collections.Generic.Dictionary<Frohgame.Core.SupplyBuildings, int> levels = session.CurrentPlanet.SupplyBuildingLevels;
			
			Console.WriteLine("Metallmine level: " + levels[Frohgame.Core.SupplyBuildings.Metalmine]);
			
			Console.ReadKey();
			
			session.Serialize("session.dat");
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

			int sleep = Frohgame.Utils.Random (2000, 5000);
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
