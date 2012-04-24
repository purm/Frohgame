using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FROHGAME.Core {
    static public class Mathemathics {
        /// <summary>
        /// Berechnet die Zeit die benötigt werden um genug Ressourcen zu prduzieren
        /// </summary>
        /// <param name="HTML">html von den bau details</param>
        /// <returns>Tage Stunden Minuten </returns>

        static public TimeSpan CalcMaxTimeForRes(int currentMetal, int currentCrystal, int currentDeuterium,
                                        int neededMetal, int neededCrystal, int neededDeuterium,
                                        int metalPerHour, int crystalPerHour, int deuteriumPerHour)  {
            
            TimeSpan MaxTime = new TimeSpan(0);

            if (CalcTimeForRes(neededMetal, currentMetal, metalPerHour) > MaxTime) {
                MaxTime = CalcTimeForRes(neededMetal, currentCrystal, metalPerHour) + MaxTime;
            }

            if (CalcTimeForRes(neededCrystal, currentCrystal, crystalPerHour) > MaxTime) {
                MaxTime = CalcTimeForRes(neededCrystal, currentCrystal, crystalPerHour) + MaxTime;
            }

            if (CalcTimeForRes(neededDeuterium, currentDeuterium, deuteriumPerHour) > MaxTime) {
                MaxTime = CalcTimeForRes(neededDeuterium, currentDeuterium, deuteriumPerHour) + MaxTime;
            }

            //ToDO: MaxTime zusammen rechnen und in Sekunden umwandeln 

            Console.WriteLine("CalcMaxTimeForRes: " + MaxTime.Days + " Tage " + MaxTime.Hours + " Stunden " + MaxTime.Minutes + " Minuten ");
            return MaxTime;
        }

        /// <summary>
        /// Berechnet die Einzelnen Ressourcen
        /// </summary>
        /// <param name="NeededRes">Die Res die man braucht</param>
        /// <param name="CurrentRes">Res die man besitzt</param>
        /// <param name="ResPerHour">Res die man macht</param>
        /// <returns>Time oder 0</returns>
        static public TimeSpan CalcTimeForRes(int NeededRes, int CurrentRes, int ResPerHour) {
            if (ResPerHour == 0)
                return new TimeSpan(0);

            double timeInHours = (double)((double)NeededRes - (double)CurrentRes) / (double)ResPerHour;
            return TimeSpan.FromHours(timeInHours);
        }

    }
}
