using System;
using System.Collections.Generic;

namespace Frohgame
{
	public class FactorOffset {
		public double Metal;
		public double Crystal;
		public double Deuterium;
	}
	
	/// <summary>
	/// Rechnet krams
	/// </summary>
	public class Calculator
	{
		Dictionary<int, FactorOffset> Factors = new Dictionary<int, FactorOffset>();
		Dictionary<int, FactorOffset> Offsets = new Dictionary<int, FactorOffset>();
		
		public Calculator ()
		{
			//TODO: echte faktoren einfügen:
			Factors.Add(
				(int)FROHGAME.Core.StationBuildings.TerraFormer, 
				new FactorOffset() {
					Metal = 1234, //mit Metal faktor ändern
					Crystal = 1234, //mit Crystal faktor ändern
					Deuterium = 1235, //mit Deuterium faktor ändern
				}
			);
			
			Factors.Add(
				(int)FROHGAME.Core.Military.Battlecruiser, 
				new FactorOffset() {
					Metal = 1234, //mit Metal faktor ändern
					Crystal = 1234, //mit Crystal faktor ändern
					Deuterium = 1235, //mit Deuterium faktor ändern
				}
			);
			
			//TODO: echte offsets einfügen:
			Offsets.Add(
				(int)FROHGAME.Core.Military.Bomber,
				new FactorOffset() {
					Metal = 50,
					Crystal = 50,
					Deuterium = 50,
				}
			);
		}
		
		/// <summary>
		/// Kalkuliert die kosten für ein 'Ogame' Element (gebäude oder schiff, etc)
		/// WICHTIG: nicht per 	CalculateCosts(FROHGAME.Core.Military.Battlecruiser, ...) aufrufen
		/// 		 sondern 	CalculateCosts((int)FROHGAME.Core.Military.Battlecruiser, ...)
		/// </summary>
		/// <returns>
		/// die costen
		/// </returns>
		/// <param name='element'>
		/// 'Ogame' Element (gebäude oder schiff, etc).
		/// </param>
		/// <param name='level'>
		/// Ziellevel.
		/// </param>
		public int CalculateCosts(int element, int level) {
			FactorOffset factor = Factors[element];
			
			//der startwert^^:
			FactorOffset offset = Offsets[element]; 
			//TODO: berechnungen durchführen
			return 12;
		}
	}
}

