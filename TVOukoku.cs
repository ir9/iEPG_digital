using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using Microsoft.VisualBasic;

namespace mAgicTvPlugIn
{
	public class TVOukokuException : Exception
	{
		public TVOukokuException(string msg) : base(msg){}
	};

	public class TVOukokuRecord
	{
		public string   title;
		public DateTime startTime;
		public string   tvStation;
		public string   iepgURL;

		public override string ToString() {
			return String.Format("title:{0} / startTime:{1} / tvStation:{2} / iepgURL:{3}", title, startTime, tvStation, iepgURL);
		}
	};
	
	public class TVOukoku
	{
		static readonly string BASE_URL      = "http://tv.so-net.ne.jp";
		static readonly string SEARCH_SUFFIX = "/schedulesBySearch.action?condition.keyword={0}&stationPlatformId={1}&condition.genres[0].parentId={2}&condition.genres[0].childId=-1&submit=%8C%9F%8D%F5&iepgType=0";
		static readonly string USERAGENT     = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.80 Safari/537.36";

		private readonly Encoding UTF_8     = Encoding.UTF8;
		private readonly Encoding SHIFT_JIS = Encoding.GetEncoding("shift_jis");

		public TVOukoku() {

		}

		public string SearchEntry(ReservationModel reservModel)
		{
			_D("enter SearchEntry() : {0}", reservModel.ToString());

			string               searchRequestURL = createSearchRequestURL(reservModel);							_D("searchRequestURL = {0}", searchRequestURL);
			List<TVOukokuRecord> entryList        = GetEntryList(searchRequestURL, reservModel.outputReceiveHTML);	_D("entry count = {0}",      entryList.Count);

			string         tvStationWide = Strings.StrConv(reservModel.tvStation.ToUpper(), VbStrConv.Wide, 0); _D("entry count = {0}", entryList.Count);
			TVOukokuRecord matchRec      = GetMatchObject(entryList, reservModel.startDateTime, tvStationWide);
			if(matchRec == null)
				throw new TVOukokuException("エントリーが存在しませんでした : \n" + searchRequestURL + "\n");

			string iEPGFile = GetIEPGFile(matchRec);
			return iEPGFile;
		}

		/*--------------------------------------------------*
		 *	個々の番組情報を取得しちゃうよ関連
		 *--------------------------------------------------*/
		private List<TVOukokuRecord> GetEntryList(string url, bool outputHtmlFile)
		{
			List<TVOukokuRecord> ret = new List<TVOukokuRecord>();

//			WebRequest req = CreateWebRequest(url);

			CookieContainer cookieCont = new CookieContainer();
			HttpWebRequest  req        = (HttpWebRequest)WebRequest.Create(url);
			req.CookieContainer   = cookieCont;
			req.AllowAutoRedirect = false;

			using(HttpWebResponse res = (HttpWebResponse)req.GetResponse())
			{
				if((int)res.StatusCode != 308)
					return null;
				string location = res.Headers[HttpResponseHeader.Location];
				url = location;
			}
			req = (HttpWebRequest)WebRequest.Create(url);
			req.CookieContainer = cookieCont;

			do	// ホントはココで全ページ回るべき → まぁでも面倒だからいいよね…どうせ１ページだけだろうし…（ぉ
			{
				string str = ReadAllString(req, UTF_8);
				if(outputHtmlFile) OutputHtml(str);
				ret.AddRange(TVOukokuParser.Parse(str));
			} while (false);

			return ret;
		}


		private TVOukokuRecord GetMatchObject(List<TVOukokuRecord> list, DateTime startTime, String tvStationWideChar)
		{
			foreach(TVOukokuRecord rec in list)
			{
				if(rec.tvStation.Equals(tvStationWideChar, StringComparison.InvariantCultureIgnoreCase) && rec.startTime.CompareTo(startTime) == 0)
				{
					return rec;
				}
			}

			return null;
		}

		private string createSearchRequestURL(ReservationModel model)
		{
			string title_utf8 = HttpUtility.UrlEncode(model.searchText, UTF_8);
			string requestOrg = String.Format(SEARCH_SUFFIX, title_utf8, model.platformID, model.categoryID);

			string requestOrgEnced = HttpUtility.UrlEncode(requestOrg, UTF_8);

			// origin に redirect 先のURLをエンコードした物を代入しなきゃならん
			string cat = String.Format("/stationAreaSettingCompleted.action?origin={0}&stationAreaId={1}&stationPlatformId={2}&condition.genres[0].parentId={3}&condition.genres[0].childId=-1",
				requestOrgEnced, model.areaID, model.platformID, model.categoryID);
			return BASE_URL + cat;
		}

		
		/*-----------------------------------------------------*
		 *	iEPG関連
		 *-----------------------------------------------------*/
		private string CreateIEPGUrl(string url)
		{
			return BASE_URL + url;
		}

		private string GetIEPGFile(TVOukokuRecord rec)
		{
			string     url = CreateIEPGUrl(rec.iepgURL);		_D("iEPGFile url : {0}", url);
			WebRequest req = CreateWebRequest(url);

			string iEPGFile = ReadAllString(req, SHIFT_JIS);	_D("success to get a iEPGfile.");
			return iEPGFile;
		}


		/*-----------------------------------------------------*
		 *	共通
		 *-----------------------------------------------------*/
		private WebRequest CreateWebRequest(string url)
		{
			CookieContainer cookieCont = new CookieContainer();
			HttpWebRequest  req        = (HttpWebRequest)WebRequest.Create(url);;
			if(req == null)
				return req;
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0x00000C00 | 0x00000300);
			req.CookieContainer   = cookieCont;
			req.AllowAutoRedirect = false;
			req.UserAgent = USERAGENT;

			using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
			{
				if ((int)res.StatusCode != 308)
					return null;
				string location = res.Headers[HttpResponseHeader.Location];
				url = location;
			}
			req = (HttpWebRequest)WebRequest.Create(url);
			req.CookieContainer = new CookieContainer();
			req.UserAgent = USERAGENT;
			return req;
		}
		
		private string ReadAllString(WebRequest req, Encoding encoding)
		{
			using (WebResponse  res = req.GetResponse()) {
			using (StreamReader istr = new StreamReader(res.GetResponseStream(), encoding)){	// 決めうち良くない＾＾；
				HttpWebResponse httpRes = (HttpWebResponse)res;
				String str = istr.ReadToEnd();
				return str;
			}}
		}


		/*-----------------------------------------------------*
		 *	デバッグ
		 *-----------------------------------------------------*/
		static Random random = new Random();
		static private void OutputHtml(string html)
		{
			string fileName = string.Format("TVOukoku_{0:x}.html", random.Next());
			string filePath = Path.GetTempPath() + fileName;

			using(TextWriter w = new StreamWriter(filePath, false, Encoding.UTF8)) {
				w.Write(html);
			}
			_D("Output received html in {0}", filePath);
		}

		static private void _D(string fmt, params object[] obj)
		{
			string text = string.Format(fmt, obj);
			System.Diagnostics.Debug.WriteLine(text);
		}

		/**
		 *
		 */
		public void ParserTest()
		{
			TextReader tr = new StreamReader(@"K:\CPP\C#\magicanime_recordplugin_sample_071205\schedulesBySearch.action", UTF_8);
			string     str = tr.ReadToEnd();

			foreach (TVOukokuRecord p in TVOukokuParser.Parse(str))
			{
				System.Console.Out.WriteLine(p);
			}
		}
	};
}

