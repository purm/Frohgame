using System;
using System.Collections.Generic;

namespace Frohgame
{
	public class ObjectOffset
	{
		public string Name;
		public double Factor;
		public int Metal;
		public int Crystal;
		public int Deuterium;
		public int Energy;
		public string Art;
	}
	
	public class Calculator
	{
		Dictionary<int, ObjectOffset> Type = new Dictionary<int, ObjectOffset> ();
		
		public Calculator ()
		{
			//Todo weitere Daten hinzufügen
			Type.Add (
				(int)FROHGAME.Core.SupplyBuildings.Metalmine, 
				new ObjectOffset () {
				    Name = "Metalmine",
				    Factor = 1.5,
					Metal = 60, 
					Crystal = 15, 
				    Deuterium = 0,
				    Energy = 0,
				    Art = "Building",
				}
			);
		}
		
		public double CalculateCosts (int element, int NextLevel)
		{
			ObjectOffset type = Type [element];
			
			// Müssen vorher ausgelesen werden!
			int RoboFabrik = 0; 
			int NanitFabrik = 0;
			int UniSpeed = 1; // 1 Standard 3 = Speeduni 
			double Result; // Speicher immer das aktuelle resultat
			
			//Berechne die bentötigten Ressourcen Funktioniert!
			double Metal = Math.Floor (type.Metal * Math.Pow (type.Factor, NextLevel - 1));
			double Crystal = Math.Floor (type.Crystal * Math.Pow (type.Factor, NextLevel - 1));
			double Deuterium = Math.Floor (type.Deuterium * Math.Pow (type.Factor, NextLevel - 1));
			double Energy = Math.Floor (type.Energy * Math.Pow (type.Factor, NextLevel - 1));
			
			Console.WriteLine ("Benötigte Ressourcen für: " + type.Name + " Metal: " + Metal + " Kristall: " + Crystal + " auf Stufe: " + NextLevel);
			
			
			switch (type.Art) {
				
			case "Building":
				Result = ((Metal + Crystal) * 3600) / (2500 * (RoboFabrik - 1 * -1) * Math.Pow (2, NanitFabrik));
				Result = Result / UniSpeed;
				CalculateTime (Result);
				break;
				
			case "Station":
				
				break;
					
			case "Research":
				
				break;
			case "Fleet": 
				
				break;
			default:
				
				break;	
			}
			
			return  0;
		}
		
		public TimeSpan CalculateTime (double i)
		{
			TimeSpan span = TimeSpan.FromSeconds(i);
			
			//TODO: folgende linie entfernen
			Console.WriteLine(span.Days + " Tage " + span.Hours + ":" + span.Minutes + ":" + span.Seconds);
			
			return span;				
		}
	}
} 

