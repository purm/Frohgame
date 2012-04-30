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
			
			FROHGAME.Core.FrohgameSession session = null;
			
			if(File.Exists("session.dat")) {
				Console.WriteLine("Session von Datei laden? (Yes/No)");
				if(Console.ReadLine().ToUpper() == "YES") {
					session = FROHGAME.Core.FrohgameSession.Deserialize("session.dat");
				}
			}
			
			if(session == null) {
				session = new FROHGAME.Core.FrohgameSession (accNode.Attributes ["name"].Value, accNode.Attributes ["password"].Value, accNode.Attributes ["server"].Value);	
			}
			else {
				Console.WriteLine("Session erfolgreich deserialisiert");
			}
			
			//session.HttpHandler.Proxy = "127.0.0.1:8888";
			session.Logger.OnStringLogged += new FROHGAME.Logger.OnLoggedStringDelegate (Logger_OnStringLogged);
			session.HttpHandler.OnNavigating += new FROHGAME.Http.HttpHandler.OnNavigatingDelegate (HttpHandler_OnNavigating);
			session.HttpHandler.OnNavigated += new FROHGAME.Http.HttpHandler.OnNavigatedDelegate (HttpHandler_OnNavigated);	
			
			//session.Calculator.CalculateNeeds((int)FROHGAME.Core.SupplyBuildings.Metalmine,12);
			
			//macht natürlich erst sinn, wenn Loggedin fertig implementiert ist!
			if(!session.IsLoggedIn(true)) {
				session.Login();
			}
			
			Console.WriteLine("VERSION: " + session.Version);
			
			string str = (
                "METALL: " + session.Metal + " - " + session.MetalPerHour + "/h" +
                " - KRISTALL: " + session.Crystal + " - " + session.CrystalPerHour + "/h" +
                " - DEUTERIUM: " + session.Deuterium + " - " + session.DeuteriumPerHour + "/h" + 
                " - DUNKLE MATERIE: " + session.DarkMatter + 
                " - ENERGIE: " + session.Energy);

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine (str);

			Console.WriteLine ("PLANETEN:");
			foreach (FROHGAME.Core.Planet p in session.PlanetList) {
				Console.WriteLine ("PLANET: " + p.Name + " - " + p.Id + " - [" + p.Coords.Galaxy.ToString () + ":" + p.Coords.SunSystem.ToString () + ":" + p.Coords.Place.ToString () + "]");
			}

			//Zeit bis 100.000 Metall:
			string time = FROHGAME.Core.Mathemathics.CalcMaxTimeForRes (
                session.Metal,
                session.Crystal,
                session.Deuterium,
                2000, 0, 0,
                session.MetalPerHour,
                session.CrystalPerHour,
                session.DeuteriumPerHour).ToString ();

			//int time = FROHGAME.Core.Mathemathics.CalcTimeForRes(100000, session.Metal, session.MetalPerHour);

			//test

			Console.WriteLine ("TIME: " + time);
        
			session.NagivateToIndexPage(FROHGAME.Core.IndexPages.Resources);
			System.Collections.Generic.Dictionary<FROHGAME.Core.SupplyBuildings, int> levels = session.SupplyBuildingLevels;
			
			Console.WriteLine("Metallmine level: " + levels[FROHGAME.Core.SupplyBuildings.Metalmine]);
			
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

		static void HttpHandler_OnNavigated (FROHGAME.Http.HttpResult res)
		{
			string toLog = String.Format ("[DEBUG] [NAVIGATED] [{0}]: {1}", DateTime.Now.ToString (), res.ResponseUrl.ToString ());
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine (toLog);
			System.IO.File.AppendAllText ("log.txt", toLog + Environment.NewLine);

			int sleep = FROHGAME.Utils.Random (2000, 5000);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine ("Schlafen: " + sleep.ToString () + "ms");
			Thread.Sleep (sleep);
		}

		static void Logger_OnStringLogged (FROHGAME.Core.LoggingCategories category, string log)
		{
			string toLog = String.Format ("[DEBUG] [{0}]: {1}", DateTime.Now.ToString (), log);

			switch (category) {
			case FROHGAME.Core.LoggingCategories.Combat:
				Console.ForegroundColor = ConsoleColor.Magenta;
				break;
			case FROHGAME.Core.LoggingCategories.NavigationAction:
				Console.ForegroundColor = ConsoleColor.Gray;
				break;
			case FROHGAME.Core.LoggingCategories.Parse:
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
