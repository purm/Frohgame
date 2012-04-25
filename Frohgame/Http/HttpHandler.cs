using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;

/*
 * 
 * Author(s): Purm, cannap
 * 
 */

namespace FROHGAME.Http
{
	public class HttpHandler
	{
        #region Delegates

		public delegate void OnNavigatedDelegate (HttpResult res);

		public delegate void OnNavigatingDelegate (string targetUrl,string post);

        #endregion

        #region Events

		/// <summary>
		/// Wird ausgelöst sobald eine Navigation stattgefunden hat
		/// </summary>
		public event OnNavigatedDelegate OnNavigated;

		/// <summary>
		/// Wird ausgelöst sobald eine Navigation "beginnt"
		/// </summary>
		public event OnNavigatingDelegate OnNavigating;

        #endregion

        #region Properties

		CookieContainer _cookies = new CookieContainer ();
		/// <summary>
		/// Cookies der Session
		/// </summary>
		public CookieContainer Cookies {
			get { return _cookies; }
			set { _cookies = value; }
		}

		string _referer;
		/// <summary>
		/// Vorherige Seiten Url
		/// </summary>
		public string Referer {
			get { return _referer; }
			set { _referer = value; }
		}

		string _userAgent;
		/// <summary>
		/// UserAgent der benutzt werden soll
		/// </summary>
		public string UserAgent {
			get { return _userAgent; }
			set { _userAgent = value; }
		}

		string _proxy;
		/// <summary>
		/// Proxy
		/// </summary>
		public string Proxy {
			get { return _proxy; }
			set { _proxy = value; }
		}

        #endregion

        #region Contructors

		internal HttpHandler (string userAgent, string proxy)
		{
			this._proxy = proxy;
			this.UserAgent = userAgent;

			//Präventiert Exception auf manchen Systemen!
			System.Net.ServicePointManager.Expect100Continue = false;
		}

		internal HttpHandler (string userAgent)
            : this(userAgent, null)
		{

		}

        #endregion

        #region Internal Methods

		/// <summary>
		/// Sendet eine Http Post Anfrage an eine Url
		/// </summary>
		/// <param name="url">Http Url</param>
		/// <param name="post">Post String</param>
		/// <returns>Http Ergebnis (enthält Request Header, Response Header & Response Content)</returns>
		internal HttpResult Post (string url, string post)
		{
			if (string.IsNullOrEmpty (url))
				throw new ArgumentNullException ("url");

			if (string.IsNullOrEmpty (post))
				throw new ArgumentNullException ("post");

			return Request (url, post);
		}

		/// <summary>
		/// Sendet eine Http Get Anfrage an eine Url
		/// </summary>
		/// <param name="url">Http Url</param>
		/// <returns>Http Ergebnis (enthält Request Header, Response Header & Response Content)</returns>
		internal HttpResult Get (string url)
		{
			if (string.IsNullOrEmpty (url))
				throw new ArgumentNullException ("url");

			return Request (url, string.Empty);
		}

        #endregion

        #region Private Methods

		HttpResult Request (string url, string post)
		{
			if (OnNavigating != null)
				this.OnNavigating (url, post);
             
			//Request erstellen
			HttpWebRequest webRequest = WebRequest.Create (url) as HttpWebRequest;
			HttpWebResponse webResponse;
			HttpResult returnResult = new HttpResult ();

			returnResult.NavigationStarted = DateTime.Now;

			//Proxy auf null setzen fixed performance problem
			if (string.IsNullOrEmpty (this._proxy))
				webRequest.Proxy = null;
			else
				webRequest.Proxy = new WebProxy (this._proxy);

			//Cookies laden
			webRequest.CookieContainer = this.Cookies;

			//So gehts vermutlich schneller:
			webRequest.KeepAlive = true;

			//Automatische Weiterleitung aktivieren^^
			webRequest.AllowAutoRedirect = true;

			//Useragent setzen
			webRequest.UserAgent = this.UserAgent;

			//Vorherige Seite angeben
			webRequest.Referer = Referer;

			if (!string.IsNullOrEmpty (post)) {
				//Http Post Request erstellen
				webRequest.Method = "POST";
				webRequest.ContentType = "application/x-www-form-urlencoded";

				byte[] postData = Encoding.Default.GetBytes (post);

				webRequest.ContentLength = postData.Length;

				Stream dataStream = webRequest.GetRequestStream ();
				dataStream.Write (postData, 0, postData.Length);
			}
			webResponse = (HttpWebResponse)webRequest.GetResponse ();

			returnResult.RequestHeader = webRequest.Headers;
			returnResult.ResponseContent = System.Web.HttpUtility.HtmlDecode (new StreamReader (webResponse.GetResponseStream (), Encoding.UTF8, true).ReadToEnd ());
			returnResult.ResponseHeader = webResponse.Headers;

			//Cookies abspeichern
			this.Cookies.Add (webResponse.Cookies);

			//Seite abspeichern
			this.Referer = url;

			returnResult.RequestUrl = webRequest.RequestUri;
			returnResult.ResponseUrl = webResponse.ResponseUri;
			returnResult.NavigationFinished = DateTime.Now;

			//Event auslösen
			if (OnNavigated != null)
				this.OnNavigated (returnResult);

			return returnResult;
		}

        #endregion
	}
}
