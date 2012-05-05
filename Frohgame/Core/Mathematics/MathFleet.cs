using System;
using System.Collections.Generic;

//Berechnet die Flugdauer, die Lagerkapzität sowie den Verbaucht
namespace Frohgame
{
	
//todo : duration-time,arrival,return-time,fix,return-flight
	
	public class MathFleet
	{
		
		static private Dictionary<int, ObjectOffset> Offsets = new Dictionary<int, ObjectOffset> ();
		
		private class ObjectOffset
		{
			public int Antrieb;
			public int Multi;
			public int Speed1;
			public int Speed2;
			public int Verbrauch;
			public int Platz;
			public int Count;
		}
		/// <summary>
		/// Diese Daten müssen alle vorher Gesetzt werden!
		/// </summary> 
		/// 	

		static private SetDatas read = new SetDatas ();
		public class SetDatas
		{ 
			
			public int Verbrennungstriebwerk = 6;
			public int Impuls = 0;
			public int Hyper = 0;
			public bool isSpeedFactor = false;
			public int Speed = 100;
			//Galaxywerte 
			public int StartGalaxy = 7;
			public int StartSystem = 22;
			public int StartPlanet = 12;
			public int TargetGalaxy = 8;
			public int TargetSystem = 22;
			public int TargetPlanet = 12;
			//Schiff Counter da wird bisschen Komplizierter als ich dachte
			public int LightFighterCount = 4;
		}

		[Serializable()]
		private class FleetData
		{
			
		
			private FleetData ()
			{
				//Todo weitere Daten hinzufügen
				Offsets.Add (
				(int)Frohgame.Core.Military.LightFighter,
				new ObjectOffset () {
				    Antrieb = 115,
					Multi = 10,
					Speed1 = 12500,
					Speed2 = 0,
					Verbrauch = 20,
					Platz = 50,
					Count = read.LightFighterCount,
				}
			);	
				
			}
		}
		
		private int Distance ()
		{	
			
			int dist = 0;
			if ((read.TargetGalaxy - read.StartGalaxy) != 0) {
				dist = Math.Abs (read.TargetGalaxy - read.StartGalaxy) * 20000;
			} else if ((read.TargetSystem - read.StartSystem) != 0) {
				dist = Math.Abs (read.TargetSystem - read.StartSystem) * 5 * 19 + 2700; 
			} else if ((read.TargetPlanet - read.StartPlanet) != 0) {
				dist = Math.Abs (read.TargetPlanet - read.StartPlanet) * 5 + 1000; 
			} else { 
				dist = 5;		
			}
			return dist;
			
		}
		
		private double Duration ()
		{
			int SpeedFactor = 1;	
			if (read.isSpeedFactor) {
				SpeedFactor = 2; 
			}
			int msp = MaxSpeed ();
			int dist = Distance ();	
			var ret = Math.Round (((35000 / read.Speed * Math.Sqrt (dist * 10 / msp) + 10) / SpeedFactor));
			return ret;
		}
		
		
		//Wird aufgerufen und gibt den Verbrauch zurück 
		public double Consumption ()
		{
			double Consumption = 0;
			int BasicConsumption = 0;
			//int Values;
			//int i;	
			
			int msp = MaxSpeed ();
			int sp = read.Speed;
			int dist = Distance ();
			var dur = Duration ();
			
			int SpeedFactor = 1;
			if (read.isSpeedFactor) { 
				SpeedFactor = 2;
			} 
		
			
			int ID;
			for (ID = 202; ID <= 215; ID++) {
				ObjectOffset offset = Offsets [ID];
				if (ID != 212) {
					if (offset.Count > 0) { 
						int shipspeed = GetSpeed (ID);
						double spd = 35000 / (dur * SpeedFactor - 10) * Math.Sqrt (dist * 10 / shipspeed);
						BasicConsumption = offset.Verbrauch * offset.Count;
						Consumption += BasicConsumption * dist / 35000 * ((spd / 10) + 1) * ((spd / 10) + 1);
					}	
				}
			}
			Consumption = Math.Round (Consumption) + 1;
			return Consumption;
		}
			
		//int dis = Distance();
			
		//Hier muss die ID der Flotte angeben Like: FROHGAME.Core.Military.LightFighter
		private int GetSpeed (int element)
		{ 
			ObjectOffset offset = Offsets [element];
			int Speed;
			int Antrieb = offset.Antrieb;
			int Multi = offset.Multi;
			int Speed1 = offset.Speed1;
			int Speed2 = offset.Speed2;
			
			if ((element == 202) && (offset.Antrieb >= 4)) {
				Multi = 20;
				int stufe = read.Verbrennungstriebwerk;
				Speed1 = Speed2;
				return Speed1;
			}
			if ((element == 211) && (offset.Antrieb >= 7)) {
				Multi = 30;
				int stufe = read.Impuls;
				Speed1 = Speed2;
				Speed = Speed1 + Speed1 * stufe * Multi / 100;
				return Speed;
			}
			return 0;
			
		}
		
		private int MaxSpeed ()
		{		
			int ID;
			int speed;
			int msp = 1000000000;
			for (ID = 202; ID <= 215; ID++) {
				ObjectOffset offset = Offsets [ID];
				if (ID != 212) {  
					if (offset.Count > 0) { 
						speed = GetSpeed (ID);
						msp = Math.Min (msp, speed);
					}
				}
			}
			return msp;
		}
		
		
		//Wird aufgerufen und gibt den Platz der Flotte zurück 
		public double Capacity ()
		{
			
			int ID;
			double capacity1 = 0;
			for (ID = 202; ID <= 215; ID++) {
				ObjectOffset offset = Offsets [ID];
				if (ID != 212) {
					capacity1 += offset.Count * offset.Platz;
				} 
				capacity1 -= Consumption () - 1;
			}
			return capacity1;
		
		}
		
	} 
}
