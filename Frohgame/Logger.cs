using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

/*
 * 
 * Author(s): Purm
 * 
 */

namespace Frohgame
//git test
{
	/// <summary>
	/// Loggt interne Ereignisse
	/// </summary>
	//
	[Serializable()]
	public class Logger
	{
        #region Delegates

		public delegate void OnLoggedStringDelegate (Frohgame.Core.LoggingCategories category,string log);

        #endregion

        #region Events

		/// <summary>
		/// Wird ausgelöst sobald etwas (intern) geloggt wird
		/// </summary>
		//
		[field: NonSerialized]
		public event OnLoggedStringDelegate OnStringLogged;

        #endregion            

        #region Internal Methods

		internal void Log (Frohgame.Core.LoggingCategories category, string log)
		{
			if (OnStringLogged != null)
				OnStringLogged (category, log);
		}

        #endregion
	}
}
