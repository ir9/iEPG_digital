using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using DEBUG = System.Diagnostics.Debug;

namespace mAgicTvPlugIn
{
	class TVOukokuParser
	{
		private class NotFoundException : Exception
		{
			public NotFoundException() {}
			public NotFoundException(string m) : base(m){}
		};

		private class NotHaveIEPGException : Exception
		{};

		static readonly private Regex SPLIT         = new Regex(@"<div\s+class\s*=\s*""\s*utileList\s*""\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		static readonly private Regex TITLE_GETTER  = new Regex(@"<h2><a href=""/schedule/\d+.action"">(.+?)</a></h2>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
															//  <a id="iepg-101072201007051930-1" name="iepg-button" href="/iepg.tvpid?id=101072201007051930" class="iepg" title="iEPGÉfÉWÉ^Éã">
		static readonly private Regex iEPG_URL      = new Regex(@"<a\s+id\s*=\s*""iepg-\d+-\d+""\s+name\s*=\s*""iepg-button""\s+href\s*=\s*""(/iepg\.tvpid\?id=\d+)"".+?>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		static readonly private Regex DELETE_FOOTER = new Regex(@"<div\s+class\s*=\s*""listIndexNum.+?"".*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		static readonly private Regex DateTimePlus = new Regex(@"<p\s+class\s*=\s*""utileListProperty""\s*>.+?</p\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		static readonly private Regex LINE_SPLITER = new Regex(@"\n", RegexOptions.Compiled);
																	// 7/7 (êÖ) 2:30 Å` 3:00          (30ï™) 
		static readonly private Regex DATATIME_PARSER = new Regex(@"(?<month>\d+)/(?<day>\d+)\s+\(\s*(?<week>.+?)\s*\)\s+(?<hour>\d+):(?<minute>\d+)\s*Å`\s*\d+:\d+", RegexOptions.Compiled);
		static readonly private Regex DELETE_CH       = new Regex(@"\(Ch.\d+\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private TVOukokuParser() {

		}

		static public List<TVOukokuRecord> Parse(string html)
		{
			_D("Enter TVOukokuParser::Prase()");
			TVOukokuParser p = new TVOukokuParser();
			return p.Parse_(html);
		}

		private List<TVOukokuRecord> Parse_(string html)
		{
			List<TVOukokuRecord> ret = new List<TVOukokuRecord>();

			html = html.Replace("utileList bl", "utileList");
			string[] str = SplitString(html);	_D("record list = {0}", str.Length);

			foreach(string s in str)
			{
				TVOukokuRecord rec = new TVOukokuRecord();

				try
				{
					GetTitle(s, rec);
					AnalyzeDateTimePlus(s, rec);
					GetIEPGUrl(s, rec);
					ret.Add(rec);
				}
				catch (NotHaveIEPGException)
				{ /* pass */ }
				catch (Exception e) {
					_D("** unexpected format record **\r\n{0}*********************{1}\r\n", e.ToString(), s);
				}
			}

			return ret;
		}

		private void GetTitle(string part, TVOukokuRecord target)
		{
			_D("<0> Enter GetTitle()");
			Match m = TITLE_GETTER.Match(part);
			if(!m.Success)
				throw new NotFoundException();
			target.title = m.Groups[1].Value; _D("<0> get title : {0}", target.title);
		}

		private void GetIEPGUrl(string part, TVOukokuRecord target)
		{
			_D("<2> Enter GetIEPGUrl()");
			Match m = iEPG_URL.Match(part);
			if(!m.Success)
				throw new NotHaveIEPGException();

			// success!
			string url = m.Groups[1].Value;
			target.iepgURL = url; _D("<2> ipegURL = {0}", url);
		}

		private void AnalyzeDateTimePlus(string part, TVOukokuRecord target)
		{
			_D("<1> Enter AnalyzeDateTimePlus()");
			Match m = DateTimePlus.Match(part);
			if(!m.Success)
				throw new NotFoundException();
			string   datatime_plus = m.Groups[0].Value;	 _D("<1> extract section of dattime and tvStation : {0}", datatime_plus);
			string[] lines         = LINE_SPLITER.Split(datatime_plus);

			AnalyzeDateTime(lines[1], target);
			AnalyzeTVStation(lines[2], target);
		}

		private void AnalyzeDateTime(string datatime, TVOukokuRecord target)
		{
			_D("<1.1> Enter AnalyzeDateTime()");
			Match m = DATATIME_PARSER.Match(datatime);
			if(!m.Success)
				throw new NotFoundException();
			// (?<month>\d)+/(?<day>\d)+\s+\(\s*(?<week>.\)s*\)\s+(?<hour>\d+):(?<minute>\d+)\s+Å`\s+\d+:\d+
			int month  = int.Parse(m.Groups["month"].Value);  _D("<1.1> month  : {0}", month);
			int day    = int.Parse(m.Groups["day"].Value);    _D("<1.1> day    : {0}", day);
			int hour   = int.Parse(m.Groups["hour"].Value);   _D("<1.1> hour   : {0}", hour);
			int minute = int.Parse(m.Groups["minute"].Value); _D("<1.1> minute : {0}", minute);

			// îNìxâzÇµÇ…ëŒâûÇ∑ÇÈÇÃÇ©ÇµÇÁ
			DateTime now = DateTime.Now;
			int nowYear  = now.Year;
			if(now.Month == 12 && month == 1)
				++nowYear;
			DateTime startTime = new DateTime(nowYear, month, day, hour, minute, 0);

			// success!
			target.startTime = startTime; _D("<1.1> datetime : {0}", target.startTime.ToString("s"));
		}

		private void AnalyzeTVStation(string station, TVOukokuRecord target)
		{
			_D("<1.2> Enter AnalyzeTVStation()");
			_D("<1.2> station = {0}", station);

			station = DELETE_CH.Replace(station, "");              _D("<1.2> station = {0}", station);
			station = station.ToUpper();                           _D("<1.2> station = {0}", station);
			station = station.Trim();                              _D("<1.2> station = {0}", station);
			station = Strings.StrConv(station, VbStrConv.Wide, 0); _D("<1.2> station = {0}", station);

			// success!!
			target.tvStation = station; _D("<1.2> tvStation : {0}", target.tvStation);
		}

		private string[] SplitString(string html)
		{
			string[] split = SPLIT.Split(html);
			// ç≈å„ÇÃóvëfÇ…ÇÕÉSÉ~Ç™Ç¬Ç¢ÇƒÇÈÇÃÇ≈çÌèúÇ∑ÇÈ
			split[split.Length - 1] = DELETE_FOOTER.Replace(split[split.Length - 1], "");

			// êÊì™ÇÃóvëfÇÕóvÇÁÇ»Ç¢éq
			string[] arr = new string[split.Length - 1];
			Array.Copy(split, 1, arr, 0, arr.Length);
			return arr;
		}

		/*------------------------------------------------*
		 *	debug
		 *------------------------------------------------*/
		private static void _D(string fmt, params object[] obj) {
			string text = string.Format(fmt, obj);
			System.Diagnostics.Debug.WriteLine(text);
		}
	}
}
