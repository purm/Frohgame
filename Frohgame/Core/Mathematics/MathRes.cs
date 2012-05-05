using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frohgame.Core
{
	static public class Mathemathics
	{
		#region Berechnung für die Zeit aus bis genug Res da sind
		/// <summary>
		/// Berechnet die Zeit die benötigt werden um genug Ressourcen zu prduzieren
		/// </summary>
		/// <param name="HTML">html von den bau details</param>
		/// <returns>Tage Stunden Minuten </returns>

		static public TimeSpan CalcMaxTimeForRes (int currentMetal, int currentCrystal, int currentDeuterium,
                                        int neededMetal, int neededCrystal, int neededDeuterium,
                                        int metalPerHour, int crystalPerHour, int deuteriumPerHour)
		{
            
			TimeSpan MaxTime = new TimeSpan (0);

			if (CalcTimeForRes (neededMetal, currentMetal, metalPerHour) > MaxTime) {
				MaxTime = CalcTimeForRes (neededMetal, currentCrystal, metalPerHour) + MaxTime;
			}

			if (CalcTimeForRes (neededCrystal, currentCrystal, crystalPerHour) > MaxTime) {
				MaxTime = CalcTimeForRes (neededCrystal, currentCrystal, crystalPerHour) + MaxTime;
			}

			if (CalcTimeForRes (neededDeuterium, currentDeuterium, deuteriumPerHour) > MaxTime) {
				MaxTime = CalcTimeForRes (neededDeuterium, currentDeuterium, deuteriumPerHour) + MaxTime;
			}

			//ToDO: MaxTime zusammen rechnen und in Sekunden umwandeln 

			Console.WriteLine ("CalcMaxTimeForRes: " + MaxTime.Days + " Tage " + MaxTime.Hours + " Stunden " + MaxTime.Minutes + " Minuten ");
			return MaxTime;
		}

		/// <summary>
		/// Berechnet die Einzelnen Ressourcen
		/// </summary>
		/// <param name="NeededRes">Die Res die man braucht</param>
		/// <param name="CurrentRes">Res die man besitzt</param>
		/// <param name="ResPerHour">Res die man macht</param>
		/// <returns>Time oder 0</returns>
		static public TimeSpan CalcTimeForRes (int NeededRes, int CurrentRes, int ResPerHour)
		{
			if (ResPerHour == 0)
				return new TimeSpan (0);

			double timeInHours = (double)((double)NeededRes - (double)CurrentRes) / (double)ResPerHour;
			return TimeSpan.FromHours (timeInHours);
		}
		
		#endregion
		
		#region Berchnet die Benötigen Ressourcen zum Bau von Sachen
		[Serializable()]
	public class ObjectOffset
		{
			public double Factor;
			public int Metal;
			public int Crystal;
			public int Deuterium;
			public int Energy;
			public Frohgame.Core.ElementTypes Type;
		}
	
		[Serializable()]
	public class CalculatorResult
		{
			public int Metal;
			public int Crystal;
			public int Deuterium;
			public int Energy;
			public TimeSpan Duration;
		}
	
		[Serializable()]
	public class Calculator
		{
			Dictionary<int, ObjectOffset> Offsets = new Dictionary<int, ObjectOffset> ();
		
			public Calculator ()
			{
				//Todo weitere Daten hinzufügen
				Offsets.Add (
				(int)Frohgame.Core.SupplyBuildings.Metalmine, 
				new ObjectOffset () {
				    Factor = 1.5,
					Metal = 60, 
					Crystal = 15, 
				    Deuterium = 0,
				    Energy = 0,
					Type = Frohgame.Core.ElementTypes.Building
				}
			);
			}
		
			public CalculatorResult CalculateNeeds (int element, int NextLevel)
			{
				ObjectOffset offset = Offsets [element];
			
				// TODO: folgendes auslesen & als klassenmember benutzen
				int RoboFabrik = 0; 
				int NanitFabrik = 0;
				bool Technocrat = false; //KP
				int UniSpeed = 1; // 1 Standard 3 = Speeduni 
				double Result;
				int Werft = 0;
			
				//Berechne die bentötigten Ressourcen Funktioniert!
				double Metal = Math.Floor (offset.Metal * Math.Pow (offset.Factor, NextLevel - 1));
				double Crystal = Math.Floor (offset.Crystal * Math.Pow (offset.Factor, NextLevel - 1));
				double Deuterium = Math.Floor (offset.Deuterium * Math.Pow (offset.Factor, NextLevel - 1));
				double Energy = Math.Floor (offset.Energy * Math.Pow (offset.Factor, NextLevel - 1));
			
				//Console.WriteLine ("Benötigte Ressourcen für: " + type.Name + " Metal: " + Metal + " Kristall: " + Crystal + " auf Stufe: " + NextLevel);		
			
				CalculatorResult res = new CalculatorResult ();
			
				res.Crystal = (int)Crystal;
				res.Metal = (int)Metal;
				res.Deuterium = (int)Deuterium;
				res.Energy = (int)Energy;
			
				switch (offset.Type) {
				case Frohgame.Core.ElementTypes.Building: // Stations & Resourcen- Buildings
				
					Result = ((Metal + Crystal) * 3600) / (2500 * (RoboFabrik - 1 * -1) * Math.Pow (2, NanitFabrik));
					Result = Result / UniSpeed;
					res.Duration = TimeSpan.FromSeconds (Result);
					break;
						
				case Frohgame.Core.ElementTypes.Research: //Forschung 
				
					Result = ((Metal + Crystal) * 3600) / (1000 * (RoboFabrik - 1 * -1) * Math.Pow (2, NanitFabrik));
					if (Technocrat) {
						Result = Result * 0.75;
					}	
					Result = Result / UniSpeed;
					res.Duration = TimeSpan.FromSeconds (Result);
					break;
				
				case Frohgame.Core.ElementTypes.Ship: //Schiffe 
				
					Result = Math.Floor ((Metal + Crystal) * 3600) / (2500 * (Werft - 1 * -1) * Math.Pow (2, NanitFabrik)); 
					Result = Result * NextLevel; //Nextlevel ist auch die Anzahl der Elemente falls es ein Flug oder Def Object ist
					Result = Result / UniSpeed;
					res.Duration = TimeSpan.FromSeconds (Result);
					break;	
				
				default:
					throw new ArgumentException ("ungültiger parameter", "element");
				}

				return res;
		
			}
		}
	#endregion
	}
}
