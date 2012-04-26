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
	}
	
	/// <summary>
	/// Rechnet krams
	/// </summary>
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
				}
			);
		}
			
		public int CalculateCosts (int element, int NextLevel)
		{
			ObjectOffset type = Type [element];
			
			// Müssen vorher ausgelesen werden!
			int RoboFabrik = 0; 
			int NanitFabrik = 0;
			int UniSpeed = 1;
			
	        //Berechne die bentötigten Ressourcen Funktioniert!
			double Metal = Math.Floor (type.Metal * Math.Pow (type.Factor, NextLevel - 1));
			double Crystal = Math.Floor (type.Crystal * Math.Pow (type.Factor, NextLevel - 1));
			double Deuterium = Math.Floor (type.Deuterium * Math.Pow (type.Factor, NextLevel - 1));
			double Energy = Math.Floor (type.Energy * Math.Pow (type.Factor, NextLevel - 1));
			
			// Das klappt schonmal 
			Console.WriteLine ("Benötigte Ressourcen für: " + type.Name + " Metal: " + Metal + " Kristall: " + Crystal + " auf Stufe: " + NextLevel );
			
			//Einfach SupplyBuilding Berechnung //TODO Irgendwas ist falsch
			double Formel1 = (type.Metal + type.Crystal * 3600) / (2500 * RoboFabrik - 1 * -1) * Math.Pow (2, NanitFabrik);
			if(UniSpeed > 1) Formel1 = Formel1 / UniSpeed; //falls uni Speed angeben wurde
			
			Formel1 = Math.Max (1, Math.Floor (Formel1));
			double sec = Mod (Formel1, 60);
			
			Formel1 = Div (Formel1, 60);
			
			double min = Mod (Formel1, 60);
			
			Formel1 = Div (Formel1, 60);
			
			double h = Mod (Formel1, 24);
			double d = Div (Formel1, 24);
			
			if (sec < 10) {
				sec = '0' + sec;
			}
			if (min < 10) {
				min = '0' + min;
			}
			if (h < 10) {
				h = '0' + h;
			}
			if (d == 0) {
				Console.WriteLine (h + ':' + min + ':' + sec);
				//  return  h + ':' + min + ':' + sec;
				
				return 0;
			} else {
				Console.WriteLine (d + "T " + h + ":" + min + ":" + sec);
				return 0;
				//  return d+"T "+h+":"+min+":"+sec;
			}
		}
		
		private double Div (double a, double b)
		{
			return Math.Floor (a / b);
		}
		
		private double Mod (double a, double b)
		{
			return a - Math.Floor (a / b) * b;
			
		
		}
	}
} 

