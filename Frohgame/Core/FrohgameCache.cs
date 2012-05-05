using System;
using System.Collections.Generic;
using Frohgame.Http;
using Frohgame.Core;
using System.Collections.ObjectModel;

namespace Frohgame
{
	[Serializable()]
	public class FrohgameCache
	{	
		public FrohgameCache() {
			this.LastIndexPagesParsers = new ObservableCollection<HtmlAgilityPack.HtmlDocument>();
			this._lastIndexPagesResults = new ObservableCollection<HttpResult>();
			this.LastIndexPagesResults.CollectionChanged += HandleLastIndexPagesResultshandleCollectionChanged;
		}

		void HandleLastIndexPagesResultshandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		[NonSerialized()]
		HtmlAgilityPack.HtmlDocument _lastPageParser = new HtmlAgilityPack.HtmlDocument ();
		/// <summary>
		/// Hier wird jeweils die ZULETZT aufgerufene Seite gespeichert.. egal ob ajax, indexpage, etc um sie zu parsen
		/// </summary>
		public HtmlAgilityPack.HtmlDocument LastPageParser {
			get { 
				if(_lastPageParser == null) {
					_lastPageParser = new HtmlAgilityPack.HtmlDocument();
					if(LastPageResult != null) {
						if(LastPageResult.ResponseContent != null) {
							_lastPageParser.LoadHtml(LastPageResult.ResponseContent);
						}
					}
				}

				return _lastPageParser; 
			}
			private set { _lastPageParser = value; }
		}
		
		HttpResult _lastPageResult;
		/// <summary>
		/// Hier wird jeweils die ZULETZT aufgerufene Seite gespeichert.. egal ob ajax, indexpage, etc
		/// </summary>
		public HttpResult LastPageResult {
			get {
				return _lastPageResult;
			}
			set {
				this._lastPageResult = value;
				
				if(value != null) {
					if(value.ResponseContent != null) {
						LastPageParser.LoadHtml(value.ResponseContent);
					}
				}
			}
		}
		
		[NonSerialized()]
		HtmlAgilityPack.HtmlDocument _lastIndexPageParser = new HtmlAgilityPack.HtmlDocument ();
		/// <summary>
		/// Hier wird jeweils die ZULETZT aufgerufene "IndexPage" (siehe IndexPages Enum) gespeichert, zum parsen
		/// </summary>
		public HtmlAgilityPack.HtmlDocument LastIndexPageParser {
			get { 
				if(_lastIndexPageParser == null) {
					_lastIndexPageParser = new HtmlAgilityPack.HtmlDocument();
					if(LastIndexPageResult != null) {
						if(LastIndexPageResult.ResponseContent != null) {
							_lastIndexPageParser.LoadHtml(LastIndexPageResult.ResponseContent);
						}
					}
				}

				return _lastIndexPageParser; 
			}
			private set { _lastIndexPageParser = value; }
		}
			
		HttpResult _lastIndexPageResult;
		/// <summary>
		/// Hier wird jeweils die ZULETZT aufgerufene "IndexPage" (siehe IndexPages Enum) gespeichert
		/// </summary>
		public HttpResult LastIndexPageResult {
			get {
				return _lastIndexPageResult;
			}
			set {
				this._lastIndexPageResult = value;
				
				if(value != null) {
					if(value.ResponseContent != null) {
						LastIndexPageParser.LoadHtml(value.ResponseContent);
					}
				}
			}
		}
		
		ObservableCollection<HttpResult> _lastIndexPagesResults;
		public ObservableCollection<HttpResult> LastIndexPagesResults {
			get {
				return this._lastIndexPagesResults;
			}
			private set {
				_lastIndexPagesResults = value;
			}
		}
		
		[NonSerialized()]
		ObservableCollection<HtmlAgilityPack.HtmlDocument> _lastIndexPagesParsers;
		public ObservableCollection<HtmlAgilityPack.HtmlDocument> LastIndexPagesParsers {
			get {
				return this._lastIndexPagesParsers;
			}
			private set {
				_lastIndexPagesParsers = value;
			}
		}
	}
}

