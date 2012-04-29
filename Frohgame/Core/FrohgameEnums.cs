using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 
 * Author(s): Purm & cannap
 * Don't blame for stupid names!
 * 
 */

namespace FROHGAME.Core {
    #region Enums

    public enum SupplyBuildings {
        Metalmine = 1,
        Crystalmine = 2,
        DeuteriumSynthesizer = 3, // ODER WIE AUCH IMMER DIE SCHEISSE GESCHRIEBEN WERDEN SOLL GRR
        SolarPowerPlant = 4,
        MetalBox = 22,
        CrystalBox = 23,
        DeuteriumBox = 24,
        FusionPowerStation = 12,
        HiddenMetalBox = 25,
        HiddenCrystalBox = 26,
        HiddenDeuteriumBox = 27
    }

    public enum StationBuildings {
        RobotorFactory = 14,
        SpaceShipYard = 21,
        ResearchLaboratory = 31,
        AllianceDepository = 34,
        MissileSilo = 44,
        NaniFactory = 15,
        TerraFormer = 33
    }
	
	public enum Military { 
		LightFighter = 204,
		HeavyFighter = 205,
		Cruieser = 206,
		Battleship = 207,
		Battlecruiser = 215,
		Bomber = 211,
		Destroyer = 213,
		Deathstar = 214,
		SmallCargoShip = 202,
		LargeCargoShip = 203,
		ColonyShip = 208,
		Recycler = 209, 
		EspionageProbe = 210,
		SolarSatellite = 212	
	}

    public enum IndexPages {
        Overview,
        Resources,
        Station,
        Trader,
        Research,
        Shipyard,
        Defense,
        Fleet1,
        Galaxy,
        Alliance,
        Premium,
        Changelog
    }

    public enum LoggingCategories {
        Parse,
        NavigationAction,
        Combat
    }
	
	public enum ElementTypes {
		Building,
		Ship,
		Research
	}

    #endregion
}
