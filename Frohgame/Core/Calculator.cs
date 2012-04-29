using System;
using System.Collections.Generic;

namespace FROHGAME
{
	public class ObjectOffset
	{
		public double Factor;
		public int Metal;
		public int Crystal;
		public int Deuterium;
		public int Energy;
		public FROHGAME.Core.ElementTypes Type;
	}
	
	public class CalculatorResult {
		public int Metal;
		public int Crystal;
		public int Deuterium;
		public int Energy;
		public TimeSpan Duration;
	}
	
	public class Calculator
	{
		Dictionary<int, ObjectOffset> Offsets = new Dictionary<int, ObjectOffset> ();
		
		public Calculator ()
		{
			//Todo weitere Daten hinzufügen
			Offsets.Add (
				(int)FROHGAME.Core.SupplyBuildings.Metalmine, 
				new ObjectOffset () {
				    Factor = 1.5,
					Metal = 60, 
					Crystal = 15, 
				    Deuterium = 0,
				    Energy = 0,
					Type = FROHGAME.Core.ElementTypes.Building
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
			
			CalculatorResult res = new CalculatorResult();
			
			res.Crystal = Crystal;
			res.Metal = Metal;
			res.Deuterium = Deuterium;
			res.Energy = Energy;
			
			switch (offset.Type) {
			case FROHGAME.Core.ElementTypes.Building: // Stations & Resourcen- Buildings
				
				Result = ((Metal + Crystal) * 3600) / (2500 * (RoboFabrik - 1 * -1) * Math.Pow (2, NanitFabrik));
				Result = Result / UniSpeed;
				res.Duration = TimeSpan.FromSeconds(Result);
				break;
						
			case FROHGAME.Core.ElementTypes.Research: //Forschung 
				
				Result = ((Metal + Crystal) * 3600) / (1000 * (RoboFabrik - 1 * -1) * Math.Pow (2, NanitFabrik));
				if (Technocrat) {
					Result = Result * 0.75;
				}	
				Result = Result / UniSpeed;
				res.Duration = TimeSpan.FromSeconds(Result);
				break;
				
			case FROHGAME.Core.ElementTypes.Ship: //Schiffe 
				
				Result = Math.Floor ((Metal + Crystal) * 3600) / (2500 * (Werft - 1 * -1) * Math.Pow (2, NanitFabrik)); 
				Result = Result * NextLevel; //Nextlevel ist auch die Anzahl der Elemente falls es ein Flug oder Def Object ist
				Result = Result / UniSpeed;
				res.Duration = TimeSpan.FromSeconds(Result);
				break;	
				
			default:
				throw new ArgumentException("ungültiger parameter", "element");
			}

			return res;
		
		}
	}
} 

