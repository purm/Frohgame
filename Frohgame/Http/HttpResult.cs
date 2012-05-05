using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

/*
 * 
 * Author(s): Purm
 * 
 */

namespace Frohgame.Http
{
	[Serializable()]
	public class HttpResult
	{
        #region Properties

		DateTime _navigationFinished;
		/// <summary>
		/// Zeitpunkt, an dem die Navigation abgeschlossen ist
		/// </summary>
		public DateTime NavigationFinished {
			get { return _navigationFinished; }
			set { _navigationFinished = value; }
		}

		DateTime _navigationStarted;
		/// <summary>
		/// Zeitpunkt, an dem die Navigation startet
		/// </summary>
		public DateTime NavigationStarted {
			get { return _navigationStarted; }
			set { _navigationStarted = value; }
		}

		WebHeaderCollection _requestHeader;
		/// <summary>
		/// Anfrage Header vom Client an den Server gesendet
		/// </summary>
		public WebHeaderCollection RequestHeader {
			get { return _requestHeader; }
			set { _requestHeader = value; }
		}

		WebHeaderCollection _responseHeader;
		/// <summary>
		/// Antwort Header vom Server an den Client
		/// </summary>
		public WebHeaderCollection ResponseHeader {
			get { return _responseHeader; }
			set { _responseHeader = value; }
		}

		string _responseContent;
		/// <summary>
		/// (normalerweise (X)Html-) Inhaltsantwort vom Server
		/// </summary>
		public string ResponseContent {
			get { return _responseContent; }
			set { _responseContent = value; }
		}

		Uri _responseUrl;
		/// <summary>
		/// Url die bei der letzten Anfrage benutzt wurde (nach umleitungen etc)
		/// </summary>
		public Uri ResponseUrl {
			get { return _responseUrl; }
			set { _responseUrl = value; }
		}

		Uri _requestUrl;
		/// <summary>
		/// Url, die bei der Anfrage benutzt wurde
		/// </summary>
		public Uri RequestUrl {
			get { return _requestUrl; }
			set { _requestUrl = value; }
		}

        #endregion

        #region Public Methods

		/// <summary>
		/// Wandelt ein HttpResult in einen String um
		/// </summary>
		/// <returns>(normalerweise (X)Html-) Inhaltsantwort vom Server</returns>
		public override string ToString ()
		{
			return ResponseContent;
		}

        #endregion
	}
}
